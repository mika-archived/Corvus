using System.Collections.Generic;

namespace Corvus.Amf.v0
{
    internal class AmfObjectEnd : AmfData<short>
    {
        public override AmfMarker Marker => AmfMarker.ObjectEnd;

        public AmfObjectEnd()
        {
            Value = 0;
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();
            bytes.AddRange(AmfEncoder.EncodeNumber16(0));
            bytes.Add((byte) Marker);
            return bytes.ToArray();
        }
    }
}