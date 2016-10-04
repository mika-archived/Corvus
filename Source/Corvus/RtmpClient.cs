using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;
using Corvus.ProtocolMessages;

namespace Corvus
{
    /// <summary>
    ///     RTMP を扱います。
    /// </summary>
    public abstract class RtmpClient
    {
        private readonly NetConnection _netConnection;
        private readonly Random _random;
        private bool _isConnected;
        protected internal Packet Packet { get; }

        protected RtmpClient(Uri rtmpUri)
        {
            Packet = new Packet(rtmpUri);
            _random = new Random();
            _netConnection = new NetConnection(this);

            App = rtmpUri.AbsolutePath;
            FlashVer = "WIN 23,0,0,162";
            TcUrl = rtmpUri.OriginalString;
            Fpad = false;
            Capabilities = 239;
            AudioCodecs = (int) (AudioCodec.SpeexAudio | AudioCodec.AAC | AudioCodec.G711U | AudioCodec.G711A | AudioCodec.NellyMouser |
                                 AudioCodec.NellyMouser8 | AudioCodec.UNUSED | AudioCodec.MP3 | AudioCodec.ADPCM | AudioCodec.None);
            VideoCodecs = (int) (VideoCodec.H264 | VideoCodec.HomebrewV | VideoCodec.VP6Alpha | VideoCodec.VP6 |
                                 VideoCodec.Homebrew | VideoCodec.Sorenson);
        }

        /// <summary>
        ///     指定された RtmpUrl に対して、接続を行います。
        /// </summary>
        public async Task Connect()
        {
            await Packet.PrepareAsync();
            await Handshake();
            await _netConnection.Connect();

            _isConnected = true;
#pragma warning disable 4014
            Task.Run(async () => await StreamLoop());
#pragma warning restore 4014
        }

        private async Task StreamLoop()
        {
            while (_isConnected)
            {
                var reader = new ChunkReader(Packet);
                await reader.Read();

                var messageType = reader.MessageHeader.MessageTypeId.ToMessageType();
                ProtocolMessage message = null;
                switch (reader.MessageHeader.MessageTypeId.ToMessageType())
                {
                    case MessageType.SetChunkSize:
                        message = new SetChunkSize(Packet, reader);
                        break;

                    case MessageType.AbortMessage:
                        message = new AbortMessage(Packet, reader);
                        break;

                    case MessageType.Acknowledgement:
                        // 通らないハズ...
                        message = new Acknowledgement(Packet, reader);
                        break;

                    case MessageType.UserControlMessages:
                        message = new UserControlMessages(Packet, reader);
                        break;

                    case MessageType.WindowAckSize:
                        message = new WindowAcknowledgementSize(Packet, reader);
                        break;

                    case MessageType.SetPeerBandwidth:
                        message = new SetPeerBandwidth(Packet, reader);
                        break;

                    case MessageType.AudioMessage:
                        message = new AudioMessage(Packet, reader);
                        break;

                    case MessageType.VideoMessage:
                        message = new VideoMessage(Packet, reader);
                        break;

                    case MessageType.DataMessage0:
                    case MessageType.DataMessage3:
                        message = new DataMessage(Packet, reader);
                        break;

                    case MessageType.SharedObjectMsg0:
                    case MessageType.SharedObjectMsg3:
                        // TODO: Impl
                        break;

                    case MessageType.CommandMessage0:
                    case MessageType.CommandMessage3:
                        message = new CommandMessage(Packet, reader);
                        break;

                    case MessageType.AggregateMessage:
                        message = new AggregateMessage(Packet, reader);
                        break;

                    case MessageType.NotSupport:
                        // TODO: Impl
                        break;
                }
                if (message == null)
                    return;
                message.Read();
                OnMessageReceived?.Invoke(new RtmpMessageReceivedEvent(messageType, message));
            }
        }

        /// <summary>
        ///     接続を閉じます。
        /// </summary>
        public void Disconnect()
        {
            Packet.ShutdownAsync();
        }

        /// <summary>
        ///     URI に対して、 Handshake を行います。
        /// </summary>
        private async Task Handshake()
        {
            // [C0] + [C1]
            var c0 = new byte[1537];
            c0[0] = 0x03;
            var time = Timestamp.GetNow();
            ArrayHelper.Concat(c0, time, 1); // time (4 bytes)
            ArrayHelper.Fill(c0, 0x00, 4, 5); // 0x00 (4 bytes)
            for (var i = 9; i < 1537; i++) // random (1528 bytes)
                c0[i] = (byte) _random.Next(0x00, 0xff);
            await Packet.SendAsync(c0);

            // [S0] + [S1] + [S2]
            var s0 = await Packet.ReceiveAsync(1 + 1536);
            if (s0[0] != 0x03)
                throw new NotSupportedException($"Corvus is supported RTMP 3, but response version is {s0[0]}");

            // [C2]
            var c2 = s0.Skip(1).ToArray();
            ArrayHelper.Concat(c2, Timestamp.GetNow(), 4); // time2 (4 bytes)
            await Packet.SendAsync(c2);

            // [S2]
            var s2 = await Packet.ReceiveAsync(1536);
            if (!c0.Skip(9).ToArray().SequenceEqual(s2.Skip(8).ToArray()))
                Debug.WriteLine("WARN: C1 Random and S2 Random does not match.");
        }

        #region Properties

        /// <summary>
        ///     接続するサーバーのアプリ名
        /// </summary>
        public string App { get; set; }

        /// <summary>
        ///     Flash Player バージョン
        /// </summary>
        public string FlashVer { get; set; }

        /// <summary>
        ///     サーバー URL
        /// </summary>
        public string TcUrl { get; set; }

        /// <summary>
        ///     プロキシを使用するか
        /// </summary>
        public bool Fpad { get; set; }

        /// <summary>
        ///     機能
        /// </summary>
        public int Capabilities { get; set; }

        /// <summary>
        ///     使用するオーディオコーデック
        /// </summary>
        public int AudioCodecs { get; set; }

        /// <summary>
        ///     使用するビデオコーデック
        /// </summary>
        public int VideoCodecs { get; set; }

        /// <summary>
        ///     サポートしている動画機能
        /// </summary>
        public int VideoFunction => 0x0001; // SUPPORT_VID_CLIENT_SEEK

        /// <summary>
        ///     AMF エンコードメソッド
        /// </summary>
        public int ObjectEncoding => 0x03; // AMF0

        /// <summary>
        ///     ユーザーパラメータ
        /// </summary>
        public string UserArguments { get; set; }

        #endregion Properties

        #region Events

        public delegate void RtmpMessageReceivedEventHandler(RtmpMessageReceivedEvent e);

        public event RtmpMessageReceivedEventHandler OnMessageReceived;

        #endregion
    }
}