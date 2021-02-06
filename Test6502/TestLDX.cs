using CPU6502Emulator;
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

        [Test]
        public void LoadImmediatePositive()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x37;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateZero()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x0;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadImmediateNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDXI;
            cpu[0x0101] = 0x80;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }

        [Test]
        public void LoadZPPositive()
        {
            cpu[0x0100] = (byte)OpCode.LDXZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x37;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDXZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x80;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPZero()
        {
            cpu[0x0100] = (byte)OpCode.LDXZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x0;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPYPositive()
        {
            cpu.Y = 1;
            cpu[0x0100] = (byte)OpCode.LDXZY;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPYNegative()
        {
            cpu.Y = 1;
            cpu[0x0100] = (byte)OpCode.LDXZY;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadZPYZero()
        {
            cpu.Y = 1;
            cpu[0x0100] = (byte)OpCode.LDXZY;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x0;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void LoadAbsPositive()
        {
            cpu[0x0100] = (byte)OpCode.LDXA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsNegative()
        {
            cpu[0x0100] = (byte)OpCode.LDXA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsZero()
        {
            cpu[0x0100] = (byte)OpCode.LDXA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x00;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYPositive()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x37;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryPositive()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x37;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x37, cpu.X);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYNegative()
        {
            cpu.Y = 1;

            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x80;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryNegative()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x80;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu.X);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYZero()
        {
            cpu.Y = 1;

            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x00;

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void LoadAbsYCrossBoundaryZero()
        {
            cpu.Y = 1;
            
            cpu[0x0100] = (byte)OpCode.LDXAY;
            cpu[0x0101] = 0xFF;
            cpu[0x0102] = 0x01;
            cpu[0x0200] = 0x00;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu.X);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
    }
}