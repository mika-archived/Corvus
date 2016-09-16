namespace Corvus.Amf.V0
{
    public class AmfUnsupported : AmfData<object>
    {
        public override AmfMarker Marker => AmfMarker.Unsupported;
    }
}