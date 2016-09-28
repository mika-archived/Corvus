using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class AudioMessage : ProtocolMessage
    {
        public byte Format { get; set; }

        public byte SampleRate { get; set; }

        public byte SampleSize { get; set; }

        public byte Channel { get; set; }

        public ReadOnlyCollection<byte> Data { get; set; }

        public AudioMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            // First byte.
            // [4 bits] -- Format (1010 .. HE-AAC)
            // [2 bits] -- Sample rate
            // [1 bit] --- Sample size
            // [1 bit] --- Channel (0 .. Mono, 1 .. Stereo)
            var firstByte = Reader.Body[0];
            Format = (byte) (firstByte & 0xF0);
            SampleRate = (byte) (firstByte & 0x0B);
            SampleSize = (byte) (firstByte & 0x02);
            Channel = (byte) (firstByte & 0x01);
            Data = Reader.Body.Skip(1).ToList().AsReadOnly();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}