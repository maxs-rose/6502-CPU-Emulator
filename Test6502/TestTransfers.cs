using CPU6502Emulator;
using FluentAssertions;
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

            cycles.Should().Be(0);
            cpu.X.Should().Be(a);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.Y.Should().Be(a);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.X.Should().Be(sp);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.A.Should().Be(x);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.sp.Should().Be(x);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
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

            cycles.Should().Be(0);
            cpu.A.Should().Be(y);
            cpu.flags.Should().Be(expectedFlags);
            cpu.pc.Should().Be(0x0101);
        }
    }
}