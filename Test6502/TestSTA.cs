using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestSTA
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
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAZ;
            cpu[0x0101] = zpAddress;

            var cycles = 3;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[zpAddress]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [TestCase(0x37, 0x04, 0x1, 0x05)]
        [TestCase(0x0, 0x04, 0xFF, 0x04)]
        [TestCase(0x80, 0x04, 0xF, 0x13)]
        public void StoreZPX(byte value, byte zpAddress, byte x, byte finalLocation)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.X = x;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAZX;
            cpu[0x0101] = zpAddress;
            
            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[finalLocation]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0102, cpu.pc);
        }
        
        [TestCase(0x37, (ushort)0x01FF)]
        [TestCase(0x0, (ushort)0x01FF)]
        [TestCase(0x80, (ushort)0x01FF)]
        public void StoreAbs(byte value, ushort address)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAA;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = (byte)(value + 1);

            var cycles = 4;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[address]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [TestCase(0x37, (ushort)0x01FF, 1, (ushort)0x0200)] // boundary crossing
        [TestCase(0x37, (ushort)0x01FD, 1, (ushort)0x01FE)] // no boundary crossing
        [TestCase(0x0, (ushort)0x01FF, 1, (ushort)0x0200)]
        [TestCase(0x0, (ushort)0x01FD, 1, (ushort)0x01FE)]
        [TestCase(0x80, (ushort)0x01FF, 1, (ushort)0x0200)]
        [TestCase(0x80, (ushort)0x01FD, 1, (ushort)0x01FE)]
        public void StoreAbsX(byte value, ushort address, byte x, ushort finalLocation)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.X = x;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAAX;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = (byte)(value + x);

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[finalLocation]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
        
        [TestCase(0x37, (ushort)0x01FF, 1, (ushort)0x0200)] // boundary crossing
        [TestCase(0x37, (ushort)0x01FD, 1, (ushort)0x01FE)] // no boundary crossing
        [TestCase(0x0, (ushort)0x01FF, 1, (ushort)0x0200)]
        [TestCase(0x0, (ushort)0x01FD, 1, (ushort)0x01FE)]
        [TestCase(0x80, (ushort)0x01FF, 1, (ushort)0x0200)]
        [TestCase(0x80, (ushort)0x01FD, 1, (ushort)0x01FE)]
        public void StoreAbsY(byte value, ushort address, byte y, ushort finalLocation)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.Y = y;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAAY;
            cpu[0x0101] = (byte)address;
            cpu[0x0102] = (byte)(address >> 8);
            cpu[address] = (byte)(value + y);

            var cycles = 5;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[finalLocation]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }

        [TestCase(0x37, 0xFF, 1, (ushort)0x0)]
        [TestCase(0x37, 0x0FD, 1, (ushort)0xFE)]
        [TestCase(0x0, 0xFF, 1, (ushort)0x0)]
        [TestCase(0x0, 0x0FD, 1, (ushort)0xFE)]
        [TestCase(0x80, 0xFF, 1, (ushort)0x0)]
        [TestCase(0x80, 0x0FD, 1, (ushort)0xFE)]
        public void StoreIndirectX(byte value, byte zpAddress, byte x, ushort address)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.X = x;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAIX;
            cpu[0x0101] = zpAddress;
            cpu[(ushort) ((zpAddress + x) % 0x100)] = (byte)address;
            cpu[(ushort) ((zpAddress + x + 1) % 0x100)] = (byte)(address >> 8);
            cpu[address] = 0x65;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[address]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }

        [TestCase(0x37, 0x05, 1, (ushort)0x1056, (ushort)0x1057)]
        [TestCase(0x37, 0x0FD, 1, (ushort)0x00FF, (ushort)0x0100)]
        [TestCase(0x0, 0x05, 1, (ushort)0x1056, (ushort)0x1057)]
        [TestCase(0x0, 0x0FD, 1, (ushort)0x00FF, (ushort)0x0100)]
        [TestCase(0x80, 0x05, 1, (ushort)0x1056, (ushort)0x1057)]
        [TestCase(0x80, 0x0FD, 1, (ushort)0x00FF, (ushort)0x0100)]
        public void StoreIndirectY(byte value, byte zpAddress, byte y, ushort address, ushort finalAddress)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.Y = y;
            cpu.A = value;
            cpu[0x0100] = (byte)OpCode.STAIY;
            cpu[0x0101] = zpAddress;
            cpu[zpAddress] = (byte)address;
            cpu[(ushort)(zpAddress + 1)] = (byte)(address >> 8);
            cpu[finalAddress] = 0x65;

            var cycles = 6;
            cpu.RunProgram(ref cycles);
            Assert.AreEqual(0, cycles);
            Assert.AreEqual(value, cpu[finalAddress]);
            Assert.AreEqual(preFlags, cpu.flags);
            Assert.AreEqual(0x0103, cpu.pc);
        }
    }
}