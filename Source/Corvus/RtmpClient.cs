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

                switch (reader.MessageHeader.MessageTypeId.ToMessageType())
                {
                    case MessageType.SetChunkSize:
                        var setChunkSize = new SetChunkSize(Packet, reader);
                        setChunkSize.Read();
                        break;

                    case MessageType.AbortMessage:
                        var abortMessage = new AbortMessage(Packet, reader);
                        abortMessage.Read();
                        break;

                    case MessageType.Acknowledgement:
                        // 通らないハズ...
                        var ack = new Acknowledgement(Packet, reader);
                        ack.Read();
                        break;

                    case MessageType.UserControlMessages:
                        var userControl = new UserControlMessages(Packet, reader);
                        userControl.Read();
                        break;

                    case MessageType.WindowAckSize:
                        var windowAckSize = new WindowAcknowledgementSize(Packet, reader);
                        windowAckSize.Read();
                        break;

                    case MessageType.SetPeerBandwidth:
                        var setPeerBandwidth = new SetPeerBandwidth(Packet, reader);
                        setPeerBandwidth.Read();
                        break;
                }
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
    }
}