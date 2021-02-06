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
        public void FalingTest()
        {
            Assert.True(false);
        }

        [Test]
        public void LoadImmediatePositive()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAI;
            cpu.memory[0x0101] = 0x37;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateNegative()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAI;
            cpu.memory[0x0101] = 0x80;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }

        [Test]
        public void LoadZPPositive()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAZ;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0069] = 0x37;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPNegative()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAZ;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0069] = 0x80;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPXPositive()
        {
            cpu.X = 1;
            cpu.memory[0x0100] = (byte)OpCode.LDAZX;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x006A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPXNegative()
        {
            cpu.X = 1;
            cpu.memory[0x0100] = (byte)OpCode.LDAZX;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x006A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadAbsPositive()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAA;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x3769] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsNegative()
        {
            cpu.memory[0x0100] = (byte)OpCode.LDAA;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x3769] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXPositive()
        {
            cpu.X = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAX;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x376A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXCrossBoundaryPositive()
        {
            cpu.X = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAX;
            cpu.memory[0x0101] = 0xFF;
            cpu.memory[0x0102] = 0x01;
            cpu.memory[0x0200] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXNegative()
        {
            cpu.X = 1;

            cpu.memory[0x0100] = (byte)OpCode.LDAAX;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsXCrossBoundaryNegative()
        {
            cpu.X = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAX;
            cpu.memory[0x0101] = 0xFF;
            cpu.memory[0x0102] = 0x01;
            cpu.memory[0x0200] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYPositive()
        {
            cpu.Y = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAY;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x376A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryPositive()
        {
            cpu.Y = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAY;
            cpu.memory[0x0101] = 0xFF;
            cpu.memory[0x0102] = 0x01;
            cpu.memory[0x0200] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYNegative()
        {
            cpu.Y = 1;

            cpu.memory[0x0100] = (byte)OpCode.LDAAY;
            cpu.memory[0x0101] = 0x69;
            cpu.memory[0x0102] = 0x37;
            cpu.memory[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryNegative()
        {
            cpu.Y = 1;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAAY;
            cpu.memory[0x0101] = 0xFF;
            cpu.memory[0x0102] = 0x01;
            cpu.memory[0x0200] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectXPositive()
        {
            cpu.X = 4;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAIX;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0006] = 0x00;
            cpu.memory[0x0007] = 0x80;
            cpu.memory[0x8000] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectXNegative()
        {
            cpu.X = 4;

            cpu.memory[0x0100] = (byte)OpCode.LDAIX;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0006] = 0x00;
            cpu.memory[0x0007] = 0x80;
            cpu.memory[0x8000] = 0x80;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYPositive()
        {
            cpu.Y = 4;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAIY;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0002] = 0x00;
            cpu.memory[0x0003] = 0x80;
            cpu.memory[0x8004] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYCrossBoundaryPositive()
        {
            cpu.Y = 4;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAIY;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0002] = 0xFF;
            cpu.memory[0x0003] = 0x00;
            cpu.memory[0x0103] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.A);
            Assert.AreEqual(false, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYNegative()
        {
            cpu.Y = 4;

            cpu.memory[0x0100] = (byte)OpCode.LDAIY;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0002] = 0x00;
            cpu.memory[0x0003] = 0x80;
            cpu.memory[0x8004] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadIndirectYCrossBoundaryNegative()
        {
            cpu.Y = 4;
            
            cpu.memory[0x0100] = (byte)OpCode.LDAIY;
            cpu.memory[0x0101] = 0x02;
            cpu.memory[0x0002] = 0xFF;
            cpu.memory[0x0003] = 0x00;
            cpu.memory[0x0103] = 0x80;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.A);
            Assert.AreEqual(true, cpu.N);
            Assert.AreEqual(0x0102, cpu.pc);
        }
    }
}