using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestJMP
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [Test]
        public void JumpAbsoluteMode()
        {
            cpu.flags = (Flags)7;
            var preFlags = cpu.flags;
            cpu[0x0100] = (byte)OpCode.JMPI;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x01;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            cycles.Should().Be(0);
            cpu.pc.Should().Be(0x0169);
            cpu.flags.Should().Be(preFlags);
        }

        [Test]
        public void JumpIndirectMode()
        {
            // TODO: Make sure this is actually how JMPIN works :) 
            
            cpu.flags = (Flags)7;
            var preFlags = cpu.flags;
            cpu[0x0100] = (byte)OpCode.JMPIN;
            cpu[0x0101] = 0xFC;
            cpu[0x0102] = 0xBA;
            cpu[0xBAFC] = 0xFF;
            cpu[0xBAFD] = 0x00;

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            cycles.Should().Be(0);
            cpu.pc.Should().Be(0x00FF);
            cpu.flags.Should().Be(preFlags);
        }
    }
}