using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestSTY
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0x37, 0x04)]
        [TestCase(0x0, 0xF0)]
        [TestCase(0x80, 0x0F)]
        public void StoreZP(byte value, byte zpAddress)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.Y = value;
            cpu[0x0100] = (byte)OpCode.STYZ;
            cpu[0x0101] = zpAddress;

            var cycles = 3;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu[zpAddress].Should().Be(value);
            cpu.flags.Should().Be(preFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, 0x04, 0x1, 0x05)]
        [TestCase(0x0, 0x04, 0xFF, 0x04)]
        [TestCase(0x80, 0x04, 0xF, 0x13)]
        public void StoreZPY(byte value, byte zpAddress, byte x, byte finalLocation)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.X = x;
            cpu.Y = value;
            cpu[0x0100] = (byte)OpCode.STYZX;
            cpu[0x0101] = zpAddress;
            
            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu[finalLocation].Should().Be(value);
            cpu.flags.Should().Be(preFlags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x37, (ushort)0x01FF)]
        [TestCase(0x0, (ushort)0x01FF)]
        [TestCase(0x80, (ushort)0x01FF)]
        public void StoreAbs(byte value, ushort address)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.Y = value;
            cpu[0x0100] = (byte)OpCode.STYA;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = (byte)(value + 1);

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu[address].Should().Be(value);
            cpu.flags.Should().Be(preFlags);
            cpu.pc.Should().Be(0x0103);
        }
    }
}