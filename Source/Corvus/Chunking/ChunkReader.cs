using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Corvus.Chunking
{
    public class ChunkReader
    {
        private static ChunkMessageHeader _lastHeader;
        private readonly Packet _packet;
        private bool _isRead;

        public ChunkBasicHeader BasicHeader { get; private set; }
        public ChunkMessageHeader MessageHeader { get; private set; }
        public ReadOnlyCollection<byte> Body { get; private set; }

        public ChunkReader(Packet packet)
        {
            _packet = packet;
            _isRead = false;
        }

        /// <summary>
        ///     1続きのチャンクを読み込みます
        /// </summary>
        /// <returns></returns>
        public async Task Read()
        {
            if (_isRead)
                return;
            _isRead = true;
            // Basic Header
            var firstByte = await _packet.ReceiveAsync(1);
            var fmt = (byte) (firstByte[0] >> 6);
            var csid = firstByte[0] & 0x3F;
            if (csid == 0)
                csid = (await _packet.ReceiveAsync(1))[0] + 64;
            else if (csid == 1)
                csid = BitConverter.ToUInt16(await _packet.ReceiveAsync(2), 0) + 64;
            BasicHeader = new ChunkBasicHeader(fmt, (ushort) csid);

            if (fmt == 0)
                await ParseType0();
            else if (fmt == 1)
                await ParseType1();
            else if (fmt == 2)
                await ParseType2();
            else
                await ParseType3();
        }

        // Type.0 - 11 bytes
        private async Task ParseType0()
        {
            await _packet.ReceiveAsync(3);
            var length = BitHelper.ToInt32(await _packet.ReceiveAsync(3));
            var msgTypeId = await _packet.ReceiveAsync(1);
            var msgStreamId = BitHelper.ToInt32(await _packet.ReceiveAsync(4));
            MessageHeader = new ChunkMessageHeader(BasicHeader.Format, (uint) length, msgTypeId[0], msgStreamId);
            _lastHeader = MessageHeader;
            await ParseBody();
        }

        // Type.1 - 7 bytes
        private async Task ParseType1()
        {
            await _packet.ReceiveAsync(3);
            var length = BitHelper.ToInt32(await _packet.ReceiveAsync(3));
            var msgTypeId = await _packet.ReceiveAsync(1);
            MessageHeader = new ChunkMessageHeader(BasicHeader.Format, (uint) length, msgTypeId[0], _lastHeader.MessageStreamId);
            await ParseBody();
        }

        // Type.2 - 3 bytes
        private async Task ParseType2()
        {
            await _packet.ReceiveAsync(3);
            MessageHeader = new ChunkMessageHeader(BasicHeader.Format, _lastHeader.Length, _lastHeader.MessageTypeId, _lastHeader.MessageStreamId);
            await ParseBody();
        }

        private async Task ParseType3()
        {
            MessageHeader = new ChunkMessageHeader(BasicHeader.Format, _lastHeader.Length, _lastHeader.MessageTypeId, _lastHeader.MessageStreamId);
            await ParseBody();
        }

        private async Task ParseBody()
        {
            // current chunk
            var length = (int) MessageHeader.Length;
            var bytesRead = new List<byte>();
            var size = length > _packet.MaxChunkSize ? (int) _packet.MaxChunkSize : length;

            bytesRead.AddRange(await _packet.ReceiveAsync(size));

            // > next chunk
            while (bytesRead.Count < length)
            {
                await _packet.ReceiveAsync(1); // Basic Header 1 byte + Message Header 0 byte
                size = length - bytesRead.Count > _packet.MaxChunkSize ? (int) _packet.MaxChunkSize : length - bytesRead.Count;
                bytesRead.AddRange(await _packet.ReceiveAsync(size));
            }
            Body = bytesRead.AsReadOnly();
        }
    }
}