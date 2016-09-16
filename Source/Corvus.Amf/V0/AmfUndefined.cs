namespace Corvus.Amf.V0
{
    public class AmfUndefined : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Undefined;
    }
}