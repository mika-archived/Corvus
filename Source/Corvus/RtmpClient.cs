using System;
using System.Diagnostics;
using System.Linq;

namespace Corvus
{
    /// <summary>
    ///     RTMP を扱います。
    /// </summary>
    public abstract class RtmpClient
    {
        private readonly NetConnection _netConnection;
        private readonly Random _random;
        protected internal Packet Packet { get; }

        protected RtmpClient(Uri rtmpUri)
        {
            Packet = new Packet(rtmpUri);
            _random = new Random();
            _netConnection = new NetConnection(this);
        }

        /// <summary>
        ///     指定された RtmpUrl に対して、接続を行います。
        /// </summary>
        public void Connect()
        {
            Packet.PrepareAsync();
            Handshake();
            _netConnection.Connect();
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
        private void Handshake()
        {
            // C0+C1
            var c0 = new byte[1537];
            c0[0] = 0x03;
            var time = Timestamp.GetNow();
            ArrayHelper.Concat(c0, time, 1); // time (4 bytes)
            ArrayHelper.Fill(c0, 0x00, 4, 5); // 0x00 (4 bytes)
            for (var i = 9; i < 1537; i++) // random (1528 bytes)
                c0[i] = (byte) _random.Next(0x00, 0xff);
            Packet.SendAsync(c0);

            // S0
            var s0 = Packet.ReceiveAsync(1);
            if (s0[0] != 0x03)
                throw new NotSupportedException($"Corvus is supported RTMP 3, but response version is {s0[0]}");

            // S1
            var s1 = Packet.ReceiveAsync(1536);

            // C2
            ArrayHelper.Concat(s1, Timestamp.GetNow(), 4); // time2 (4 bytes)
            Packet.SendAsync(s1);

            // Task.Delay(TimeSpan.FromMilliseconds(1000));
            // S2
            var s2 = Packet.ReceiveAsync(1536);
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
        ///     SEF ファイルの URL
        /// </summary>
        public string SwfUrl { get; set; }

        /// <summary>
        ///     サーバー URL
        /// </summary>
        public string TcUrl { get; set; }

        /// <summary>
        ///     プロキシを使用するか
        /// </summary>
        public bool Fpad { get; set; }

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
        ///     SWF ファイルがロードされるページ URL
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        ///     AMF エンコードメソッド
        /// </summary>
        public int ObjectEncoding => 3; // AMF3

        #endregion Properties
    }
}