using System;
using System.Linq;

namespace Corvus.Chunking
{
    internal class ChunkMessageHeader
    {
        public byte Format { get; }
        public uint Length { get; }
        public byte MessageTypeId { get; }
        public int MessageStreamId { get; }

        public ChunkMessageHeader(byte format, uint length, byte msgTypeId, int msgStreamId)
        {
            if (3 < format)
                throw new ArgumentOutOfRangeException(nameof(format));
            Format = format;
            Length = length;
            MessageTypeId = msgTypeId;
            MessageStreamId = msgStreamId;
        }

        public byte[] GetBytes()
        {
            byte[] bytes;
            if (Format == 0)
            {
                // Type.0 (11 bytes)
                bytes = new byte[11];
                ArrayHelper.Fill(bytes, 0x00, 3);
                ArrayHelper.Concat(bytes, BitConverter.GetBytes(Length).Take(3).Reverse().ToArray(), 3); // FIXME: Wrong body size.
                bytes[6] = MessageTypeId;
                ArrayHelper.Concat(bytes, BitConverter.GetBytes(MessageStreamId).ToArray(), 7);
            }
            else if (Format == 1)
            {
                // Type.1 (7 bytes)
                bytes = new byte[7];
                ArrayHelper.Fill(bytes, 0x00, 3); // FIXME: Timestamp delta.
                ArrayHelper.Concat(bytes, BitConverter.GetBytes(Length).Take(3).ToArray(), 3);
                bytes[6] = MessageTypeId;
            }
            else if (Format == 2)
            {
                // Type.2 (3 bytes)
                bytes = new byte[3];
                ArrayHelper.Fill(bytes, 0x00, 3); // FIXME: Timestamp delta.
            }
            else
                bytes = new byte[0];

            return bytes;
        }
    }
}