using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Corvus.Chunking
{
    internal class ChunkHeader
    {
        private readonly ChunkBasicHeader _basicHeader;
        private readonly ChunkMessageHeader _messageHeader;
        private readonly ReadOnlyCollection<byte> _payload;

        public ChunkHeader(byte fmt, ushort csid, byte msgTypeId, int msgStreamId, byte[] body)
        {
            _basicHeader = new ChunkBasicHeader(fmt, csid);
            _messageHeader = new ChunkMessageHeader(fmt, (uint) body.Length, msgTypeId, msgStreamId);
            _payload = new ReadOnlyCollection<byte>(body);
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(_basicHeader.GetBytes());
            bytes.AddRange(_messageHeader.GetBytes());
            // FIXME: Extended timestamp.
            bytes.AddRange(_payload);
            return bytes.ToArray();
        }
    }
}