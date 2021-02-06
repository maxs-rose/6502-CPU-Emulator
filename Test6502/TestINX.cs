using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestINX
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void IncrementXPositive()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.INX;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(2, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }

        [Test]
        public void IncrementXNegative()
        {
            cpu.X = 0x7F;
            cpu[0x0100] = (byte)OpCode.INX;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }

        [Test]
        public void IncrementXZero()
        {
            cpu.X = 0xFF;
            cpu[0x0100] = (byte)OpCode.INX;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
    }
}