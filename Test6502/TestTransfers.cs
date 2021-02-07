using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestTransfers
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }
        
        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void TAX(byte a, Flags expectedFlags)
        {
            cpu.A = a;
            cpu.X = 20;
            cpu[0x0100] = (byte) OpCode.TAX;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(a, cpu.X);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
        
        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void TAY(byte a, Flags expectedFlags)
        {
            cpu.A = a;
            cpu.Y = 20;
            cpu[0x0100] = (byte) OpCode.TAY;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(a, cpu.Y);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
        
        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void TSX(byte sp, Flags expectedFlags)
        {
            cpu.X = 20;
            cpu.sp = sp;
            cpu[0x0100] = (byte) OpCode.TSX;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(sp, cpu.X);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
        
        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void TXA(byte x, Flags expectedFlags)
        {
            cpu.X = x;
            cpu.A = 20;
            cpu[0x0100] = (byte) OpCode.TXA;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(x, cpu.A);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
        
        [TestCase(0x37, (Flags)7)]
        [TestCase(0x0, (Flags)7)]
        [TestCase(0x80, (Flags)7)]
        public void TXS(byte x, Flags expectedFlags)
        {
            cpu.flags = expectedFlags;
            cpu.X = x;
            cpu.sp = 20;
            cpu[0x0100] = (byte) OpCode.TXS;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(x, cpu.sp);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
        
        [TestCase(0x37, (Flags)0)]
        [TestCase(0x0, Flags.Z)]
        [TestCase(0x80, Flags.N)]
        public void TYA(byte y, Flags expectedFlags)
        {
            cpu.Y = y;
            cpu.A = 20;
            cpu[0x0100] = (byte) OpCode.TYA;
            
            var cycles = 2;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(y, cpu.A);
            Assert.AreEqual(expectedFlags, cpu.flags);
            Assert.AreEqual(0x0101, cpu.pc);
        }
    }
}