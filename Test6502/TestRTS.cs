using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestRTS
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase((ushort)0x3969)]
        [TestCase((ushort)0x0160)]
        [TestCase((ushort)0x1F00)]
        [TestCase((ushort)0x6969)]
        public void RTS(ushort address)
        {
            cpu.flags = (Flags)7;
            var preFlags = cpu.flags;
            cpu[0x0100] = (byte)OpCode.JSR;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = (byte)OpCode.RTS;

            var cycles = 6 + 6; // JSR + RTS
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0103, cpu.pc);
            Assert.AreEqual(0xFF, cpu.sp);
            Assert.AreEqual(preFlags, cpu.flags);
        }
    }
}