using System;

namespace CPU6502Emulator.Exceptions
{
    public class NotEnoughCyclesException : Exception
    {
        public NotEnoughCyclesException(string message) : base(message) { }
    }
}