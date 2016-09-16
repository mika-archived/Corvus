namespace Corvus.Amf.v0
{
    public class AmfUnsupported : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Unsupported;
    }
}