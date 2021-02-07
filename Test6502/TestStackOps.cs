using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestStackOps
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0x37)]
        [TestCase(0x0)]
        [TestCase(0x80)]
        public void PHA(byte value)
        {
            cpu.flags = (Flags)7;
            cpu.A = value;
            var preFlags = cpu.flags;
            
            cpu[0x0100] = (byte)OpCode.PHA;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[++cpu.sp]);
            Assert.AreEqual(0x0101, cpu.pc);
            Assert.AreEqual(preFlags, cpu.flags);
        }

        [TestCase((Flags)0x37)]
        [TestCase((Flags)0x0)]
        [TestCase((Flags)0x80)]
        public void PHP(Flags value)
        {
            cpu.flags = value;
            var preFlags = cpu.flags;
            
            cpu[0x0100] = (byte)OpCode.PHP;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(cpu.flags, (Flags)cpu[++cpu.sp]);
            Assert.AreEqual(value, (Flags)cpu[cpu.sp]);
            Assert.AreEqual(value, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
            Assert.AreEqual(preFlags, cpu.flags);
        }

        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void PLA(byte value, Flags flags)
        {
            cpu[cpu.sp--] = value;
            cpu[0x0100] = (byte)OpCode.PLA;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu.A);
            Assert.AreEqual(0x0101, cpu.pc);
            Assert.AreEqual(flags, cpu.flags);
        }

        [TestCase((Flags)0)]
        [TestCase(Flags.Z)]
        [TestCase(Flags.N)]
        [TestCase((Flags)0xF0)]
        [TestCase((Flags)0x5D)]
        [TestCase((Flags)0x44)]
        public void PLP(Flags flags)
        {
            cpu.flags = ~cpu.flags;
            cpu[cpu.sp--] = (byte)flags;
            cpu[0x0100] = (byte)OpCode.PLP;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0101, cpu.pc);
            Assert.AreEqual(flags, cpu.flags);
        }
    }
}