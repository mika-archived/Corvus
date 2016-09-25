using System.Collections.Generic;
using System.Collections.ObjectModel;

using Corvus.Amf.v0;

namespace Corvus.Commands.NetConnection
{
    internal class ConnectCommand : INetCommand
    {
        public ConnectCommand(RtmpClient rtmpClient)
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

        public string CommandName => "connect"; // Set to `connect`.
        public int TransactionId => 1; // Always set to `1`.
        public ReadOnlyCollection<AmfData> CommandObject { get; }
        public AmfData OptionalUserArgumets { get; set; }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();
            foreach (var amfData in CommandObject)
                bytes.AddRange(amfData.GetBytes());
            bytes.AddRange(OptionalUserArgumets.GetBytes());
            return bytes.ToArray();
        }
    }
}