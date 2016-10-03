namespace Corvus.Amf.v0
{
    public class AmfProperty
    {
        public string Name { get; }

        public AmfData Value { get; }

        public AmfProperty(string name, AmfData value)
        {
            Name = name;
            Value = value;
        }

        public byte[] GetBytes() => AmfEncoder.EncodeProperty(this);
    }
}