using System;

namespace NetKeystore.Exceptions
{
    public class KdfNotSupportedException : Exception
    {
        public KdfNotSupportedException() : base() { }
        public KdfNotSupportedException(string message) : base(message) { }
    }
}