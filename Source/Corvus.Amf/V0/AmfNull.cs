namespace Corvus.Amf.V0
{
    public class AmfNull : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Null;
    }
}