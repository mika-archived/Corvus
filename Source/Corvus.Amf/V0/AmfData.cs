namespace Corvus.Amf.v0
{
    public abstract class AmfData
    {
        public abstract AmfMarker Marker { get; }
        public object Value { get; set; }

        public abstract byte[] GetBytes();
    }
}