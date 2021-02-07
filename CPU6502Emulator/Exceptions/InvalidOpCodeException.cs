using System;

namespace CPU6502Emulator.Exceptions
{
    public class InvalidOpCodeException : Exception
    {
        public InvalidOpCodeException() : base() { }
        public InvalidOpCodeException(string message) : base(message) { }
        public InvalidOpCodeException(string message, Exception inner) : base(message, inner) { }
    }
}