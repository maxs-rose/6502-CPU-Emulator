using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestORA
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0x0, 0x0, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b0010_0100, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b1010_0100, 0b1100_0100, Flags.N, 0b1110_0100)]
        public void ORAI(byte acc, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.ORAI;
            cpu[0x0101] = mask;

            var cycles = 2;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x0, 0x0, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b0010_0100, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b1010_0100, 0b1100_0100, Flags.N, 0b1110_0100)]
        public void ORAZ(byte acc, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.ORAZ;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = mask;

            var cycles = 3;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x0, 8, 0x0, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 2, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b0010_0100, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b1010_0100, 9, 0b1100_0100, Flags.N, 0b1110_0100)]
        public void ORAZX(byte acc, byte x, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.ORAZX;
            cpu[0x0101] = 0x02;
            cpu[(byte)(0x02 + x)] = mask;

            var cycles = 3;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x0, (ushort)0xFF01, 0x0, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, (ushort)0xFE09, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0b1100_0100, Flags.N, 0b1110_0100)]
        public void ORAA(byte acc, ushort address, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.ORAA;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = mask;

            var cycles = 4;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x0, (ushort)0xFF01, 0xF0, 0x0, Flags.Z, 0x0, 4)]
        [TestCase(0b0110_0100, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0100, 5)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0100, 4)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0x01, 0b1100_0100, Flags.N, 0b1110_0100, 4)]
        public void ORAAX(byte acc, ushort address, byte x, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.ORAAX;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[(ushort) ((address + x) % 0x10000)] = mask;

            var cycles = c;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x0, (ushort)0xFF01, 0xF0, 0x0, Flags.Z, 0x0, 4)]
        [TestCase(0b0110_0100, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0100, 5)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0100, 4)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0x01, 0b1100_0100, Flags.N, 0b1110_0100, 4)]
        public void ORAAY(byte acc, ushort address, byte y, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.Y = y;

            cpu[0x0100] = (byte) OpCode.ORAAY;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[(ushort) ((address + y) % 0x10000)] = mask;

            var cycles = c;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0103);
        }
        
        [TestCase(0x0, 3, (ushort)0xFF01, 0xF0, 0x0, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 5, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b0010_0100, 8, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0100)]
        [TestCase(0b1010_0100, 10, (ushort)0xFF01, 0x01, 0b1100_0100, Flags.N, 0b1110_0100)]
        public void ORAIX(byte acc, byte zpa, ushort address, byte x, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.ORAIX;
            cpu[0x0101] = zpa;
            cpu[(byte)(zpa + x)] = (byte)address;
            cpu[(byte)(zpa + x + 1)] = (byte)(address >> 8);
            cpu[address] = mask;

            var cycles = 6;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0x0, 10, (ushort)0xFF01, 0xF0, 0x0, Flags.Z, 0x0, 5)]
        [TestCase(0b0110_0100, 1, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0100, 6)]
        [TestCase(0b0010_0100, 5, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0100, 5)]
        [TestCase(0b1010_0100, 200, (ushort)0xFF01, 0x01, 0b1100_0100, Flags.N, 0b1110_0100, 5)]
        public void ORAIY(byte acc, byte zpa, ushort address, byte y, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.Y = y;

            cpu[0x0100] = (byte) OpCode.ORAIY;
            cpu[0x0101] = zpa;
            cpu[zpa] = (byte)address;
            cpu[(byte) (zpa + 1)] = (byte)(address >> 8);
            cpu[(ushort) ((address + y) % 0x1_00_00)] = mask;

            var cycles = c;
            cpu.RunProgram(ref cycles);

            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
    }
}