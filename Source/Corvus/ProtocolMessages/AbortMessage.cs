using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class AbortMessage : ProtocolMessage
    {
        public int ChunkStreamId { get; set; }

        public AbortMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            ChunkStreamId = BitConverter.ToInt32(Reader.Body.Reverse().ToArray(), 0);
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}