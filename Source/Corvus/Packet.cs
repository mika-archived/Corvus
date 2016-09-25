using System;
using System.Threading.Tasks;

using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Corvus
{
    // IDisposable?
    public class Packet
    {
        private readonly DataReader _reader;
        private readonly StreamSocket _socket;
        private readonly Uri _uri;
        private readonly DataWriter _writer;

        /// <summary>
        ///     タイムアウト
        /// </summary>
        public TimeSpan Timeout { get; set; }

        public Packet(Uri uri)
        {
            _uri = uri;
            _socket = new StreamSocket();
            _socket.Control.KeepAlive = true;
            _writer = new DataWriter(_socket.OutputStream);
            _reader = new DataReader(_socket.InputStream);

            Timeout = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        ///     接続要求を行います。
        /// </summary>
        public async Task PrepareAsync()
        {
            await _socket.ConnectAsync(new HostName(_uri.Host), _uri.Port.ToString());
        }

        /// <summary>
        ///     接続を閉じます。
        /// </summary>
        public void ShutdownAsync()
        {
            _writer.DetachStream();
            _reader.DetachStream();
            _socket.Dispose();
        }

        /// <summary>
        ///     payload を送信します。
        /// </summary>
        /// <param name="payload"></param>
        public async Task SendAsync(byte[] payload)
        {
            _writer.WriteBytes(payload);
            await _writer.StoreAsync();
        }

        /// <summary>
        ///     n バイト受信します。
        /// </summary>
        /// <param name="n">-1 を指定すると、すべて読み取ります。</param>
        /// <returns></returns>
        public async Task<byte[]> ReceiveAsync(int n)
        {
            var size = await _reader.LoadAsync((uint) n);
            var bytesRead = new byte[size];
            _reader.ReadBytes(bytesRead);

            return bytesRead;
        }
    }
}