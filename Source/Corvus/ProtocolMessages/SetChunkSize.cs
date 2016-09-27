using System;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class SetChunkSize : ProtocolMessage
    {
        public SetChunkSize(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override Task Read()
        {
            throw new NotImplementedException();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}