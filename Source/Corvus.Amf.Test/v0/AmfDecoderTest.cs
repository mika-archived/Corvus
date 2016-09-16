using System.Linq;

using Corvus.Amf.v0;

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Corvus.Amf.Test.v0
{
    [TestClass]
    public class AmfDecoderTest
    {
        [TestMethod]
        public void DecodeNumber64Test()
        {
            var actual = AmfDecoder.Decode(ByteArray.Number64Bytes).First();
            Assert.AreEqual(3575d, (double) actual.Value);
        }

        [TestMethod]
        public void DecodeBooleanTest()
        {
            var actual = AmfDecoder.Decode(ByteArray.BooleanBytes).First();
            Assert.AreEqual(false, (bool) actual.Value);
        }

        [TestMethod]
        public void DecodeStringTest()
        {
            var actual = AmfDecoder.Decode(ByteArray.StringBytes).First();
            Assert.AreEqual("connect", (string) actual.Value);
        }

        [TestMethod]
        public void DecodeObjectTest()
        {
            var actual = AmfDecoder.Decode(ByteArray.ObjectBytes).First();
            Assert.AreEqual(typeof(AmfObject), actual.GetType()); // Type check

            var obj = (AmfObject) actual;
            Assert.AreEqual("fmsVer", obj.Value[0].Name);
            Assert.AreEqual("FMS/3,5,7,7009", (string) obj.Value[0].Value.Value);
            Assert.AreEqual("capabilities", obj.Value[1].Name);
            Assert.AreEqual(127d, (double) obj.Value[1].Value.Value);
        }

        [TestMethod]
        public void DecodeReferenceTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DecodeEcmaArrayTest()
        {
            var actual = AmfDecoder.Decode(ByteArray.EcmaArrayBytes).First();
            Assert.AreEqual(typeof(AmfEcmaArray), actual.GetType()); // Type check

            var obj = (AmfEcmaArray) actual;
            Assert.AreEqual("version", obj.Value[0].Name);
            Assert.AreEqual("3,5,7,7009", (string) obj.Value[0].Value.Value);
        }

        [TestMethod]
        public void DecodeStrictArrayTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DecodeDateTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DecodeXmlDocumentTest()
        {
            Assert.Inconclusive();
        }

        [TestMethod]
        public void DecodeTypedObjectTest()
        {
            Assert.Inconclusive();
        }
    }
}