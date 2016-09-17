using System.Collections.Generic;

namespace Corvus.Amf.v0
{
    public class AmfStrictArray : AmfData<List<AmfData>>
    {
        public int Length => Value.Count;

        public override AmfMarker Marker => AmfMarker.StrictArray;

        public AmfStrictArray()
        {
            Value = new List<AmfData>();
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte> {(byte) AmfMarker.StrictArray};
            bytes.AddRange(AmfEncoder.EncodeNumber32(Length - 1));
            foreach (var value in Value)
                bytes.AddRange(AmfEncoder.Encode(value.Value));
            bytes.AddRange(new AmfObjectEnd().GetBytes());
            return bytes.ToArray();
        }
    }
}