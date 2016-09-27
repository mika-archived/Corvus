using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;
using Corvus.Protocol;

namespace Corvus.ProtocolMessages
{
    internal class SetPeerBandwidth : ProtocolMessage
    {
        public int Size { get; private set; }

        public LimitType Limit { get; private set; }

        public SetPeerBandwidth(Packet packet) : base(packet) {}

        public override async Task Read()
        {
            var reader = new ChunkReader(Packet);
            await reader.Read();

            var bytes = reader.Body;
            Size = BitHelper.ToInt32(bytes.Take(4));
            var limit = bytes[4];
            if (limit == 0)
                Limit = LimitType.Hard;
            else if (limit == 1)
                Limit = LimitType.Soft;
            else
                Limit = LimitType.Dynamic;
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}