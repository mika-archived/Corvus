using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Corvus.Amf.v0
{
    /// <summary>
    ///     byte[] を AmfData&lt;T&gt; に変換します。
    /// </summary>
    public static class AmfDecoder
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        public static List<AmfData> Decode(byte[] value)
        {
            var values = new List<AmfData>();
            using (var reader = CreateReader(value))
            {
                reader.BaseStream.Seek(0, SeekOrigin.Begin);
                while (true)
                {
                    var bytes = reader.ReadBytes(1);
                    if (bytes.Length == 0)
                        break;
                    var marker = bytes[0];
                    switch (marker)
                    {
                        case (byte) AmfMarker.Number:
                            values.Add(DecodeNumber64(reader));
                            break;

                        case (byte) AmfMarker.Boolean:
                            values.Add(DecodeBoolean(reader));
                            break;

                        case (byte) AmfMarker.String:
                            values.Add(DecodeString(reader));
                            break;

                        case (byte) AmfMarker.Object:
                            values.Add(DecodeObject(reader));
                            break;

                        case (byte) AmfMarker.Movieclip:
                            Debug.WriteLine("WARN: AMF0 Movieclip Type is not supported.");
                            break;

                        case (byte) AmfMarker.Null:
                            values.Add(new AmfNull());
                            break;

                        case (byte) AmfMarker.Undefined:
                            values.Add(new AmfUndefined());
                            break;

                        case (byte) AmfMarker.Reference:
                            values.Add(DecodeReference(reader));
                            break;

                        case (byte) AmfMarker.EcmaArray:
                            values.Add(DecodeEcmaArray(reader));
                            break;

                        case (byte) AmfMarker.ObjectEnd:
                            Debug.WriteLine("WARN: Incongruous AMF0 Object End Type.");
                            break;

                        case (byte) AmfMarker.StrictArray:
                            values.Add(DecodeStrictArray(reader));
                            break;

                        case (byte) AmfMarker.Date:
                            values.Add(DecodeDate(reader));
                            break;

                        case (byte) AmfMarker.LongString:
                            values.Add(DecodeLongString(reader));
                            break;

                        case (byte) AmfMarker.Unsupported:
                            values.Add(new AmfUnsupported());
                            break;

                        case (byte) AmfMarker.Recordset:
                            Debug.WriteLine("WARN: AMF0 RecordSet Type is not supported.");
                            break;

                        case (byte) AmfMarker.XmlDocument:
                            values.Add(DecodeXmlDocument(reader));
                            break;

                        case (byte) AmfMarker.TypedObject:
                            values.Add(DecodeTypedObject(reader));
                            break;

                        default:
                            Debug.WriteLine("WARN: Invalid AMF0 Type.");
                            break;
                    }
                }
            }
            return values;
        }

        private static AmfData Decode(BinaryReader br)
        {
            var bytes = br.ReadBytes(1);
            if (bytes.Length == 0)
                return null;
            var marker = bytes[0];
            switch (marker)
            {
                case (byte) AmfMarker.Number:
                    return DecodeNumber64(br);

                case (byte) AmfMarker.Boolean:
                    return DecodeBoolean(br);

                case (byte) AmfMarker.String:
                    return DecodeString(br);

                case (byte) AmfMarker.Object:
                    return DecodeObject(br);

                case (byte) AmfMarker.Movieclip:
                    Debug.WriteLine("WARN: AMF0 Movieclip Type is not supported.");
                    return null;

                case (byte) AmfMarker.Null:
                    return new AmfNull();

                case (byte) AmfMarker.Undefined:
                    return new AmfUndefined();

                case (byte) AmfMarker.Reference:
                    return DecodeReference(br);

                case (byte) AmfMarker.EcmaArray:
                    return DecodeEcmaArray(br);

                case (byte) AmfMarker.ObjectEnd:
                    Debug.WriteLine("WARN: Incongruous AMF0 Object End Type.");
                    return null;

                case (byte) AmfMarker.StrictArray:
                    return DecodeStrictArray(br);

                case (byte) AmfMarker.Date:
                    return DecodeDate(br);

                case (byte) AmfMarker.LongString:
                    return DecodeLongString(br);

                case (byte) AmfMarker.Unsupported:
                    return new AmfUnsupported();

                case (byte) AmfMarker.Recordset:
                    Debug.WriteLine("WARN: AMF0 RecordSet Type is not supported.");
                    return null;

                case (byte) AmfMarker.XmlDocument:
                    return DecodeXmlDocument(br);

                case (byte) AmfMarker.TypedObject:
                    return DecodeTypedObject(br);

                default:
                    Debug.WriteLine("WARN: Invalid AMF0 Type.");
                    return null;
            }
        }

        private static BinaryReader CreateReader(byte[] value)
        {
            var bw = new BinaryWriter(new MemoryStream());
            bw.Write(value);
            return new BinaryReader(bw.BaseStream);
        }

        // Object-End is [00 00 09].
        private static bool IsObjectEnd(BinaryReader br)
        {
            var value = DecodeNumber16(br);
            if ((value == 0x00) && ((byte) br.Read() == (byte) AmfMarker.ObjectEnd))
                return true;
            br.BaseStream.Seek(-2, SeekOrigin.Current);
            return false;
        }

        // Number (16 bit)
        // [Value - 2 bytes]
        private static short DecodeNumber16(BinaryReader br)
        {
            var bytes = br.ReadBytes(2).Reverse().ToArray();
            return BitConverter.ToInt16(bytes, 0);
        }

        // Number (24 bit)
        // [Value - 4 bytes]
        private static int DecodeNumber32(BinaryReader br)
        {
            var bytes = br.ReadBytes(4).Reverse().ToArray();
            return BitConverter.ToInt32(bytes, 0);
        }

        // Number (64 bit)
        // [Marker] [Value - 8 bytes]
        private static AmfData DecodeNumber64(BinaryReader br)
        {
            var bytes = br.ReadBytes(8).Reverse().ToArray();
            return new AmfValue<double>(BitConverter.ToDouble(bytes, 0));
        }

        // Boolean
        // [Marker] [Value - 1 byte]
        private static AmfData DecodeBoolean(BinaryReader br)
        {
            var bytes = br.ReadBytes(1);
            return new AmfValue<bool>(bytes[0] == 0x01);
        }

        // String
        // [Marker] [Length - 2 bytes] [ Value - variable]
        private static AmfData DecodeString(BinaryReader br)
        {
            var length = DecodeNumber16(br);
            var bytes = br.ReadBytes(length);
            return new AmfValue<string>(Encoding.UTF8.GetString(bytes));
        }

        // String
        // [Marker] [Length - 4 bytes] [ Value - variable]
        private static AmfData DecodeLongString(BinaryReader br)
        {
            var length = DecodeNumber32(br);
            var bytes = br.ReadBytes(length);
            return new AmfValue<string>(Encoding.UTF8.GetString(bytes));
        }

        // Property (Object-Property)
        // [Length - 2 bytes] [Name] [Value]
        private static AmfProperty DecodeProperty(BinaryReader br)
        {
            var name = DecodeString(br);
            var value = Decode(br);
            return new AmfProperty((string) name.Value, value);
        }

        // Object
        // [Marker] [Properties...] [Marker]
        private static AmfObject DecodeObject(BinaryReader br)
        {
            var obj = new AmfObject();
            while (true)
            {
                if (IsObjectEnd(br))
                    break;
                obj.Value.Add(DecodeProperty(br));
            }
            return obj;
        }

        // Reference
        // [Marker] [Value - 2 bytes]
        private static AmfReference DecodeReference(BinaryReader br)
        {
            var value = DecodeNumber16(br);
            return new AmfReference {Value = value};
        }

        // ECMA Array
        // [Marker] [Length] [Properties...]
        private static AmfEcmaArray DecodeEcmaArray(BinaryReader br)
        {
            DecodeNumber32(br);
            var obj = new AmfEcmaArray();
            while (true)
            {
                if (IsObjectEnd(br))
                    break;
                obj.Value.Add(DecodeProperty(br));
            }
            return obj;
        }

        // Strict Array
        private static AmfStrictArray DecodeStrictArray(BinaryReader br)
        {
            DecodeNumber32(br);
            var obj = new AmfStrictArray();
            while (true)
            {
                if (IsObjectEnd(br))
                    break;
                obj.Value.Add(Decode(br));
            }
            return obj;
        }

        // Date
        // [Marker] [Value - 8 bytes] [Timezone - 2 bytes]
        private static AmfData DecodeDate(BinaryReader br)
        {
            var value = (double) DecodeNumber64(br).Value;
            DecodeNumber16(br); // timezone
            return new AmfValue<DateTime>(UnixEpoch.AddMilliseconds(value));
        }

        // XML document
        // [Marker] [Value - variable]
        private static AmfData DecodeXmlDocument(BinaryReader br)
        {
            var xml = DecodeLongString(br);
            return new AmfValue<XmlReader>(XmlReader.Create(new StringReader((string) xml.Value)));
        }

        // Typed Object
        // [Marker] [ClassName] [Properties...] [Marker]
        private static AmfTypedObject DecodeTypedObject(BinaryReader br)
        {
            var className = (string) DecodeString(br).Value;
            var obj = new AmfTypedObject {ClassName = className};
            while (true)
            {
                if (IsObjectEnd(br))
                    break;
                obj.Value.Add(DecodeProperty(br));
            }
            return obj;
        }
    }
}