namespace Corvus.Amf.v0
{
    public class AmfNull : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Null;
    }
}