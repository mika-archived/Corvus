using System;
using System.Xml;

namespace Corvus.Amf.v0
{
    public class AmfValue<T> : AmfData<T>
    {
        public override AmfMarker Marker => AmfMarker.Undefined;

        public AmfValue(T value)
        {
            if ((typeof(T) == typeof(double)) || (typeof(T) == typeof(int)) ||
                (typeof(T) == typeof(short)) || (typeof(T) == typeof(bool)) ||
                (typeof(T) == typeof(string)) || (typeof(T) == typeof(DateTime)) ||
                (typeof(T) == typeof(XmlReader)))
            {
                Value = value;
                return;
            }
            throw new NotSupportedException();
        }
    }
}