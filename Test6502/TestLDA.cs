using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestLDA
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0x37, (Flags)0, TestName = "LDAI Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAI Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAI Negative")]
        public void LoadImmediate(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDAI;
            cpu[0x0101] = number;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAZ Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAZ Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAZ Negative")]
        public void LoadZ(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDAZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = number;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAZX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAZX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAZX Negative")]
        public void LoadZX(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.LDAZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = number;

            var cycles =  4;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
            cpu.X.Should().Be(1);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAA Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAA Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAA Negative")]
        public void LoadAbs(byte number, Flags expectedFlags)
        {
            cpu[0x0100] = (byte)OpCode.LDAA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = number;

            var cycles =  4;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAAX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAAX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAAX Negative")]
        public void LoadAbsX(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = number;

            var cycles =  4;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
            cpu.X.Should().Be(1);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAAX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAAX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAAX Negative")]
        public void LoadAbsXCrossBoundary(byte number, Flags expectedFlags)
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = number;

            var cycles =  5;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
            cpu.X.Should().Be(1);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAAY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAAY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAAY Negative")]
        public void LoadAbsY(byte number, Flags expectedFlags)
        {
            cpu.Y = 1;
            cpu.X = 7;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = number;

            var cycles =  4;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
            cpu.Y.Should().Be(1);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAAY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAAY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAAY Negative")]
        public void LoadAbsYCrossBoundary(byte number, Flags expectedFlags)
        {
            cpu.Y = 1;
            cpu.X = 7;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = number;

            var cycles =  5;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0103);
            cpu.Y.Should().Be(1);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAIX Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAIX Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAIX Negative")]
        public void LoadIndirectX(byte number, Flags expectedFlags)
        {
            cpu.X = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIX;
            cpu[0x0101] = 0x02;
            cpu[0x0006] = 0x00;
            cpu[0x0007] = 0x80;
            cpu[0x8000] = number;

            var cycles =  6;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
            cpu.X.Should().Be(4);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAIY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAIY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAIY Negative")]
        public void LoadIndirectY(byte number, Flags expectedFlags)
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0x00;
            cpu[0x0003] = 0x80;
            cpu[0x8004] = number;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
            cpu.Y.Should().Be(4);
        }
        
        [TestCase(0x37, (Flags)0, TestName = "LDAIY Positive")]
        [TestCase(0x0, Flags.Z, TestName = "LDAIY Zero")]
        [TestCase(0x80, Flags.N, TestName = "LDAIY Negative")]
        public void LoadIndirectYCrossBoundary(byte number, Flags expectedFlags)
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0xFF;
            cpu[0x0003] = 0x00;
            cpu[0x0103] = number;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(number);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0102);
            cpu.Y.Should().Be(4);
        }
    }
}