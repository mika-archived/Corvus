namespace Corvus.Amf.v0
{
    public class AmfNull : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Null;

        public override byte[] GetBytes() => new[] {(byte) AmfMarker.Null};
    }
}