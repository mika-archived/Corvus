using System.Threading.Tasks;

using Corvus.Chunking;
using Corvus.Commands;

namespace Corvus.ProtocolMessages
{
    /// <summary>
    ///     Protocol Control Messages
    /// </summary>
    internal abstract class ProtocolMessage
    {
        protected Packet Packet { get; }
        protected ChunkReader Bytes { get; }

        protected ProtocolMessage(Packet packet, ChunkReader reader)
        {
            Packet = packet;
            Bytes = reader;
        }

        public abstract Task Read();

        public abstract Task Write();

        protected void CheckResponse(ChunkReader reader)
        {
            if (reader.MessageHeader.MessageTypeId != (byte) MessageType.CommandMessage0)
                return;
            var errorCommand = new ErrorCommand();
            errorCommand.CastTo(reader.Body);
            throw new RtmpCommandErrorException(errorCommand, "Previous command returned _error().");
        }
    }
}