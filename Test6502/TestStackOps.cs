using CPU6502Emulator;
using FluentAssertions;
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

            cycles.Should().Be(0);
            cpu[(ushort) (cpu.sp + 1)].Should().Be(value);
            cpu.flags.Should().Be(preFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu[(ushort) (cpu.sp + 1)].Should().Be((byte)cpu.flags);
            cpu[(ushort) (cpu.sp + 1)].Should().Be((byte)value);
            cpu.flags.Should().Be(value);
            cpu.flags.Should().Be(preFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.A.Should().Be(value);
            cpu.pc.Should().Be(0x0101);
            cpu.flags.Should().Be(flags);
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

            cycles.Should().Be(0);
            cpu.pc.Should().Be(0x0101);
            cpu.flags.Should().Be(flags);
        }
    }
}