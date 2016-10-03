using System;

using Corvus.Commands;

namespace Corvus
{
    internal class RtmpCommandErrorException : Exception
    {
        public ErrorCommand ErrorCommand { get; }

        public RtmpCommandErrorException(ErrorCommand errorCommand)
        {
            ErrorCommand = errorCommand;
        }

        public RtmpCommandErrorException(ErrorCommand errorCommand, string message) : base(message)
        {
            ErrorCommand = errorCommand;
        }

        public RtmpCommandErrorException(ErrorCommand errorCommand, string message, Exception innerException) : base(message, innerException)
        {
            ErrorCommand = errorCommand;
        }
    }
}