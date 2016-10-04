using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    /// <summary>
    ///     Window Acknowledgement Size
    /// </summary>
    internal class WindowAcknowledgementSize : ProtocolMessage
    {
        public int AckSize { get; private set; }

        public WindowAcknowledgementSize(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            AckSize = BitConverter.ToInt32(Reader.Body.Reverse().ToArray(), 0); // 4 bytes, BE
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}