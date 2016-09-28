using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class Acknowledgement : ProtocolMessage
    {
        public int SequenceNumber { get; set; }

        public Acknowledgement(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            SequenceNumber = BitConverter.ToInt32(Reader.Body.Reverse().ToArray(), 0);
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}