using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestINC
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void IncrementZPPositive()
        {
            cpu[0x0100] = (byte)OpCode.INCZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x1;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x2, cpu[0x0069]);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }

        [Test]
        public void IncrementZPNegative()
        {
            cpu[0x0100] = (byte)OpCode.INCZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0x7F;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu[0x0069]);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }

        [Test]
        public void IncrementZPZero()
        {
            cpu[0x0100] = (byte)OpCode.INCZ;
            cpu[0x0101] = 0x69;
            cpu[0x0069] = 0xFF;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu[0x0069]);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void IncrementZPXPositive()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.INCZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x38, cpu[0x006A]);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void IncrementZPXNegative()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.INCZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0x7F;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu[0x006A]);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void IncrementZPXZero()
        {
            cpu.X = 1;
            cpu[0x0100] = (byte)OpCode.INCZX;
            cpu[0x0101] = 0x69;
            cpu[0x006A] = 0xFF;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x0, cpu[0x006A]);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsPositive()
        {
            cpu[0x0100] = (byte)OpCode.INCA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x37;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x38, cpu[0x3769]);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsNegative()
        {
            cpu[0x0100] = (byte)OpCode.INCA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0x7F;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu[0x3769]);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsZero()
        {
            cpu[0x0100] = (byte)OpCode.INCA;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x3769] = 0xFF;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu[0x3769]);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsXPositive()
        {
            cpu.X = 1;
            
            cpu[0x0100] = (byte)OpCode.INCX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x37;

            var cycles = 7;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x38, cpu[0x376A]);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsXNegative()
        {
            cpu.X = 1;

            cpu[0x0100] = (byte)OpCode.INCX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0x7F;

            var cycles = 7;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x80, cpu[0x376A]);
            Assert.AreEqual(Flags.N, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [Test]
        public void IncrementAbsXZero()
        {
            cpu.X = 1;

            cpu[0x0100] = (byte)OpCode.INCX;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x37;
            cpu[0x376A] = 0xFF;

            var cycles = 7;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(0x00, cpu[0x376A]);
            Assert.AreEqual(Flags.Z, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
    }
}