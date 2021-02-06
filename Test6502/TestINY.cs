using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestINY
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void IncrementYPositive()
        {
            cpu.Y = 1;
            cpu[0x0100] = (byte)OpCode.INY;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(2, cpu.Y);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }

        [Test]
        public void IncrementYNegative()
        {
            cpu.Y = 0x7F;
            cpu[0x0100] = (byte)OpCode.INY;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.Y);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }

        [Test]
        public void IncrementYZero()
        {
            cpu.Y = 0xFF;
            cpu[0x0100] = (byte)OpCode.INY;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0, cpu.Y);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
    }
}