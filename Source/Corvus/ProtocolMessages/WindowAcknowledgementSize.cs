using System;
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
            AckSize = BitHelper.ToInt32(Reader.Body);
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}