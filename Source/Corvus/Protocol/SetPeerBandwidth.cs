using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.Protocol
{
    internal class SetPeerBandwidth : Protocol
    {
        public int Size { get; private set; }

        public LimitType Limit { get; private set; }

        public override async Task Read(Packet packet)
        {
            var reader = new ChunkReader(packet);
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
    }
}