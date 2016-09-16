using System.Collections.Generic;

using Corvus.Amf.V0;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Corvus.Amf.Test.V0
{
    [TestClass]
    public class AmfEncoderTest
    {
        [TestMethod]
        public void EncodeNumber64Test()
        {
            var expected = string.Join(" ", new byte[] {0x00, 0x40, 0xab, 0xee, 0x00, 0x00, 0x00, 0x00, 0x00});
            var actual = string.Join(" ", AmfEncoder.EncodeNumber64(3575));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeBooleanTest()
        {
            var expected = string.Join(" ", new byte[] {0x01, 0x00});
            var actual = string.Join(" ", AmfEncoder.EncodeBoolean(false));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeStringTest()
        {
            var expected = string.Join(" ", new byte[] {0x02, 0x00, 0x07, 0x63, 0x6f, 0x6e, 0x6e, 0x65, 0x63, 0x74});
            var actual = string.Join(" ", AmfEncoder.EncodeString("connect"));
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeObjectTest()
        {
            var expected = string.Join(" ", new byte[]
            {
                0x03, 0x00, 0x06, 0x66, 0x6d, 0x73, 0x56, 0x65,
                0x72, 0x02, 0x00, 0x0e, 0x46, 0x4d, 0x53, 0x2f,
                0x33, 0x2c, 0x35, 0x2c, 0x37, 0x2c, 0x37, 0x30,
                0x30, 0x39, 0x00, 0x0c, 0x63, 0x61, 0x70, 0x61,
                0x62, 0x69, 0x6c, 0x69, 0x74, 0x69, 0x65, 0x73,
                0x00, 0x40, 0x5f, 0xc0, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x09
            });
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
            var expected = string.Join(" ", new byte[]
            {
                0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x07, 0x76,
                0x65, 0x72, 0x73, 0x69, 0x6f, 0x6e, 0x02, 0x00,
                0x0a, 0x33, 0x2c, 0x35, 0x2c, 0x37, 0x2c, 0x37,
                0x30, 0x30, 0x39, 0x00, 0x00, 0x09
            });
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
    }
}