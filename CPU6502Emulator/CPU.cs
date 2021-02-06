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
            var firstLoc = ReadShort(ref pc, ref cycles);
            pc = firstLoc;
        }

        byte ReadByte(ushort address, ref int cycle)
        {
            cycle--;
            return this[address];
        }

        ushort ReadShort(ref ushort loByte, ref int cycles)
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
            flags |= (value & 0b10000000) > 0 ? Flags.N : 0;
            flags |= value == 0 ? Flags.Z : 0;
            
            cycles--;
        }

        public void RunProgram(ref int cycles)
        {
            while (cycles > 0)
            {
                int tempPc = pc;
                RunLDA(ref cycles);
                RunLDX(ref cycles);

                if (tempPc == pc)
                    throw new Exception($"Unrecognised opcode {pc:x8}");

                if (cycles < 0)
                    throw new Exception("Not enough cycles!");
            }
        }

        void RunLDA(ref int cycles)
        {
            if (cycles <= 0)
                return;
            
            switch (this[pc])
            {
                case (byte) OpCode.LDAI:
                {
                    A = ReadByte(++pc, ref cycles);
                    pc++;
                    SetLoadFlags(A, ref cycles);
                }return;
                case (byte) OpCode.LDAZ:
                {
                    pc++;
                    A = LoadZero(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAZX:
                {
                    pc++;
                    A = LoadZeroX(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAA:
                {
                    pc++;
                    A = LoadAbsolute(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAAX:
                {
                    pc++;
                    A = LoadAbsoluteX(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                } return;
                case (byte) OpCode.LDAAY:
                {
                    pc++;
                    A = LoadAbsoluteY(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                }return;
                case (byte) OpCode.LDAIX:
                {
                    pc++;
                    A = LoadIndirectX(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                }return;
                case (byte) OpCode.LDAIY:
                {
                    pc++;
                    A = LoadIndirectY(ref pc, ref cycles);
                    SetLoadFlags(A, ref cycles);
                }return;
            }
        }

        void RunLDX(ref int cycles)
        {
            if (cycles <= 0)
                return;
            
            switch (this[pc])
            {
                case (byte) OpCode.LDXI:
                {
                    X = ReadByte(++pc, ref cycles);
                    pc++;
                    SetLoadFlags(X, ref cycles);
                }return;
                case (byte) OpCode.LDXZ:
                {
                    throw new NotImplementedException();
                }return;
                case (byte) OpCode.LDXZY:
                {
                    throw new NotImplementedException();
                }return;
                case (byte) OpCode.LDXA:
                {
                    throw new NotImplementedException();
                }return;
                case (byte) OpCode.LDXAY:
                {
                    throw new NotImplementedException();
                }return;
            }
        }
        
        /// <summary>
        /// Load byte from zero page
        /// </summary>
        byte LoadZero(ref ushort pointer, ref int cycles)
        {
            ushort address = ReadByte(pointer++, ref cycles);
            return ReadByte(address, ref cycles);
        }
        /// <summary>
        /// Load byte with Zero X mode
        /// </summary>
        byte LoadZeroX(ref ushort pointer, ref int cycles)
        {
            ushort address = ReadByte(pointer++, ref cycles);
            address += X;
            address %= 0x100; // if we have escaped the 0 page then wrap around back to the start of it
            cycles--; // extra for adding x+ to address
            return ReadByte(address, ref cycles);
        }
        
        /// <summary>
        /// Load byte with absolute mode
        /// </summary>
        byte LoadAbsolute(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            pointer++;
            return ReadByte(address, ref cycles);
        }
        /// <summary>
        /// Load byte with absolute X mode
        /// </summary>
        byte LoadAbsoluteX(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            pointer++;

            if (CrossesBoundary(address, X, 0x10000))
                cycles--;

            return ReadByte((ushort) ((address + X) % 0x10000), ref cycles);
        }
        /// <summary>
        /// Load byte with absolute Y mode
        /// </summary>
        byte LoadAbsoluteY(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            pointer++;

            if (CrossesBoundary(address, Y, 0x10000))
                cycles--;

            return ReadByte((ushort) ((address + Y) % 0x10000), ref cycles);
        }
        
        /// <summary>
        /// Load byte with Indirect X mode 
        /// </summary>
        byte LoadIndirectX(ref ushort pointer, ref int cycles)
        {
            ushort zeroAddress = ReadByte(pc++, ref cycles);
                    
            zeroAddress += X;
            cycles--;

            var dataAddress = ReadShort(ref zeroAddress, ref cycles);

            return ReadByte(dataAddress, ref cycles);
        }
        /// <summary>
        /// Load byte with Indirect Y mode 
        /// </summary>
        byte LoadIndirectY(ref ushort pointer, ref int cycles)
        {
            ushort zeroAddress = ReadByte(pointer++, ref cycles);
            var dataAddress = ReadShort(ref zeroAddress, ref cycles);

            if (CrossesBoundary(dataAddress, Y, 0x10000))
                cycles--;

            dataAddress += Y;

            return ReadByte(dataAddress, ref cycles);
        }

        public byte this[ushort index]
        {
            get => memory[index];
            set => memory[index] = value;
        }
    }
}