using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestLDY
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0x37, (Flags)0, TestName = "LDYI Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDYI Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDYI Negative")]
        public void LoadImmediate(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDYI;
            cpu[0x0101] = number;

            var cycles = 2;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDYZ Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDYZ Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDYZ Negative")]
        public void LoadZ(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDYZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = number;

            var cycles = 3;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDYX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDYZX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDYZX Negative")]
        public void LoadZY(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDYZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDYA Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDYA Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDYA Negative")]
        public void LoadAbs(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDYA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDYAX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDYAX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDYAX Negative")]
        public void LoadAbsY(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDYAX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDYAX Positive Cross Boundary")]
        [TestCase(0x0, Flags.Z, TestName = "LDYAX Zero Cross Boundary")]
        [TestCase(0x80, Flags.N, TestName = "LDYAX Negative Cross Boundary")]
        public void LoadAbsYCrossBoundary(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDYAX;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = number;

            var cycles = 5;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.Y.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
    }
}