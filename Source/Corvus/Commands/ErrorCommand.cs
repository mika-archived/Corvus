using System;

namespace Corvus.Commands
{
    internal class ErrorCommand : ICommand
    {
        public string CommandName => "_error";

        public int TransactionId
        {
            get { throw new NotSupportedException(); }
        }
    }
}