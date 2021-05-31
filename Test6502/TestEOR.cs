using CPU6502Emulator;
using FluentAssertions;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestEOR
    {
        private CPU cpu;
        
        [SetUp]
        public void SetUp()
        {
            cpu = CPU.PowerOn();
            cpu.Reset();
        }

        [TestCase(0xFF, 0xFF, Flags.Z, 0x00)]
        [TestCase(0b0110_0100, 0b0100_0100, (Flags)0, 0b0010_0000)]
        [TestCase(0b0010_0100, 0b0100_0100, (Flags)0, 0b0110_0000)]
        [TestCase(0b1010_0100, 0b0100_0100, Flags.N, 0b1110_0000)]
        public void EORI(byte acc, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.EORI;
            cpu[0x0101] = mask;

            var cycles = 2;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0xFF, 0xFF, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 0b0100_0100, (Flags)0, 0b0010_0000)]
        [TestCase(0b0010_0100, 0b0100_0100, (Flags)0, 0b0110_0000)]
        [TestCase(0b1010_0100, 0b0100_0101, Flags.N, 0b1110_0001)]
        public void EORZ(byte acc, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.EORZ;
            cpu[0x0101] = 0x02;
            cpu[0x0002] = mask;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0xFF, 8, 0xFF, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 2, 0b0100_0100, (Flags)0, 0b0010_0000)]
        [TestCase(0b0010_0100, 0xFF, 0b0100_0100, (Flags)0, 0b0110_0000)]
        [TestCase(0b1010_0100, 9, 0b0100_0101, Flags.N, 0b1110_0001)]
        public void EORZX(byte acc, byte x, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.EORZX;
            cpu[0x0101] = 0x02;
            cpu[(byte)(0x02 + x)] = mask;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            
            cycles.Should().Be(0);
            cpu.A.Should().Be(result);
            cpu.flags.Should().Be(flags);
            cpu.pc.Should().Be(0x0102);
        }
        
        [TestCase(0xFF, (ushort)0xFF01, 0xFF, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, (ushort)0xFE09, 0b0100_0100, (Flags)0, 0b0010_0000)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0b0100_0100, (Flags)0, 0b0110_0000)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0b0100_0101, Flags.N, 0b1110_0001)]
        public void EORA(byte acc, ushort address, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;

            cpu[0x0100] = (byte) OpCode.EORA;
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
        
        [TestCase(0xFF, (ushort)0xFF01, 0xF0, 0xFF, Flags.Z, 0x0, 4)]
        [TestCase(0b0110_0100, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0010_0000, 5)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0000, 4)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0x01, 0b0100_0101, Flags.N, 0b1110_0001, 4)]
        public void EORAX(byte acc, ushort address, byte x, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.EORAX;
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
        
        [TestCase(0xFF, (ushort)0xFF01, 0xF0, 0xFF, Flags.Z, 0x0, 4)]
        [TestCase(0b0110_0100, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0010_0000, 5)]
        [TestCase(0b0010_0100, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0000, 4)]
        [TestCase(0b1010_0100, (ushort)0xFF01, 0x01, 0b0100_0101, Flags.N, 0b1110_0001, 4)]
        public void EORAY(byte acc, ushort address, byte y, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.Y = y;

            cpu[0x0100] = (byte) OpCode.EORAY;
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
        
        [TestCase(0xFF, 3, (ushort)0xFF01, 0xF0, 0xFF, Flags.Z, 0x0)]
        [TestCase(0b0110_0100, 5, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0010_0000)]
        [TestCase(0b0010_0100, 8, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0000)]
        [TestCase(0b1010_0100, 10, (ushort)0xFF01, 0x01, 0b0100_0101, Flags.N, 0b1110_0001)]
        public void EORIX(byte acc, byte zpa, ushort address, byte x, byte mask, Flags flags, byte result)
        {
            cpu.A = acc;
            cpu.X = x;

            cpu[0x0100] = (byte) OpCode.EORIX;
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
        
        [TestCase(0xFF, 10, (ushort)0xFF01, 0xF0, 0xFF, Flags.Z, 0x0, 5)]
        [TestCase(0b0110_0100, 1, (ushort)0xFE01, 0xFF, 0b0100_0100, (Flags)0, 0b0010_0000, 6)]
        [TestCase(0b0010_0100, 5, (ushort)0x0D01, 0x99, 0b0100_0100, (Flags)0, 0b0110_0000, 5)]
        [TestCase(0b1010_0100, 200, (ushort)0xFF01, 0x01, 0b0100_0101, Flags.N, 0b1110_0001, 5)]
        public void EORIY(byte acc, byte zpa, ushort address, byte y, byte mask, Flags flags, byte result, int c)
        {
            cpu.A = acc;
            cpu.Y = y;

            cpu[0x0100] = (byte) OpCode.EORIY;
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