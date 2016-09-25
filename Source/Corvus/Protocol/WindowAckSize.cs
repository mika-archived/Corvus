using System;
using System.Threading.Tasks;

namespace Corvus.Protocol
{
    /// <summary>
    ///     Window Acknowledgement Size
    /// </summary>
    internal class WindowAckSize : Protocol
    {
        public int AckSize { get; private set; }

        public override async Task Read(Packet packet)
        {
            // Header 12 bytes + Body 4 bytes
            var bytes = await packet.ReceiveAsync(16);
            AckSize = BitConverter.ToInt32(bytes, 12);
        }
    }
}