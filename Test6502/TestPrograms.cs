using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestPrograms
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void TestProgram1()
        {
            // Program:
            //  Setup: A = 2, X = 4, Y = 3
            //  Operations:
            //      Increment @ 0x6937 (cpu[0x6937] = 0xF2, flags = N)
            //      Jump to subroutine @ 0x960F
            //      Increment X register (X = 5, flags = 0)
            //      Store the A register @ 0x02 (cpu[0x02] = 2, flags = 0)
            //      Store the Y register @ 0x6938 (cpu[0x6938] = 3, flags = 0)
            //      Return from subroutine (pc = 0x0106)
            // Final Expected State:
            //      A = 2, X = 5, Y = 3
            //      cpu[0x02] = 0x02, cpu[0x6938] = 0x03, cpu[0x6937] = 0xF2
            //      pc = 0x0106, flags = 0, sp = 0xFF

            cpu.A = 2;
            cpu.X = 4;
            cpu.Y = 3;

            cpu[0x6937] = 0xF1;
            cpu[0x0100] = (byte)OpCode.INCA;
            cpu[0x0101] = 0x37;
            cpu[0x0102] = 0x69;
            cpu[0x0103] = (byte)OpCode.JSR;
            cpu[0x0104] = 0x0F;
            cpu[0x0105] = 0x96; // Jump to subroutine at 0x960F
            cpu[0x960F] = (byte)OpCode.INX;
            cpu[0x9610] = (byte)OpCode.STAZ;
            cpu[0x9611] = 0x02; // Store 3 in loc 0x02
            cpu[0x9612] = (byte)OpCode.STYA;
            cpu[0x9613] = 0x38;
            cpu[0x9614] = 0x69;
            cpu[0x9615] = (byte) OpCode.RTS;

            var cycles = 6 + 6 + 2 + 3 + 4 + 6;
            
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual((Flags)0, cpu.flags);
            Assert.AreEqual(0xFF, cpu.sp);
            Assert.AreEqual(0x0106, cpu.pc);
            Assert.AreEqual(2, cpu.A);
            Assert.AreEqual(5, cpu.X);
            Assert.AreEqual(3, cpu.Y);
            Assert.AreEqual(0x02, cpu[0x02]);
            Assert.AreEqual(0x03, cpu[0x6938]);
            Assert.AreEqual(0xF2, cpu[0x6937]);
        }
    }
}