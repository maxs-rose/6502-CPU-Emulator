using CPU6502Emulator;
using NUnit.Framework;

namespace Test6502
{
    [TestFixture]
    public class TestSTX
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
            cpu.X = value;
            cpu[0x0100] = (byte)OpCode.STXZ;
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
        public void StoreZPY(byte value, byte zpAddress, byte y, byte finalLocation)
        {
            var preFlags = (Flags) 7;
            cpu.flags = preFlags;
            cpu.Y = y;
            cpu.X = value;
            cpu[0x0100] = (byte)OpCode.STXZY;
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
            cpu.X = value;
            cpu[0x0100] = (byte)OpCode.STXA;
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
    }
}