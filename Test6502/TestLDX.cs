using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestLDX
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void LoadImmediatePositive()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x37;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateZero()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x0;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x80;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
    }
}