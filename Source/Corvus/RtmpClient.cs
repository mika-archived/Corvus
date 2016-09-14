using System;
using System.Diagnostics;
using System.Linq;

namespace Corvus
{
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
            // C0 を送信
            Packet.SendAsync(new byte[] {0x03});

            // C1 を送信
            var c1 = new byte[1536];
            var time = Timestamp.GetNow();
            ArrayHelper.Concat(c1, time); // time (4 bytes)
            ArrayHelper.Fill(c1, 0x00, 4, 4); // 0x00 (4 bytes)
            for (var i = 8; i < 1536; i++) // random (1528 bytes)
                c1[i] = (byte) _random.Next(0x00, 0xff);
            Packet.SendAsync(c1);

            // S0 を受信
            var s0 = Packet.ReceiveAsync(1);
            if (s0[0] != 0x03)
                throw new NotSupportedException($"Corvus is supported RTMP 3, but response version is {s0[0]}");

            // S1 を受信
            var s1 = Packet.ReceiveAsync(1536);
            if (!c1.Skip(8).ToArray().SequenceEqual(s1.Skip(8).ToArray()))
                Debug.WriteLine("WARN: C1 Random and S1 Random does not match.");

            // C2 を送信
            var c2 = new byte[1536];
            ArrayHelper.Concat(c2, time); // time (4 bytes)
            ArrayHelper.Concat(c2, Timestamp.GetNow(), 4); // time2 (4 bytes)
            ArrayHelper.Concat(c2, ArrayHelper.Take(s1, 8, 1528), 8); // random echo (1528 bytes)
            Packet.SendAsync(c2);

            // S2 を受信
            Packet.ReceiveAsync(1536);
        }

        #region Properties

        public string Connection { get; set; }
        public bool IsLive { get; set; }

        #endregion Properties
    }
}