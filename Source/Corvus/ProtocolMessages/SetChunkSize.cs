using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class SetChunkSize : ProtocolMessage
    {
        public int ChunkSize { get; set; }

        public SetChunkSize(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            ChunkSize = BitConverter.ToInt32(Reader.Body.Reverse().ToArray(), 0);
            Packet.MaxChunkSize = (uint) ChunkSize;
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}