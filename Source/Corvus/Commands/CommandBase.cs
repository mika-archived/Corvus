using System.Threading.Tasks;

namespace Corvus.Commands
{
    internal abstract class CommandBase
    {
        protected Packet Packet { get; }

        // XML document command based RTMP secification "7.2.1.1 connect".
        /// <summary>
        ///     Name of the command.
        /// </summary>
        public abstract string CommandName { get; }

        /// <summary>
        ///     Transcation ID.
        /// </summary>
        public abstract int TransactionId { get; }

        public CommandBase(Packet packet)
        {
            Packet = packet;
        }

        /// <summary>
        ///     Invoke command.
        /// </summary>
        /// <returns></returns>
        public abstract Task Invoke();
    }
}