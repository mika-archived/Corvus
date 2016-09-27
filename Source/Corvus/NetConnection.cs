using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Commands;
using Corvus.Commands.NetConnection;
using Corvus.ProtocolMessages;

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
            var connect = new ConnectCommand(_rtmpClient) {OptionalUserArgumets = new AmfValue<string>(_rtmpClient.UserArguments)};
            await connect.Invoke();

            var windowAckSize = new WindowAcknowledgementSize(_rtmpClient.Packet);
            await windowAckSize.Read();

            var setPeerBandwidth = new SetPeerBandwidth(_rtmpClient.Packet);
            await setPeerBandwidth.Read();

            var result = new ResultCommand(_rtmpClient.Packet);
            await result.Read();
        }
    }
}