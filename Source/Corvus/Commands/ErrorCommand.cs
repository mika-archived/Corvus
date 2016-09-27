using System;
using System.Threading.Tasks;

namespace Corvus.Commands
{
    internal class ErrorCommand : ReceiveCommand
    {
        public override string CommandName => "_error";

        public override int TransactionId
        {
            get { throw new NotSupportedException(); }
        }

        public ErrorCommand(Packet packet) : base(packet) {}

        public override Task Read()
        {
            throw new NotImplementedException();
        }
    }
}