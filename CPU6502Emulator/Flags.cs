using System;

namespace CPU6502Emulator
{
    [Flags]
    public enum Flags : byte
    {
        C = 0b00000001, // Carry
        Z = 0b00000010, // Zero
        I = 0b00000100, // Interrupt dsable
        D = 0b00001000, // decimal mode
        B = 0b00110000, // break
        V = 0b01000000, // overflow
        N = 0b10000000  // negative 
    };
}