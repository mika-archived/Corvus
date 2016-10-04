using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class SetPeerBandwidth : ProtocolMessage
    {
        public int Size { get; private set; }

        public LimitType Limit { get; private set; }

        public SetPeerBandwidth(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            var bytes = Reader.Body;
            Size = BitHelper.ToInt32(bytes.Take(4));
            var limit = bytes[4];
            if (limit == 0)
                Limit = LimitType.Hard;
            else if (limit == 1)
                Limit = LimitType.Soft;
            else
                Limit = LimitType.Dynamic;
        }

        public override async Task Write()
        {
            var writer = new ChunkWriter(Packet) {ChunkStreamId = ChunkStreamId.LowLevel, MessageType = MessageType.WindowAckSize};
            writer.Write(Size);
            await writer.Flush();
        }
    }
}