using System;

namespace CPU6502Emulator.Exceptions
{
    public class OpCodeNotImplementedException : Exception
    {
        public OpCodeNotImplementedException() : base() { }
        public OpCodeNotImplementedException(string message) : base(message) { }
        public OpCodeNotImplementedException(string message, Exception inner) : base(message, inner) { }
    }
}