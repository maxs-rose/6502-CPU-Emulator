using CPU6502Emulator;
using FluentAssertions;
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

        [TestCase(0x37, (Flags)0, TestName = "LDXI Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDXI Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDXI Negative")]
        public void LoadImmediate(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = number;

            var cycles = 2;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDXZ Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDXZ Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDXZ Negative")]
        public void LoadZ(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDXZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = number;

            var cycles = 3;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDXZY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDXZY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDXZY Negative")]
        public void LoadZY(byte number, Flags expectedFlags)
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXZY;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDXA Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDXA Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDXA Negative")]
        public void LoadAbs(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDXA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDXAY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDXAY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDXAY Negative")]
        public void LoadAbsY(byte number, Flags expectedFlags)
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = number;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDXAY Positive Cross Boundary")]
        [TestCase(0x0, Flags.Z, TestName = "LDXAY Zero Cross Boundary")]
        [TestCase(0x80, Flags.N, TestName = "LDXAY Negative Cross Boundary")]
        public void LoadAbsYCrossBoundary(byte number, Flags expectedFlags)
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = number;

            var cycles = 5;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.X.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
    }
}