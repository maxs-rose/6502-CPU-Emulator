﻿using CPU6502Emulator;
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
            
            Assert.NotNull(result);
            Assert.AreEqual(0, result.pc);
            Assert.AreEqual(0, result.sp);
        }

        [Test]
        public void Reset()
        {
            var cpu = CPU.PowerOn();
            cpu.Reset();

            Assert.AreEqual(0x0100,cpu.pc);
            Assert.AreEqual(0x0100,cpu.sp);
        }
    }
}