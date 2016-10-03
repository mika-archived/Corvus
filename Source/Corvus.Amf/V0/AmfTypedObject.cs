using System.Collections.Generic;

namespace Corvus.Amf.v0
{
    public class AmfTypedObject : AmfData<List<AmfProperty>>
    {
        public string ClassName { get; set; }

        public override AmfMarker Marker => AmfMarker.TypedObject;

        public AmfTypedObject()
        {
            Value = new List<AmfProperty>();
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte> {(byte) AmfMarker.TypedObject};
            bytes.AddRange(AmfEncoder.EncodeString(ClassName));
            foreach (var value in Value)
                bytes.AddRange(value.GetBytes());
            bytes.Add((byte) AmfMarker.ObjectEnd);
            return bytes.ToArray();
        }
    }
}