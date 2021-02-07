namespace CPU6502Emulator
{
    public struct Memory
    {
        private readonly byte[] memory; 
        
        public Memory(int memorySize)
        {
            memory = new byte[memorySize];
        }

        public void ZeroMemory()
        {
            for (var i = 0; i < memory.Length; i++)
                memory[i] = 0;
        }

        public void WriteStackByte(byte value, ref byte address, ref int cycles)
        {
            this[address--] = value;
            cycles--;
        }
        
        public void WriteStackShort(ushort value, ref byte loAddress, ref int cycles)
        {
            WriteStackByte((byte)(value >> 8), ref loAddress, ref cycles);
            WriteStackByte((byte)(value & 0xFF), ref loAddress, ref cycles);
        }

        public byte PopStack(ref byte address, ref int cycles)
        {
            cycles--;
            return this[address++];
        }
        
        public ushort ReadStackShort(ref byte loAddress, ref int cycles)
        {
            ushort value = PopStack(ref loAddress, ref cycles);
            value |= (ushort)(PopStack(ref loAddress, ref cycles) << 8);

            return value;
        }

        public byte this[ushort key]
        {
            get => memory[key];
            set => memory[key] = value;
        }
    }
}