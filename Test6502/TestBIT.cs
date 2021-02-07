using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestBIT
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }
        
        [TestCase(0xFF, 0x0, Flags.Z)]
        [TestCase(0b0110_0100, 0b0000_0100, (Flags)0)]
        [TestCase(0b0110_0100, 0b0100_0100, Flags.V)]
        [TestCase(0b0010_0100, 0b0100_0100, Flags.V)]
        [TestCase(0b1010_0100, 0b1100_0100, Flags.N | Flags.V)]
        [TestCase(0, 0b1100_0100, Flags.N | Flags.V | Flags.Z)]
        public void BITZP(byte acc, byte mask, Flags flags)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.BITZ;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = mask;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(flags, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [TestCase(0xFF, (ushort)0xFF01, 0x0, Flags.Z)]
        [TestCase(0b0110_0100, (ushort)0xFE09, 0b0000_0100, (Flags)0)]
        [TestCase(0b0110_0100, (ushort)0xFE09, 0b0100_0100, Flags.V)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0b0100_0100, Flags.V)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0b1100_0100, Flags.N | Flags.V)]
        [TestCase(0, (ushort)0xFF01, 0b1100_0100, Flags.N | Flags.V | Flags.Z)]
        public void BITA(byte acc, ushort address, byte mask, Flags flags)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.BITA;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = mask;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(flags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
    }
}