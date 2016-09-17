using System.Collections.Generic;

using Corvus.Amf.v0;

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
        public void Connect()
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
        }
    }
}