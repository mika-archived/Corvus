using System;
using System.Threading.Tasks;

namespace Corvus.Protocol
{
    internal class SetPeerBandwidth : Protocol
    {
        public int Size { get; private set; }

        public LimitType Limit { get; private set; }

        public override async Task Read(Packet packet)
        {
            var bytes = await packet.ReceiveAsync(17);
            Size = BitConverter.ToInt32(bytes, 12);
            var limit = bytes[16];
            if (limit == 0)
                Limit = LimitType.Hard;
            else if (limit == 1)
                Limit = LimitType.Soft;
            else
                Limit = LimitType.Dynamic;
        }
    }
}