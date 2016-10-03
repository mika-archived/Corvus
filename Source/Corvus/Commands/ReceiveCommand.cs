using System;
using System.Threading.Tasks;

namespace Corvus.Commands
{
    internal abstract class ReceiveCommand : CommandBase
    {
        protected ReceiveCommand(Packet packet) : base(packet) {}

        /// <summary>
        ///     Read command.
        /// </summary>
        /// <returns></returns>
        public abstract Task Read();

        public sealed override Task Invoke()
        {
            throw new NotSupportedException();
        }
    }
}