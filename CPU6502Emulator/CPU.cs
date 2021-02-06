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
        private Memory memory;

        private delegate void OpcodeArray(ref ushort pointer, ref int cycles);
        private OpcodeArray[] opcodeAray;

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
        private CPU()
        {
            memory = new Memory(0xFFFF);

            opcodeAray = new OpcodeArray[0xFF];
            Array.Fill(opcodeAray,
                (ref ushort pointer, ref int _) => throw new Exception($"Opcode not recognised {pointer:x8}"));

            // LDA
            opcodeAray[(int) OpCode.LDAI] = LDAI;
            opcodeAray[(int) OpCode.LDAZ] = LDAZ;
            opcodeAray[(int) OpCode.LDAZX] = LDAZX;
            opcodeAray[(int) OpCode.LDAZX] = LDAZX;
            opcodeAray[(int) OpCode.LDAA] = LDAA;
            opcodeAray[(int) OpCode.LDAAX] = LDAAX;
            opcodeAray[(int) OpCode.LDAAY] = LDAAY;
            opcodeAray[(int) OpCode.LDAIX] = LDAIX;
            opcodeAray[(int) OpCode.LDAIY] = LDAIY;
            
            // LDX
            opcodeAray[(int) OpCode.LDXI] = LDXI;
            opcodeAray[(int) OpCode.LDXZ] = LDXZ;
            opcodeAray[(int) OpCode.LDXZY] = LDXZY;
            opcodeAray[(int) OpCode.LDXA] = LDXA;
            opcodeAray[(int) OpCode.LDXAY] = LDXAY;
            
            // LDY
            opcodeAray[(int) OpCode.LDYI] = LDYI;
            opcodeAray[(int) OpCode.LDYZ] = LDYZ;
            opcodeAray[(int) OpCode.LDYZX] = LDYZX;
            opcodeAray[(int) OpCode.LDYA] = LDYA;
            opcodeAray[(int) OpCode.LDYAX] = LDYAX;
        }

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
            dataAddress |= (ushort) (ReadByte(loByte, ref cycles) << 8);

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
                try
                {
                    opcodeAray[this[pc++]](ref pc, ref cycles);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException($"Opcode does not exist in 6502!");
                }

                if (cycles < 0)
                    throw new Exception("Not enough cycles!");
            }
        }

        #region LDA

        void LDAI(ref ushort pointer, ref int cycles)
        {
            A = ReadByte(pc++, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAZ(ref ushort pointer, ref int cycles)
        {
            A = LoadZero(ref pc, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAZX(ref ushort pointer, ref int cycles)
        {
            A = LoadZeroX(ref pointer, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAA(ref ushort pointer, ref int cycles)
        {
            A = LoadAbsolute(ref pointer, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAAX(ref ushort pointer, ref int cycles)
        {
            A = LoadAbsoluteX(ref pointer, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAAY(ref ushort pointer, ref int cycles)
        {
            A = LoadAbsoluteY(ref pointer, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAIX(ref ushort pointer, ref int cycles)
        {
            A = LoadIndirectX(ref pc, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        void LDAIY(ref ushort pointer, ref int cycles)
        {
            A = LoadIndirectY(ref pointer, ref cycles);
            SetLoadFlags(A, ref cycles);
        }

        #endregion

        #region LDX

        void LDXI(ref ushort pointer, ref int cycles)
        {
            X = ReadByte(pointer++, ref cycles);
            SetLoadFlags(X, ref cycles);
        }

        void LDXZ(ref ushort pointer, ref int cycles)
        {
            X = LoadZero(ref pointer, ref cycles);
            SetLoadFlags(X, ref cycles);
        }

        void LDXZY(ref ushort pointer, ref int cycles)
        {
            X = LoadZeroY(ref pointer, ref cycles);
            SetLoadFlags(X, ref cycles);
        }

        void LDXA(ref ushort pointer, ref int cycles)
        {
            X = LoadAbsolute(ref pointer, ref cycles);
            SetLoadFlags(X, ref cycles);
        }

        void LDXAY(ref ushort pointer, ref int cycles)
        {
            X = LoadAbsoluteY(ref pointer, ref cycles);
            SetLoadFlags(X, ref cycles);
        }

        #endregion

        #region LDY

        void LDYI(ref ushort pointer, ref int cycles)
        {
            Y = ReadByte(pointer++, ref cycles);
            SetLoadFlags(Y, ref cycles);
        }

        void LDYZ(ref ushort pointer, ref int cycles)
        {
            Y = LoadZero(ref pointer, ref cycles);
            SetLoadFlags(Y, ref cycles);
        }

        void LDYZX(ref ushort pointer, ref int cycles)
        {
            Y = LoadZeroX(ref pointer, ref cycles);
            SetLoadFlags(Y, ref cycles);
        }

        void LDYA(ref ushort pointer, ref int cycles)
        {
            Y = LoadAbsolute(ref pointer, ref cycles);
            SetLoadFlags(Y, ref cycles);
        }

        void LDYAX(ref ushort pointer, ref int cycles)
        {
            Y = LoadAbsoluteX(ref pointer, ref cycles);
            SetLoadFlags(Y, ref cycles);
        }

        #endregion

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
        /// Load byte with Zero Y mode
        /// </summary>
        byte LoadZeroY(ref ushort pointer, ref int cycles)
        {
            ushort address = ReadByte(pointer++, ref cycles);
            address += Y;
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