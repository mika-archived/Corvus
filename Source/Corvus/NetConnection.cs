using System.Diagnostics;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Chunking;
using Corvus.Commands.NetConnection;
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
            var connect = new ConnectCommand(_rtmpClient) {OptionalUserArgumets = new AmfValue<string>(_rtmpClient.UserArguments)};
            var chunk = new ChunkHeader(0, (byte) ChunkStreamId.HighLevel, (byte) MessageType.CommandMessage0, 0, connect.GetBytes());
            await _rtmpClient.Packet.SendAsync(chunk, connect.GetBytes());

            // [Window Acknowledgement Size] + [Set Peer Bandwidth] + [_result()]
            var was = new WindowAcknowledgementSize();
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