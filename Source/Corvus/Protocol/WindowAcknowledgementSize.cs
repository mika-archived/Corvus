using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.Protocol
{
    /// <summary>
    ///     Window Acknowledgement Size
    /// </summary>
    internal class WindowAcknowledgementSize : Protocol
    {
        public int AckSize { get; private set; }

        public override async Task Read(Packet packet)
        {
            // Header 12 bytes + Body 4 bytes
            var reader = new ChunkReader(packet);
            await reader.Read();
            AckSize = BitHelper.ToInt32(reader.Body);
        }
    }
}