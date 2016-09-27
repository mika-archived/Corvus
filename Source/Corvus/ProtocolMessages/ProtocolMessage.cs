using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    /// <summary>
    ///     Protocol Control Messages
    /// </summary>
    internal abstract class ProtocolMessage
    {
        protected Packet Packet { get; }
        protected ChunkReader Reader { get; }

        protected ProtocolMessage(Packet packet, ChunkReader reader)
        {
            Packet = packet;
            Reader = reader;
        }

        public abstract void Read();

        public abstract Task Write();
    }
}