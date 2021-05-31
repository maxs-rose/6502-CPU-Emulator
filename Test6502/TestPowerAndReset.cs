using System;
using CPU6502Emulator;
using CPU6502Emulator.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestPowerAndReset
    {
        [Test]
        public void PowerOn()
        {
            var result = CPU.PowerOn();

            result.Should().NotBeNull();
            result.pc.Should().Be(0);
            result.sp.Should().Be(0);
        }

        [Test]
        public void Reset()
        {
            var cpu = CPU.PowerOn();
            cpu.Reset();

            cpu.pc.Should().Be(0x0100);
            cpu.sp.Should().Be(0xFF);
        }

        [Test]
        public void InvalidOpCode()
        {
            var cpu = CPU.PowerOn();
            cpu.Reset();

            cpu[0x0100] = 0xFF;

            var cycles = 10;

            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<InvalidOpCodeException>()
                .WithMessage($"Opcode {0xFF:x8} does not exist in 6502!");
        }
    }
}