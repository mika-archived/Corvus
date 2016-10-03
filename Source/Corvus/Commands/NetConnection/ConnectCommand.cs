using System.Collections.Generic;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Chunking;

namespace Corvus.Commands.NetConnection
{
    internal sealed class ConnectCommand : NetCommand
    {
        public override string CommandName => "connect"; // Set to `connect`.
        public override int TransactionId => 1; // Always set to `1`.

        public ConnectCommand(RtmpClient rtmpClient) : base(rtmpClient.Packet)
        {
            var nvPairs = new List<AmfData>
            {
                new AmfValue<string>(CommandName),
                new AmfValue<int>(TransactionId),
                new AmfObject
                {
                    Value = new List<AmfProperty>
                    {
                        new AmfProperty("app", new AmfValue<string>(rtmpClient.App)),
                        new AmfProperty("flashVer", new AmfValue<string>(rtmpClient.FlashVer)),
                        new AmfProperty("tcUrl", new AmfValue<string>(rtmpClient.TcUrl)),
                        new AmfProperty("fpad", new AmfValue<bool>(rtmpClient.Fpad)),
                        new AmfProperty("capabilities", new AmfValue<double>(rtmpClient.Capabilities)),
                        new AmfProperty("audioCodecs", new AmfValue<double>(rtmpClient.AudioCodecs)),
                        new AmfProperty("videoCodecs", new AmfValue<double>(rtmpClient.VideoCodecs)),
                        new AmfProperty("videoFunction", new AmfValue<double>(rtmpClient.VideoFunction)),
                        new AmfProperty("objectEncoding", new AmfValue<double>(rtmpClient.ObjectEncoding))
                    }
                }
            };
            CommandObject = nvPairs.AsReadOnly();
        }

        public override async Task Invoke()
        {
            var chunk = new ChunkHeader(0, (byte) ChunkStreamId.HighLevel, (byte) MessageType.CommandMessage0, 0, GetBytes());
            await Packet.SendAsync(chunk, GetBytes());
        }

        public override byte[] GetBytes()
        {
            var bytes = new List<byte>();
            foreach (var amfData in CommandObject)
                bytes.AddRange(amfData.GetBytes());
            bytes.AddRange(OptionalUserArgumets.GetBytes());
            return bytes.ToArray();
        }
    }
}