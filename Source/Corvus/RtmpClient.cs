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
        private readonly Random _random;
        protected Packet Packet { get; }

        protected RtmpClient(Uri rtmpUri)
        {
            Packet = new Packet(rtmpUri);
            _random = new Random();
        }

        /// <summary>
        ///     指定された RtmpUrl に対して、接続を行います。
        /// </summary>
        public void Connect()
        {
            Packet.PrepareAsync();
            Handshake();
        }

        /// <summary>
        ///     URI に対して、 Handshake を行います。
        /// </summary>
        private void Handshake()
        {
            // C0
            Packet.SendAsync(new byte[] {0x03});

            // C1
            var c1 = new byte[1536];
            var time = Timestamp.GetNow();
            ArrayHelper.Concat(c1, time); // time (4 bytes)
            ArrayHelper.Fill(c1, 0x00, 4, 4); // 0x00 (4 bytes)
            for (var i = 8; i < 1536; i++) // random (1528 bytes)
                c1[i] = (byte) _random.Next(0x00, 0xff);
            Packet.SendAsync(c1);

            // S0
            var s0 = Packet.ReceiveAsync(1);
            if (s0[0] != 0x03)
                throw new NotSupportedException($"Corvus is supported RTMP 3, but response version is {s0[0]}");

            // S1
            var s1 = Packet.ReceiveAsync(1536);
            if (!c1.Skip(8).ToArray().SequenceEqual(s1.Skip(8).ToArray()))
                Debug.WriteLine("WARN: C1 Random and S1 Random does not match.");

            // C2
            var c2 = new byte[1536];
            ArrayHelper.Concat(c2, time); // time (4 bytes)
            ArrayHelper.Concat(c2, Timestamp.GetNow(), 4); // time2 (4 bytes)
            ArrayHelper.Concat(c2, ArrayHelper.Take(s1, 8, 1528), 8); // random echo (1528 bytes)
            Packet.SendAsync(c2);

            // S2
            Packet.ReceiveAsync(1536);
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