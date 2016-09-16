using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Corvus.Amf.V0
{
    /// <summary>
    ///     値を byte[] に変換します。
    /// </summary>
    public static class AmfEncoder
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        // Number (16 bit)
        // [Value - 2 bytes]
        internal static byte[] EncodeNumber16(short value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        // Number (32 bit)
        // [Value - 4 bytes]
        internal static byte[] EncodeNumber32(int value)
        {
            return BitConverter.GetBytes(value).Reverse().ToArray();
        }

        // Number (64 bit)
        // [Marker] [Value - 8 bytes]
        public static byte[] EncodeNumber64(double value)
        {
            var bytes = new List<byte> {(byte) AmfMarker.Number};
            bytes.AddRange(BitConverter.GetBytes(value).Reverse());
            return bytes.ToArray();
        }

        // Boolean
        // [Marker] [Value - 1 byte]
        public static byte[] EncodeBoolean(bool value)
        {
            var bytes = new byte[2];
            bytes[0] = (byte) AmfMarker.Boolean;
            bytes[1] = (byte) (value ? 0x01 : 0x00);
            return bytes;
        }

        // String
        // [Marker] [Length - 2 or 4 bytes] [Value - variable]
        public static byte[] EncodeString(string value, bool forceEncode32 = false)
        {
            var bytes = new List<byte>();
            if ((value.Length > 65535) || forceEncode32)
            {
                bytes.Add((byte) AmfMarker.LongString);
                bytes.AddRange(EncodeNumber32(value.Length));
            }
            else
            {
                bytes.Add((byte) AmfMarker.String);
                bytes.AddRange(EncodeNumber16((short) value.Length));
            }
            bytes.AddRange(Encoding.UTF8.GetBytes(value));
            return bytes.ToArray();
        }

        // Property (Object-Property)
        // [Length - 2 bytes] [Name] [Value]
        internal static byte[] EncodeProperty(AmfProperty value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(EncodeNumber16((short) value.Name.Length));
            bytes.AddRange(Encoding.UTF8.GetBytes(value.Name));
            bytes.AddRange(Encode(value.Value));
            return bytes.ToArray();
        }

        // Object
        // [Marker] [Properties ...] [Marker]
        public static byte[] EncodeObject(AmfObject value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.GetBytes());
            return bytes.ToArray();
        }

        // Reference
        // [Marker] [Value - 2 bytes]
        public static byte[] EncodeReference(AmfReference value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.GetBytes());
            return bytes.ToArray();
        }

        // ECMA Array
        // [Marker] [Count - 4 bytes] [Properties ...]
        public static byte[] EncodeEcmaArray(AmfEcmaArray value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.GetBytes());
            return bytes.ToArray();
        }

        // Strict Array
        // [Marker]
        public static byte[] EncodeStrictArray(AmfStrictArray value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.GetBytes());
            return bytes.ToArray();
        }

        // Date
        // [Marker] [Value - 8 bytes] [Timezone - 2 byte]
        public static byte[] EncodeDate(DateTime value)
        {
            var bytes = new List<byte> {(byte) AmfMarker.Date};
            bytes.AddRange(EncodeNumber64((value - UnixEpoch).TotalMilliseconds));
            bytes.AddRange(EncodeNumber16(0)); // reserved, not supported should be set to 0x0000
            return bytes.ToArray();
        }

        // XML document
        // [Marker] [Value - variable]
        public static byte[] EncodeXmlDocument(XmlReader value)
        {
            var sw = new StringWriter();
            using (var writer = XmlWriter.Create(sw))
                writer.WriteNode(value, true);
            var bytes = new List<byte> {(byte) AmfMarker.XmlDocument};
            bytes.AddRange(EncodeString(sw.ToString(), true));
            return bytes.ToArray();
        }

        // Typed Object
        // [Marker] [ClassName] [Properties...] [Marker]
        public static byte[] EncodeTypedObject(AmfTypedObject value)
        {
            var bytes = new List<byte>();
            bytes.AddRange(value.GetBytes());
            return bytes.ToArray();
        }

        // Encode Objects by Type
        public static byte[] Encode(object value)
        {
            double d;
            var flag = NumberStyles.Integer | NumberStyles.Float;
            if (double.TryParse(value.ToString(), flag, null, out d))
                return EncodeNumber64(double.Parse(value.ToString()));
            if (value is bool)
                return EncodeBoolean((bool) value);
            if (value is string)
                return EncodeString((string) value);
            if (value is AmfObject)
                return EncodeObject((AmfObject) value);
            if (value is AmfReference)
                return EncodeReference((AmfReference) value);
            if (value is AmfEcmaArray)
                return EncodeEcmaArray((AmfEcmaArray) value);
            if (value is AmfStrictArray)
                return EncodeStrictArray((AmfStrictArray) value);
            if (value is DateTime)
                return EncodeDate((DateTime) value);
            if (value is XmlReader)
                return EncodeXmlDocument((XmlReader) value);
            if (value is AmfTypedObject)
                return EncodeTypedObject((AmfTypedObject) value);
            if (value is AmfData)
                return Encode(((AmfData) value).Value);

            throw new NotSupportedException($"Not supported type: {value.GetType().AssemblyQualifiedName}");
        }
    }
}