using System.Diagnostics;
using System.Threading.Tasks;

using Corvus.Amf.v0;
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

            // [Window Acknowledgement Size] + [Set Peer Bandwidth] + [_result()]
            var was = new WindowAcknowledgementSize(_rtmpClient.Packet);
            await was.Read();

            var spb = new SetPeerBandwidth(_rtmpClient.Packet);
            await spb.Read();

            // _result()
            var bytesRead = await _rtmpClient.Packet.ReceiveAsync(10);
            Debug.WriteLine(spb);
            // Window Acknowledgement Size

            //
        }
    }
}