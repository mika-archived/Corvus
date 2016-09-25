using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Protocol;

namespace Corvus
{
    internal class NetConnection
    {
        private readonly RtmpClient _rtmpClient;

        public NetConnection(RtmpClient rtmpClient)
        {
            _rtmpClient = rtmpClient;
        }

        /// <summary>
        ///     NetConnection.connect を行います。
        /// </summary>
        public async Task Connect()
        {
            var body = new List<AmfData>
            {
                // Command Name
                new AmfValue<string>("connect"),
                // Transaction ID
                new AmfValue<int>(1),
                // Command Object
                new AmfObject
                {
                    Value = new List<AmfProperty>
                    {
                        new AmfProperty("app", new AmfValue<string>(_rtmpClient.App)),
                        new AmfProperty("flashVer", new AmfValue<string>(_rtmpClient.FlashVer)),
                        new AmfProperty("tcUrl", new AmfValue<string>(_rtmpClient.TcUrl)),
                        new AmfProperty("fpad", new AmfValue<bool>(_rtmpClient.Fpad)),
                        new AmfProperty("capabilities", new AmfValue<int>(_rtmpClient.Capabilities)),
                        new AmfProperty("audioCodecs", new AmfValue<int>(_rtmpClient.AudioCodecs)),
                        new AmfProperty("videoCodecs", new AmfValue<int>(_rtmpClient.VideoCodecs)),
                        new AmfProperty("videoFunction", new AmfValue<int>(_rtmpClient.VideoFunction)),
                        new AmfProperty("objectEncoding", new AmfValue<int>(_rtmpClient.ObjectEncoding))
                    }
                }
            };
            // User Arguments
            if (!string.IsNullOrWhiteSpace(_rtmpClient.UserArguments))
                // TODO: Parse user arguments to AMF0 format.
                //       Now, supported String(S:) only.
                body.Add(new AmfValue<string>(_rtmpClient.UserArguments));
            var byteArray = new List<byte>();
            foreach (var amfData in body)
                byteArray.AddRange(amfData.GetBytes());
            // connect()
            var chunk = new RtmpChunk(0, 3, (byte) MessageType.CommandMessage0, 0, byteArray.ToArray());
            await _rtmpClient.Packet.SendAsync(chunk.GetBytes());

            // Window Acknowledgement Size (4 bytes) + Set Peer Bandwidth + _result()
            var was = new WindowAckSize();
            await was.Read(_rtmpClient.Packet);

            var spb = new SetPeerBandwidth();
            await spb.Read(_rtmpClient.Packet);

            // _result()
            var bytesRead = await _rtmpClient.Packet.ReceiveAsync(10);
            Debug.WriteLine(spb);
            // Window Acknowledgement Size

            //
        }
    }
}