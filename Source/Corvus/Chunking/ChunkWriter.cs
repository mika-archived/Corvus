using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Corvus.Chunking
{
    internal class ChunkWriter
    {
        private readonly List<byte> _bytes;
        private readonly Packet _packet;

        public ChunkStreamId ChunkStreamId { get; set; }

        public MessageType MessageType { get; set; }

        public ChunkWriter(Packet packet)
        {
            _packet = packet;
            _bytes = new List<byte>();
        }

        /// <summary>
        ///     内容を書き込みます。
        /// </summary>
        /// <returns></returns>
        public void Write(byte[] bytes) => _bytes.AddRange(bytes);

        /// <summary>
        ///     内容を書き込みます。
        /// </summary>
        /// <returns></returns>
        public void Write(bool value) => _bytes.AddRange(BitConverter.GetBytes(value));

        /// <summary>
        ///     内容を書き込みます。
        /// </summary>
        /// <returns></returns>
        public void Write(int value, uint bytesWrite = uint.MaxValue, bool isLittleEndian = false)
        {
            var bytes = isLittleEndian ? BitConverter.GetBytes(value) : BitConverter.GetBytes(value).Reverse().ToArray();
            _bytes.AddRange(bytes.Take((int) bytesWrite));
        }

        /// <summary>
        ///     送信します。
        /// </summary>
        /// <returns></returns>
        public async Task Flush()
        {
            var csid = (byte) ChunkStreamId;
            if (ChunkStreamId == ChunkStreamId.Random)
                csid = (byte) new Random().Next(10, byte.MaxValue);
            var chunk = new ChunkHeader(0, csid, (byte) MessageType, 0, _bytes.ToArray());
            await _packet.SendAsync(chunk, _bytes.ToArray());
        }
    }
}