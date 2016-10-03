using System;

namespace Corvus
{
    internal class RtmpInvalidFormatException : Exception
    {
        public RtmpInvalidFormatException() {}

        public RtmpInvalidFormatException(string message) : base(message) {}

        public RtmpInvalidFormatException(string message, Exception innerException) : base(message, innerException) {}
    }
}