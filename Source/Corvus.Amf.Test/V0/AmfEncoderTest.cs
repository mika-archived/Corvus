using System.Collections.Generic;

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
    }
}