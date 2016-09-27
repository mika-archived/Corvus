using System.Collections.Generic;

namespace Corvus.Chunking
{
    public class ChunkHeader
    {
        private readonly ChunkBasicHeader _basicHeader;
        private readonly ChunkMessageHeader _messageHeader;

        public byte Format { get; }
        public ushort ChunkStreamId { get; }
        public byte MessageTypeId { get; }
        public int MessageStreamId { get; }

        public ChunkHeader(byte fmt, ushort csid, byte msgTypeId, int msgStreamId, byte[] body)
        {
            _basicHeader = new ChunkBasicHeader(fmt, csid);
            _messageHeader = new ChunkMessageHeader(fmt, (uint) body.Length, msgTypeId, msgStreamId);
            Format = fmt;
            ChunkStreamId = csid;
            MessageTypeId = msgTypeId;
            MessageStreamId = msgStreamId;
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(_basicHeader.GetBytes());
            bytes.AddRange(_messageHeader.GetBytes());
            // FIXME: Extended timestamp.
            return bytes.ToArray();
        }
    }
}