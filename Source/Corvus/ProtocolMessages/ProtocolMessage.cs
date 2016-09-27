using System.Threading.Tasks;

namespace Corvus.ProtocolMessages
{
    /// <summary>
    ///     Protocol Control Messages
    /// </summary>
    internal abstract class ProtocolMessage
    {
        protected Packet Packet { get; }

        protected ProtocolMessage(Packet packet)
        {
            Packet = packet;
        }

        public abstract Task Read();

        public abstract Task Write();
    }
}