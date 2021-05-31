using System;
using CPU6502Emulator;
using CPU6502Emulator.Exceptions;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class CompleteCoverage
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }
        
        [Test]
        public void TestNotEnoughCycles()
        {
            cpu[0x0100] = (byte)OpCode.JMPI;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x01;

            var cycles = 2;
            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<NotEnoughCyclesException>()
                .WithMessage("Not enough cycles!");
        }

        [Test]
        public void StackOverflowInGetShort()
        {
            cpu.sp = 0;
            cpu[0x0100] = (byte)OpCode.JSR;
            cpu[0x0101] = 0x69;
            cpu[0x0102] = 0x01;

            var cycles = 6;
            
            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<StackOverflowException>();
        }

        [Test]
        public void PopByteSPUnderflow()
        {
            cpu.sp = byte.MaxValue;
            cpu[0x0100] = (byte)OpCode.PLA;

            var cycles = 4;
            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<StackUnderflowException>();
        }

        [Test]
        public void PushByteToSPOverflow()
        {
            cpu.sp = 0;
            cpu[0x0100] = (byte)OpCode.PHA;

            var cycles = 3;
            
            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<StackOverflowException>();
        }

        [Test]
        public void ReadShortFromSPUnderflow()
        {
            cpu.sp = byte.MaxValue;
            cpu[0x0100] = (byte)OpCode.RTS;

            var cycles = 6;
            
            cpu.Invoking(c => c.RunProgram(ref cycles))
                .Should().Throw<StackUnderflowException>();
        }

        [TestCase(Flags.C, true)]
        [TestCase(~Flags.C, false)]
        public void CFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.C.Should().Be(expected);
        }
        
        [TestCase(Flags.Z, true)]
        [TestCase(~Flags.Z, false)]
        public void ZFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.Z.Should().Be(expected);
        }
        
        [TestCase(Flags.I, true)]
        [TestCase(~Flags.I, false)]
        public void IFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.I.Should().Be(expected);
        }
        
        [TestCase(Flags.D, true)]
        [TestCase(~Flags.D, false)]
        public void DFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.D.Should().Be(expected);
        }
        
        [TestCase(Flags.B, true)]
        [TestCase(~Flags.B, false)]
        public void BFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.B.Should().Be(expected);
        }
        
        [TestCase(Flags.V, true)]
        [TestCase(~Flags.V, false)]
        public void VFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.V.Should().Be(expected);
        }
        
        [TestCase(Flags.N, true)]
        [TestCase(~Flags.N, false)]
        public void NFlagGetter(Flags flags, bool expected)
        {
            cpu.flags = flags;
            cpu.N.Should().Be(expected);
        }
    }
}