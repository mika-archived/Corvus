using System.Collections.Generic;
using System.Diagnostics;

using Corvus.Amf.v0;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Corvus.Amf.Test.v0
{
    [TestClass]
    public class AmfEncoderTest
    {
        [TestMethod]
        public void EncodeNumber64Test()
        {
            var expected = string.Join(" ", ByteArray.Number64Bytes);
            var actual = string.Join(" ", AmfEncoder.EncodeNumber64(3575));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeBooleanTest()
        {
            var expected = string.Join(" ", ByteArray.BooleanBytes);
            var actual = string.Join(" ", AmfEncoder.EncodeBoolean(false));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeStringTest()
        {
            var expected = string.Join(" ", ByteArray.StringBytes);
            var actual = string.Join(" ", AmfEncoder.EncodeString("connect"));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeObjectTest()
        {
            var expected = string.Join(" ", ByteArray.ObjectBytes);
            var obj = new AmfObject
            {
                Value = new List<AmfProperty>
                {
                    new AmfProperty("fmsVer", new AmfValue<string>("FMS/3,5,7,7009")),
                    new AmfProperty("capabilities", new AmfValue<double>(127))
                }
            };
            var actual = string.Join(" ", AmfEncoder.EncodeObject(obj));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeReferenceTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EncodeEcmaArrayTest()
        {
            var expected = string.Join(" ", ByteArray.EcmaArrayBytes);
            var obj = new AmfEcmaArray
            {
                Value = new List<AmfProperty>
                {
                    new AmfProperty("version", new AmfValue<string>("3,5,7,7009"))
                }
            };
            var actual = string.Join(" ", AmfEncoder.EncodeEcmaArray(obj));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeStrictArrayTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EncodeDateTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EncodeXmlDocmentTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void EncodeTypedObjectTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void RealDataTest1()
        {
            // Real data of connect command.
            var expected = string.Join(" ", ByteArray.RealDataArrayBytes);
            var nvPairs = new List<AmfData>
            {
                new AmfValue<string>("connect"),
                new AmfValue<int>(1),
                new AmfObject
                {
                    Value = new List<AmfProperty>
                    {
                        new AmfProperty("app", new AmfValue<string>("liveedge/live_160927_02_0")),
                        new AmfProperty("flashVer", new AmfValue<string>("WIN 23,0,0,162")),
                        new AmfProperty("swfUrl", new AmfValue<string>("http://live.nicovideo.jp/nicoliveplayer.swf?160530135720")),
                        new AmfProperty("tcUrl", new AmfValue<string>("rtmp://nleu22.live.nicovideo.jp:1935/liveedge/live_160927_02_0")),
                        new AmfProperty("fpad", new AmfValue<bool>(false)),
                        new AmfProperty("capabilities", new AmfValue<double>(239)),
                        new AmfProperty("audioCodecs", new AmfValue<double>(3575)),
                        new AmfProperty("videoCodecs", new AmfValue<double>(252)),
                        new AmfProperty("videoFunction", new AmfValue<double>(1)),
                        new AmfProperty("pageUrl", new AmfValue<string>("http://live.nicovideo.jp/watch/lv277223009?cc_referrer=live_top&ref=top_recommend_6_2")),
                        new AmfProperty("objectEncoding", new AmfValue<double>(3))
                    }
                },
                new AmfValue<string>("50563253:lv277223009:0:1474912349:fdddef66ec1562a6")
            };
            var bytes = new List<byte>();
            foreach (var amfData in nvPairs)
                bytes.AddRange(amfData.GetBytes());
            var actual = string.Join(" ", bytes.ToArray());
            Debug.WriteLine(expected);
            Debug.WriteLine(actual);
            Assert.AreEqual(expected, actual);
        }
    }
}