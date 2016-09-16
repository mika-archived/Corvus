namespace Corvus.Amf.V0
{
    public abstract class AmfData<T> : AmfData
    {
        public new T Value
        {
            get { return (T) base.Value; }
            set { base.Value = value; }
        }
    }
}