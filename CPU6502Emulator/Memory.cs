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

        public byte this[ushort key]
        {
            get => memory[key];
            set => memory[key] = value;
        }
    }
}