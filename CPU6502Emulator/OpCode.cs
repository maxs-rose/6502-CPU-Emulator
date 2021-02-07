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
        LDYAX = 0xBC,
        
        // STA
        STAZ = 0x85,
        STAZX = 0x95,
        STAA = 0x8D,
        STAAX = 0x9D,
        STAAY = 0x99,
        STAIX = 0x81,
        STAIY = 0x91,
        
        // STX
        STXZ = 0x86,
        STXZY = 0x96,
        STXA = 0x8E,
        
        // STY
        STYZ = 0x84,
        STYZX = 0x94,
        STYA = 0x8C,
        
        // JMP
        JMPI = 0x4C,
        JMPIN = 0x6C,
        
        // RTS
        RTS = 0x60,
        
        // JSR
        JSR = 0x20,
        
        // INC
        INCZ = 0xE6,
        INCZX = 0xF6,
        INCA = 0xEE,
        INCX = 0xFE,
        
        // INX
        INX = 0xE8,
        
        // INY
        INY = 0xC8,
        
        // Transfers
        TAX = 0xAA,
        TAY = 0xA8,
        TSX = 0xBA,
        TXA = 0x8A,
        TXS = 0x9A,
        TYA = 0x98,
        
        // Stack
        PHA = 0x48,
        PHP = 0x08,
        PLA = 0x68,
        PLP = 0x28,
        
        // AND
        ANDI = 0x29,
        ANDZ = 0x25,
        ANDZX = 0x35,
        ANDA = 0x2D,
        ANDAX = 0x3D,
        ANDAY = 0x39,
        ANDIX = 0x21,
        ANDIY = 0x31,
        
        // EOR
        EORI = 0x49,
        EORZ = 0x45,
        EORZX = 0x55,
        EORA = 0x4D,
        EORAX = 0x5D,
        EORAY = 0x59,
        EORIX = 0x41,
        EORIY = 0x51,
        
        // ORA
        ORAI = 0x09,
        ORAZ = 0x05,
        ORAZX = 0x15,
        ORAA = 0x0D,
        ORAAX = 0x1D,
        ORAAY = 0x19,
        ORAIX = 0x01,
        ORAIY = 0x11,
        
        // BIT
        BITZ = 0x24,
        BITA = 0x2C,
    }
}