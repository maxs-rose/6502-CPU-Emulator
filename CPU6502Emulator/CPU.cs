﻿using System;
using System.Collections.Generic;
using CPU6502Emulator.Exceptions;

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
        private Memory memory = new Memory(0xFFFF);

        private delegate void OpcodeArray(ref ushort pointer, ref int cycles);

        private readonly OpcodeArray[] opcodeAray = new OpcodeArray[0xFF];

        // Registers
        public ushort pc;
        public byte sp; // points to system stack
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
        public bool C => (flags & Flags.C) > 0;
        public bool Z => (flags & Flags.Z) > 0;
        public bool I => (flags & Flags.I) > 0;
        public bool D => (flags & Flags.D) > 0;
        public bool B => (flags & Flags.B) > 0;
        public bool V => (flags & Flags.V) > 0;
        public bool N => (flags & Flags.N) > 0;

        // all we want to do here is create the memory we dont need to init anything yet, that is the job if the power on/reset sequence

        private CPU()
        {
            Array.Fill(opcodeAray,
                (ref ushort pointer, ref int _) => throw new OpCodeNotImplementedException($"Opcode {this[(ushort) (pointer - 1)]:X} is not implemented"));

            // LDA
            opcodeAray[(int) OpCode.LDAI] = LDAI;
            opcodeAray[(int) OpCode.LDAZ] = LDAZ;
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

            // JMP
            opcodeAray[(int) OpCode.JMPI] = JMPI;
            opcodeAray[(int) OpCode.JMPIN] = JMPIN;
            
            // RTS
            opcodeAray[(int) OpCode.RTS] = RTS;

            // JSR
            opcodeAray[(int) OpCode.JSR] = JSR;

            // INC
            opcodeAray[(int) OpCode.INCZ] = INCZ;
            opcodeAray[(int) OpCode.INCZX] = INCZX;
            opcodeAray[(int) OpCode.INCA] = INCA;
            opcodeAray[(int) OpCode.INCX] = INCX;

            // INX
            opcodeAray[(int) OpCode.INX] = INX;

            // INY
            opcodeAray[(int) OpCode.INY] = INY;

            // STA
            opcodeAray[(int) OpCode.STAZ] = STAZ;
            opcodeAray[(int) OpCode.STAZX] = STAZX;
            opcodeAray[(int) OpCode.STAA] = STAA;
            opcodeAray[(int) OpCode.STAAX] = STAAX;
            opcodeAray[(int) OpCode.STAAY] = STAAY;
            opcodeAray[(int) OpCode.STAIX] = STAIX;
            opcodeAray[(int) OpCode.STAIY] = STAIY;

            // STX
            opcodeAray[(int) OpCode.STXZ] = STXZ;
            opcodeAray[(int) OpCode.STXZY] = STXZY;
            opcodeAray[(int) OpCode.STXA] = STXA;

            // STY
            opcodeAray[(int) OpCode.STYZ] = STYZ;
            opcodeAray[(int) OpCode.STYZX] = STYZX;
            opcodeAray[(int) OpCode.STYA] = STYA;
            
            // Transfers
            opcodeAray[(int) OpCode.TAX] = TAX;
            opcodeAray[(int) OpCode.TAY] = TAY;
            opcodeAray[(int) OpCode.TSX] = TSX;
            opcodeAray[(int) OpCode.TXA] = TXA;
            opcodeAray[(int) OpCode.TXS] = TXS;
            opcodeAray[(int) OpCode.TYA] = TYA;
            
            // Stack
            opcodeAray[(int) OpCode.PHA] = PHA;
            opcodeAray[(int) OpCode.PHP] = PHP;
            opcodeAray[(int) OpCode.PLA] = PLA;
            opcodeAray[(int) OpCode.PLP] = PLP;
            
            // AND
            opcodeAray[(int) OpCode.ANDI] = ANDI;
            opcodeAray[(int) OpCode.ANDZ] = ANDZ;
            opcodeAray[(int) OpCode.ANDZX] = ANDZX;
            opcodeAray[(int) OpCode.ANDA] = ANDA;
            opcodeAray[(int) OpCode.ANDAX] = ANDAX;
            opcodeAray[(int) OpCode.ANDAY] = ANDAY;
            opcodeAray[(int) OpCode.ANDIX] = ANDIX;
            opcodeAray[(int) OpCode.ANDIY] = ANDIY;
            
            // EOR
            opcodeAray[(int) OpCode.EORI] = EORI;
            opcodeAray[(int) OpCode.EORZ] = EORZ;
            opcodeAray[(int) OpCode.EORZX] = EORZX;
            opcodeAray[(int) OpCode.EORA] = EORA;
            opcodeAray[(int) OpCode.EORAX] = EORAX;
            opcodeAray[(int) OpCode.EORAY] = EORAY;
            opcodeAray[(int) OpCode.EORIX] = EORIX;
            opcodeAray[(int) OpCode.EORIY] = EORIY;
            
            // ORA
            opcodeAray[(int) OpCode.ORAI] = ORAI;
            opcodeAray[(int) OpCode.ORAZ] = ORAZ;
            opcodeAray[(int) OpCode.ORAZX] = ORAZX;
            opcodeAray[(int) OpCode.ORAA] = ORAA;
            opcodeAray[(int) OpCode.ORAAX] = ORAAX;
            opcodeAray[(int) OpCode.ORAAY] = ORAAY;
            opcodeAray[(int) OpCode.ORAIX] = ORAIX;
            opcodeAray[(int) OpCode.ORAIY] = ORAIY;
            
            // BIT
            opcodeAray[(int) OpCode.BITZ] = BITZ;
            opcodeAray[(int) OpCode.BITA] = BITA;
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

            sp = 0xFF;

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

        void WriteByte(ushort address, byte data, ref int cycle)
        {
            this[address] = data;
            cycle--;
        }

        bool CrossesBoundary(int v1, int v2, int mod)
        {
            var temp = v1 + v2;
            temp %= mod;

            return (temp & 0xFF00) != (v1 & 0xFF00);
        }

        void SetLoadFlags(int value, ref int cycles)
        {
            if ((value & 0b10000000) > 0)
                flags |= Flags.N;
            else
                flags &= ~Flags.N;
            
            if (value == 0)
                flags |= Flags.Z;
            else
                flags &= ~Flags.Z;

            cycles--;
        }

        public void RunProgram(ref int cycles)
        {
            while (cycles > 0)
            {
                try
                {
                    opcodeAray[
                        this[pc++]
                    ](ref pc, ref cycles);
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOpCodeException($"Opcode {this[(ushort)(pc - 1)]:x8} does not exist in 6502!");
                }

                if (cycles < 0)
                    throw new NotEnoughCyclesException("Not enough cycles!");
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

        #region JMP

        void JMPI(ref ushort pointer, ref int cycles)
        {
            pointer = ReadShort(ref pointer, ref cycles);
            cycles--; // extra cycle used to set pc
        }

        void JMPIN(ref ushort pointer, ref int cycles)
        {
            pointer = ReadShort(ref pointer, ref cycles);
            pointer = ReadShort(ref pointer, ref cycles);
            cycles--;
        }

        #endregion

        #region JSR

        void JSR(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            PushShortToSP(pointer, ref cycles);
            pointer = address;
        }

        #endregion

        #region RTS

        void RTS(ref ushort pointer, ref int cycles)
        {
            pointer = ReadShortFromSP(ref cycles);
            cycles -= 3;
            pointer += 1;
            cycles--;
        }

        #endregion

        #region INC

        void INCZ(ref ushort pointer, ref int cycles)
        {
            var zAddress = ReadByte(pointer++, ref cycles);
            cycles--;
            var data = (byte) (ReadByte(zAddress, ref cycles) + 1);

            WriteByte(zAddress, data, ref cycles);
            SetLoadFlags(data, ref cycles);
        }

        void INCZX(ref ushort pointer, ref int cycles)
        {
            var address = LoadZeroXAddress(ref pointer, ref cycles);
            var data = ReadByte(address, ref cycles);
            data += 1;
            WriteByte(address, data, ref cycles);
            SetLoadFlags(data, ref cycles);
            cycles--;
        }

        void INCA(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            pointer++;
            var data = ReadByte(address, ref cycles);
            data += 1;

            WriteByte(address, data, ref cycles);
            SetLoadFlags(data, ref cycles);
            cycles--;
        }

        void INCX(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            pointer++;

            address += X;
            address = (ushort) (address % 0x10000);
            cycles--;

            var data = ReadByte(address, ref cycles);
            data += 1;

            SetLoadFlags(data, ref cycles);

            WriteByte(address, data, ref cycles);
            cycles--;
        }

        #endregion

        #region INX

        void INX(ref ushort pointer, ref int cycles)
        {
            X += 1;
            SetLoadFlags(X, ref cycles);
            cycles--;
        }

        #endregion

        #region INY

        void INY(ref ushort pointer, ref int cycles)
        {
            Y += 1;
            SetLoadFlags(Y, ref cycles);
            cycles--;
        }

        #endregion

        #region STA

        void STAZ(ref ushort pointer, ref int cycles)
        {
            var location = ReadByte(pointer++, ref cycles);
            WriteByte(location, A, ref cycles);
            cycles--;
        }

        void STAZX(ref ushort pointer, ref int cycles)
        {
            var location = ReadByte(pointer++, ref cycles);
            location += X;
            cycles--;
    
            WriteByte(location, A, ref cycles);
            
            cycles--;
        }

        void STAA(ref ushort pointer, ref int cycles)
        {
            var location = ReadShort(ref pointer, ref cycles);
            WriteByte(location, A, ref cycles);

            pointer++;
            cycles--;
        }

        void STAAX(ref ushort pointer, ref int cycles)
        {
            var location = ReadShort(ref pointer, ref cycles);
            location += X;
            location = (ushort) (location % 0x10000);
            cycles--;
            
            WriteByte(location, A, ref cycles);

            pointer++;
            cycles--;
        }

        void STAAY(ref ushort pointer, ref int cycles)
        {
            var location = ReadShort(ref pointer, ref cycles);
            location += Y;
            location = (ushort) (location % 0x10000);
            cycles--;
            
            WriteByte(location, A, ref cycles);

            pointer++;
            cycles--;
        }

        void STAIX(ref ushort pointer, ref int cycles)
        {
            ushort location = ReadByte(pointer++, ref cycles);
            location += X;
            location = (ushort) (location % 0x100);
            cycles--;

            var storeAt = ReadShort(ref location, ref cycles);
            
            WriteByte(storeAt, A, ref cycles);

            cycles--;
        }

        void STAIY(ref ushort pointer, ref int cycles)
        {
            ushort zpLocation = ReadByte(pointer++, ref cycles);
            var storeAt = ReadShort(ref zpLocation, ref cycles);
            
            storeAt += Y;
            storeAt = (ushort) (storeAt % 0x10000);
            cycles--;
            
            WriteByte(storeAt, A, ref cycles);

            cycles--;
        }

        #endregion

        #region STX

        void STXZ(ref ushort pointer, ref int cycles)
        {
            var zpAddress = ReadByte(pointer++, ref cycles);
            WriteByte(zpAddress, X, ref cycles);

            cycles--;
        }

        void STXZY(ref ushort pointer, ref int cycles)
        {
            var zpAddress = ReadByte(pointer++, ref cycles);
            
            zpAddress += Y;
            cycles--;
            
            WriteByte(zpAddress, X, ref cycles);
            
            cycles--;
        }

        void STXA(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            WriteByte(address, X, ref cycles);

            pointer++;
            cycles--;
        }

        #endregion

        #region STY

        void STYZ(ref ushort pointer, ref int cycles)
        {
            var zpAddress = ReadByte(pointer++, ref cycles);
            WriteByte(zpAddress, Y, ref cycles);

            cycles--;
        }

        void STYZX(ref ushort pointer, ref int cycles)
        {
            var zpAddress = ReadByte(pointer++, ref cycles);
            
            zpAddress += X;
            cycles--;
            
            WriteByte(zpAddress, Y, ref cycles);
            
            cycles--;
        }

        void STYA(ref ushort pointer, ref int cycles)
        {
            var address = ReadShort(ref pointer, ref cycles);
            WriteByte(address, Y, ref cycles);

            pointer++;
            cycles--;
        }

        #endregion

        #region Transfers

        void TAX(ref ushort pointer, ref int cycles)
        {
            X = A;
            SetLoadFlags(X, ref cycles);
            cycles--;
        }
        void TAY(ref ushort pointer, ref int cycles)
        {
            Y = A;
            SetLoadFlags(Y, ref cycles);
            cycles--;
        }
        void TSX(ref ushort pointer, ref int cycles)
        {
            X = sp;
            SetLoadFlags(X, ref cycles);
            cycles--;
        }
        void TXA(ref ushort pointer, ref int cycles)
        {
            A = X;
            SetLoadFlags(A, ref cycles);
            cycles--;
        }
        void TXS(ref ushort pointer, ref int cycles)
        {
            sp = X;
            cycles -= 2;
        }
        void TYA(ref ushort pointer, ref int cycles)
        {
            A = Y;
            SetLoadFlags(A, ref cycles);
            cycles--;
        }

        #endregion

        #region Stack

        void PHA(ref ushort pointer, ref int cycles)
        {
            PushByteToSP(A, ref cycles);

            cycles -= 2;
        }
        void PHP(ref ushort pointer, ref int cycles)
        {
            PushByteToSP((byte)flags, ref cycles);

            cycles -= 2;
        }
        void PLA(ref ushort pointer, ref int cycles)
        {
            A = PopByteFromSP(ref cycles);

            SetLoadFlags(A, ref cycles);
            
            cycles--;
        }
        void PLP(ref ushort pointer, ref int cycles)
        {
            flags = (Flags)PopByteFromSP(ref cycles);

            cycles -= 2;
        }

        #endregion

        #region AND

        void ANDI(ref ushort pointer, ref int cycles)
        {
            var data = ReadByte(pointer++, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDZ(ref ushort pointer, ref int cycles)
        {
            var data = LoadZero(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDZX(ref ushort pointer, ref int cycles)
        {
            var data = LoadZeroX(ref pointer, ref cycles);
            cycles++;
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDA(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsolute(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDAX(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteX(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDAY(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteY(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDIX(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectX(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ANDIY(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectY(ref pointer, ref cycles);
            A &= data;
            SetLoadFlags(A, ref cycles);
        }

        #endregion

        #region EOR
        
        void EORI(ref ushort pointer, ref int cycles)
        {
            var data = ReadByte(pointer++, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORZ(ref ushort pointer, ref int cycles)
        {
            var data = LoadZero(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORZX(ref ushort pointer, ref int cycles)
        {
            var data = LoadZeroX(ref pointer, ref cycles);
            cycles++;
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORA(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsolute(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORAX(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteX(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORAY(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteY(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORIX(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectX(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void EORIY(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectY(ref pointer, ref cycles);
            A ^= data;
            SetLoadFlags(A, ref cycles);
        }

        #endregion

        #region ORA
        
        void ORAI(ref ushort pointer, ref int cycles)
        {
            var data = ReadByte(pointer++, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ORAZ(ref ushort pointer, ref int cycles)
        {
            var data = LoadZero(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }

        void ORAZX(ref ushort pointer, ref int cycles)
        {
            var data = LoadZeroX(ref pointer, ref cycles);
            cycles++;
            A |= data;
            SetLoadFlags(A, ref cycles);
        }

        void ORAA(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsolute(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ORAAX(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteX(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ORAAY(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsoluteY(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ORAIX(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectX(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }
        
        void ORAIY(ref ushort pointer, ref int cycles)
        {
            var data = LoadIndirectY(ref pointer, ref cycles);
            A |= data;
            SetLoadFlags(A, ref cycles);
        }

        #endregion

        #region BIT
        
        void BITZ(ref ushort pointer, ref int cycles)
        {
            var data = LoadZero(ref pointer, ref cycles);
            
            if ((data & 0b0100_0000) > 0)
                flags |= Flags.V;
            else
                flags &= ~Flags.V;
            
            if ((data & 0b1000_0000) > 0)
                flags |= Flags.N;
            else
                flags &= ~Flags.N;
            
            data &= A;
            
            if (data == 0)
                flags |= Flags.Z;
            else
                flags &= ~Flags.Z;

            cycles--;
        }
        
        void BITA(ref ushort pointer, ref int cycles)
        {
            var data = LoadAbsolute(ref pointer, ref cycles);
            
            if ((data & 0b0100_0000) > 0)
                flags |= Flags.V;
            else
                flags &= ~Flags.V;
            
            if ((data & 0b1000_0000) > 0)
                flags |= Flags.N;
            else
                flags &= ~Flags.N;
            
            data &= A;
            
            if (data == 0)
                flags |= Flags.Z;
            else
                flags &= ~Flags.Z;

            cycles--;
        }

        #endregion
        
        void PushShortToSP(ushort value, ref int cycles)
        {
            if ((byte)(sp - 2) > sp)
                throw new StackOverflowException();

            memory.WriteStackShort(value, ref sp, ref cycles);
            cycles -= 2;
        }

        ushort ReadShortFromSP(ref int cycles)
        {
            if ((byte)(sp + 2) < sp)
                throw new StackUnderflowException();

            return memory.ReadStackShort(ref sp, ref cycles);
        }

        void PushByteToSP(byte value, ref int cycles)
        {
            if ((byte)(sp - 1) > sp)
                throw new StackOverflowException();

            memory.WriteStackByte(value, ref sp, ref cycles);
        }

        byte PopByteFromSP(ref int cycles)
        {
            if ((byte)(sp + 1) < sp)
                throw new StackUnderflowException();

            cycles--;
            return memory.PopStack(ref sp, ref cycles);
        }

        /// <summary>
        /// Load byte from zero page
        /// </summary>
        byte LoadZero(ref ushort pointer, ref int cycles)
        {
            ushort address = ReadByte(pointer++, ref cycles);
            return ReadByte(address, ref cycles);
        }

        byte LoadZeroXAddress(ref ushort pointer, ref int cycles)
        {
            ushort address = ReadByte(pointer++, ref cycles);
            address += X;
            address %= 0x100; // if we have escaped the 0 page then wrap around back to the start of it
            cycles--;

            return (byte) address;
        }

        /// <summary>
        /// Load byte with Zero X mode
        /// </summary>
        byte LoadZeroX(ref ushort pointer, ref int cycles)
        {
            ushort address = LoadZeroXAddress(ref pointer, ref cycles);
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
            ushort zeroAddress = ReadByte(pointer++, ref cycles);

            zeroAddress += X;
            zeroAddress = (ushort) (zeroAddress % 0x100);
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