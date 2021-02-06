namespace CPU6502Emulator
{
    public enum OpCode : byte
    {
        // LDA
        LDAI = 0xA9,
        LDAZ = 0xA5,
        LDAZX = 0xB5,
        LDAA = 0xAD,
        LDAAX = 0xBD,
        LDAAY = 0xB9,
        LDAIX = 0xA1,
        LDAIY = 0xB1,
        
        // LDX
        LDXI = 0xA2,
        LDXZ = 0xA6,
        LDXZY = 0xB6,
        LDXA = 0xAE,
        LDXAY = 0xBE,
        
        // LDY
        LDYI = 0xA0,
        LDYZ = 0xA4,
        LDYZX = 0xB4,
        LDYA = 0xAC,
        LDYAX = 0xBC
    }
}