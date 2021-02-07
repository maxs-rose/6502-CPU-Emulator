using System;

namespace CPU6502Emulator.Exceptions
{
    public class StackUnderflowException : Exception
    {
        public StackUnderflowException() {}
        public StackUnderflowException(string message) : base(message) { }
        public StackUnderflowException(string message, Exception inner) : base(message, inner) { }
    }
}