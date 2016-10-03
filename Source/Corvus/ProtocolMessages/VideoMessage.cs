using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class VideoMessage : ProtocolMessage
    {
        public byte Type { get; set; }

        public byte Format { get; set; }

        public ReadOnlyCollection<byte> Data { get; set; }

        public VideoMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            // First byte.
            // [4 bits] ... Type
            // [4 bits] ... Format
            var firstByte = Reader.Body[0];
            Type = (byte) (firstByte & 0xF0);
            Format = (byte) (firstByte & 0x0F);
            Data = Reader.Body.Skip(1).ToList().AsReadOnly();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}