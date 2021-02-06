using System;

namespace CPU6502Emulator
{
    // byte = 8 bits
    // short = 16 bits
    
    // Zero page 0000-00FF
    // Second page 0100-01FF
    // FFFA/B = no mask interrupt
    // FFFC/D = power/reset vector
    // FFFE/F = BRK/interrupt handler
    
    // 0000 - Start 0 (Start of memory)
    // 00FF - End 0
    // 0100 - System stack
    // 01FF - System stack
    // 0200 - mem
    // FFF9 - mem
    // FFFA - misc
    // FFFF - misc (end of memory)

    public class CPU
    {
        // Memory
        public Memory memory;

        // Registers
        public ushort pc;
        public int sp; // points to system stack
        public byte A;
        public byte X;
        public byte Y;
        // - Status flags
        public Flags flags;
        // c - carry
        // z - zero
        // i - interrupt disable
        // d - decimal mode
        // b - break
        // v - overflow (looks at bits 6 and 7 to check for validness, ie 0111 1111 + 0111 1111 != 1111 1111 as that is negative)
        // n - negative (bit 7 is set)

        // all we want to do here is create the memory we dont need to init anything yet, that is the job if the power on/reset sequence
        private CPU() { memory = new Memory(0xFFFF); } 
        
        public static CPU PowerOn()
        {
            var cpu = new CPU();
            
            return cpu;
        }
        
        public void Reset()
        {
            memory.ZeroMemory();
            
            this[0xFFFC] = 0x00;
            this[0xFFFD] = 0x01;
            
            sp = 0x0100;

            flags = 0;
            A = X = Y = 0;
            
            // location of first usable opcode
            pc = 0xFFFC;
            var cycles = 2;
            var firstLoc = GetShortAddress(ref pc, ref cycles);
            pc = firstLoc;
        }

        byte ReadByte(ushort address, ref int cycle)
        {
            cycle--;
            return memory[address];
        }

        ushort GetShortAddress(ref ushort loByte, ref int cycles)
        {
            ushort dataAddress = ReadByte(loByte++, ref cycles);
            dataAddress |= (ushort)(ReadByte(loByte, ref cycles) << 8);

            return dataAddress;
        }

        bool CrossesBoundary(int v1, int v2, int mod)
        {
            var temp = v1 + v2;
            temp %= mod;

            return (temp & 0xFF00) != (v1 & 0xFF00);
        }

        void SetLoadFlags(int value, ref int cycles)
        {
            flags |= (A & 0b10000000) > 0 ? Flags.N : 0;
            flags |= A == 0 ? Flags.Z : 0;
            
            cycles--;
        }

        public void RunProgram(ref int cycles)
        {
            while (cycles > 0)
            {
                int tempPc = pc;
                RunLDA(ref cycles);

                if (tempPc == pc)
                    throw new Exception($"Unrecognised opcode {pc:x8}");

                if (cycles < 0)
                    throw new Exception("Not enough cycles!");
            }
        }

        void RunLDA(ref int cycles)
        {
            switch (memory[pc++])
            {
                case (byte) OpCode.LDAI:
                    // load into A register
                    A = ReadByte(pc++, ref cycles);

                    SetLoadFlags(A, ref cycles);
                    return;
                case (byte) OpCode.LDAZ:
                {
                    ushort address = ReadByte(pc++, ref cycles);
                    A = ReadByte(address, ref cycles);

                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAZX:
                {
                    ushort address = ReadByte(pc++, ref cycles);
                    A = ReadByte((ushort) (address + X), ref cycles);
                    cycles--; // extra for adding x+ to address

                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAA:
                {
                    var address = GetShortAddress(ref pc, ref cycles);
                    pc++;
                    A = ReadByte(address, ref cycles);

                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAAX:
                {
                    var address = GetShortAddress(ref pc, ref cycles);
                    pc++;

                    if (CrossesBoundary(address, X, 0xFFFF))
                        cycles--;

                    A = ReadByte((ushort) ((address + X) % 0xFFFF), ref cycles);

                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAAY:
                {
                    var address = GetShortAddress(ref pc, ref cycles);
                    pc++;

                    if (CrossesBoundary(address, Y, 0xFFFF))
                        cycles--;

                    A = ReadByte((ushort) ((address + Y) % 0xFFFF), ref cycles);

                    SetLoadFlags(A, ref cycles);
                }return;
                case (byte) OpCode.LDAIX:
                {
                    ushort zeroAddress = ReadByte(pc++, ref cycles);
                    
                    zeroAddress += X;
                    cycles--;

                    var dataAddress = GetShortAddress(ref zeroAddress, ref cycles);

                    A = ReadByte(dataAddress, ref cycles);

                    SetLoadFlags(A, ref cycles);
                }return;
                case (byte) OpCode.LDAIY:
                {
                    ushort zeroAddress = ReadByte(pc++, ref cycles);

                    var dataAddress = GetShortAddress(ref zeroAddress, ref cycles);

                    if (CrossesBoundary(dataAddress, Y, 0xFFFF))
                        cycles--;

                    dataAddress += Y;
                    dataAddress %= 0xFFFF;

                    A = ReadByte(dataAddress, ref cycles);

                    SetLoadFlags(A, ref cycles);
                }return;
            }
        }

        public byte this[ushort index]
        {
            get => memory[index];
            set => memory[index] = value;
        }
    }
}