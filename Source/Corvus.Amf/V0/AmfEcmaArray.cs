using System.Collections.Generic;

namespace Corvus.Amf.V0
{
    public class AmfEcmaArray : AmfData<List<AmfProperty>>
    {
        public int Length => Value.Count;

        public override AmfMarker Marker => AmfMarker.EcmaArray;

        public AmfEcmaArray()
        {
            Value = new List<AmfProperty>();
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte> {(byte) AmfMarker.EcmaArray};
            bytes.AddRange(AmfEncoder.EncodeNumber32(Length - 1));
            foreach (var value in Value)
                bytes.AddRange(value.GetBytes());
            bytes.AddRange(new AmfObjectEnd().GetBytes());
            return bytes.ToArray();
        }
    }
}