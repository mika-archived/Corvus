using System;
using System.Threading.Tasks;

namespace Corvus.Commands
{
    internal class ResultCommand : ReceiveCommand
    {
        public override string CommandName => "_result";

        public override int TransactionId
        {
            get { throw new NotSupportedException(); }
        }

        public ResultCommand(Packet packet) : base(packet) {}

        public override Task Read()
        {
            throw new NotImplementedException();
        }
    }
}