using CPU6502Emulator;
using FluentAssertions;
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

        [TestCase(1, 2, (Flags)0, TestName = "Increment a value")]
        [TestCase(0x7F, 0x80, Flags.N, TestName = "Increment to negative value")]
        [TestCase(0xFF, 0, Flags.Z, TestName = "Increment to 0")]
        public void IncrementY(byte yValue, byte expectedY, Flags expectedFlags)
        {
            cpu.Y = yValue;
            cpu[0x0100] = (byte)OpCode.INY;

            var cycles = 2;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(expectedY);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
        }
    }
}