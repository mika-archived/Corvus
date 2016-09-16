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
    }
}