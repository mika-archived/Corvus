namespace Corvus.Amf.V0
{
    public abstract class AmfData
    {
        public abstract AmfMarker Marker { get; }
        public object Value { get; set; }
    }
}