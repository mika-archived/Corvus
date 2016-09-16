using System.Collections.Generic;

namespace Corvus.Amf.v0
{
    public class AmfReference : AmfData<short>
    {
        public override AmfMarker Marker => AmfMarker.Reference;

        public byte[] GetBytes()
        {
            var bytes = new List<byte> {(byte) AmfMarker.Reference};
            bytes.AddRange(AmfEncoder.EncodeNumber16(Value));
            return bytes.ToArray();
        }
    }
}