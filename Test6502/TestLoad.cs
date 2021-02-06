using System.Xml.Schema;
using CPU6502Emulator;
using NUnit.Framework;

namespace Test_6504
{
    [TestFixture]
    public class TestLoad
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
            cpu[0x0100] = (byte)OpCode.LDAI;
            cpu[0x0101] = 0x37;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateZero()
        {
            cpu[0x0100] = (byte)OpCode.LDAI;
            cpu[0x0101] = 0x0;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDAI;
            cpu[0x0101] = 0x80;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }

        [Test]
        public void LoadZPPositive()
        {
            cpu[0x0100] = (byte)OpCode.LDAZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x37;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDAZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x80;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPZero()
        {
            cpu[0x0100] = (byte)OpCode.LDAZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x0;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPXPositive()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.LDAZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPXNegative()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.LDAZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPXZero()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.LDAZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x0;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadAbsPositive()
        {
            cpu[0x0100] = (byte)OpCode.LDAA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDAA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsZero()
        {
            cpu[0x0100] = (byte)OpCode.LDAA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x00;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXPositive()
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXCrossBoundaryPositive()
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXNegative()
        {
            cpu.X = 1;

            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXCrossBoundaryNegative()
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXZero()
        {
            cpu.X = 1;

            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXCrossBoundaryZero()
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAX;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x00;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYPositive()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryPositive()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYNegative()
        {
            cpu.Y = 1;

            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryNegative()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYZero()
        {
            cpu.Y = 1;

            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x00;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryZero()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDAAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x00;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectXPositive()
        {
            cpu.X = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIX;
            cpu[0x0101] = 0x02;
            cpu[0x0006] = 0x00;
            cpu[0x0007] = 0x80;
            cpu[0x8000] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectXNegative()
        {
            cpu.X = 4;

            cpu[0x0100] = (byte)OpCode.LDAIX;
            cpu[0x0101] = 0x02;
            cpu[0x0006] = 0x00;
            cpu[0x0007] = 0x80;
            cpu[0x8000] = 0x80;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectXZero()
        {
            cpu.X = 4;

            cpu[0x0100] = (byte)OpCode.LDAIX;
            cpu[0x0101] = 0x02;
            cpu[0x0006] = 0x00;
            cpu[0x0007] = 0x80;
            cpu[0x8000] = 0x00;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYPositive()
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0x00;
            cpu[0x0003] = 0x80;
            cpu[0x8004] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYCrossBoundaryPositive()
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0xFF;
            cpu[0x0003] = 0x00;
            cpu[0x0103] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYNegative()
        {
            cpu.Y = 4;

            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0x00;
            cpu[0x0003] = 0x80;
            cpu[0x8004] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYCrossBoundaryNegative()
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0xFF;
            cpu[0x0003] = 0x00;
            cpu[0x0103] = 0x80;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYZero()
        {
            cpu.Y = 4;

            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0x00;
            cpu[0x0003] = 0x80;
            cpu[0x8004] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYCrossBoundaryZero()
        {
            cpu.Y = 4;
            
            cpu[0x0100] = (byte)OpCode.LDAIY;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = 0xFF;
            cpu[0x0003] = 0x00;
            cpu[0x0103] = 0x00;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.A);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
    }
}