using System;

namespace Corvus.Chunking
{
    internal class ChunkBasicHeader
    {
        public byte Format { get; }
        public ushort ChunkStreamId { get; }

        public int Size => GetBytes().Length;

        public ChunkBasicHeader(byte fmt, ushort csid)
        {
            if (3 < fmt)
                throw new ArgumentOutOfRangeException(nameof(fmt));
            Format = fmt;
            ChunkStreamId = csid;
        }

        public byte[] GetBytes()
        {
            byte[] bytes;
            if (ChunkStreamId < 64)
            {
                // 1 byte
                bytes = new byte[1];
                bytes[0] = (byte) ((Format << 6) + ChunkStreamId);
            }
            else if ((64 <= ChunkStreamId) && (ChunkStreamId < 320))
            {
                // 2 bytes
                bytes = new byte[2];
                bytes[0] = (byte) ((Format << 6) + 0);
                bytes[1] = (byte) (ChunkStreamId - 64);
            }
            else
            {
                // 3 bytes
                bytes = new byte[3];
                bytes[0] = (byte) ((Format << 6) + 0);
                // Big endian
                bytes[1] = BitConverter.GetBytes(ChunkStreamId - 64)[1];
                bytes[2] = BitConverter.GetBytes(ChunkStreamId - 64)[0];
            }
            return bytes;
        }
    }
}