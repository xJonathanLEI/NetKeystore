using System;

namespace NetKeystore.Exceptions
{
    public class CipherNotSupportedException : Exception
    {
        public CipherNotSupportedException() : base() { }
        public CipherNotSupportedException(string message) : base(message) { }
    }
}