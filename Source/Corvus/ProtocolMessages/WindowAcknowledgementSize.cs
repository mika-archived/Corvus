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

        public WindowAcknowledgementSize(Packet packet) : base(packet) {}

        public override async Task Read()
        {
            // Header 12 bytes + Body 4 bytes
            var reader = new ChunkReader(Packet);
            await reader.Read();
            AckSize = BitHelper.ToInt32(reader.Body);
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}