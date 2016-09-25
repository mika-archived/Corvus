using System;

namespace Corvus.Commands
{
    internal class ResultCommand : ICommand
    {
        public string CommandName => "_result";

        public int TransactionId
        {
            get { throw new NotSupportedException(); }
        }
    }
}