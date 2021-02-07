using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestJSR
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void JSR()
        {
            cpu.flags = (Flags)7;
            var preFlags = cpu.flags;
            cpu[0x0100] = (byte)OpCode.JSR;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x01;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0169, cpu.pc);
            Assert.AreEqual(0xFD, cpu.sp);
            Assert.AreEqual(0x02, cpu[(ushort)(cpu.sp + 1)]);
            Assert.AreEqual(0x01, cpu[(ushort)(cpu.sp + 2)]);
            Assert.AreEqual(preFlags, cpu.flags);
        }
    }
}