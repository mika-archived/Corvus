using System.IO;

namespace Corvus.ProtocolMessages.Aggregate
{
    internal class AggregateHeader
    {
        public MessageType Type { get; set; }

        public int Size { get; set; }

        public int Timestamp { get; set; }

        public byte TimestampDelta { get; set; }

        public int StreamId { get; set; }

        public AggregateHeader(BinaryReader reader)
        {
            Type = reader.ReadByte().ToMessageType();
            Size = BitHelper.ToInt32(reader.ReadBytes(3));
            Timestamp = BitHelper.ToInt32(reader.ReadBytes(3));
            TimestampDelta = reader.ReadByte();
            StreamId = BitHelper.ToInt32(reader.ReadBytes(3));
        }
    }
}