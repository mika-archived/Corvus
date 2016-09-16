namespace Corvus.Amf.v0
{
    public class AmfUndefined : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Undefined;
    }
}