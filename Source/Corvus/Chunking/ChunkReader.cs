using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Corvus.Chunking
{
    internal class ChunkReader
    {
        private readonly Packet _packet;

        public ChunkBasicHeader BasicHeader { get; private set; }
        public ChunkMessageHeader MessageHeader { get; private set; }
        public ReadOnlyCollection<byte> Body { get; private set; }

        public ChunkReader(Packet packet)
        {
            _packet = packet;
        }

        /// <summary>
        ///     1続きのチャンクを読み込みます
        /// </summary>
        /// <returns></returns>
        public async Task Read()
        {
            // Basic Header
            var firstByte = await _packet.ReceiveAsync(1);
            var fmt = (byte) (firstByte[0] >> 6);
            var csid = firstByte[0] & 0x3F;
            if (csid == 0)
                csid = (await _packet.ReceiveAsync(1))[0] + 64;
            else if (csid == 1)
                csid = BitConverter.ToUInt16(await _packet.ReceiveAsync(2), 0) + 64;
            BasicHeader = new ChunkBasicHeader(fmt, (ushort) csid);

            if (fmt != 0)
                throw new NotSupportedException();

            // Read length
            await _packet.ReceiveAsync(3); // timestamp, ignore
            var length = BitHelper.ToInt32(await _packet.ReceiveAsync(3));
            var msgTypeId = await _packet.ReceiveAsync(1);
            var msgStreamId = BitHelper.ToInt32(await _packet.ReceiveAsync(4));

            var bytesRead = new List<byte>();
            var size = length > _packet.MaxChunkSize ? (int) _packet.MaxChunkSize : length;

            bytesRead.AddRange(await _packet.ReceiveAsync(size));
            while (bytesRead.Count < length)
            {
                await _packet.ReceiveAsync(1);
                size = length - bytesRead.Count > _packet.MaxChunkSize ? (int) _packet.MaxChunkSize : length - bytesRead.Count;
                bytesRead.AddRange(await _packet.ReceiveAsync(size));
            }
            MessageHeader = new ChunkMessageHeader(fmt, (uint) length, msgTypeId[0], msgStreamId);
            Body = bytesRead.AsReadOnly();
        }
    }
}