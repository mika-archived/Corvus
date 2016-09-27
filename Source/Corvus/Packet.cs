using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

using Corvus.Chunking;

namespace Corvus
{
    // IDisposable?
    public class Packet
    {
        private readonly DataReader _reader;
        private readonly StreamSocket _socket;
        private readonly Uri _uri;
        private readonly DataWriter _writer;

        internal uint MaxChunkSize { get; set; }

        public Packet(Uri uri)
        {
            _uri = uri;
            MaxChunkSize = 128;
            _socket = new StreamSocket();
            _socket.Control.KeepAlive = true;
            _socket.Control.NoDelay = true;
            _writer = new DataWriter(_socket.OutputStream);
            _reader = new DataReader(_socket.InputStream);
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
            await _writer.FlushAsync();
        }

        /// <summary>
        ///     payload を送信します。
        /// </summary>
        /// <param name="header">RTMP Chunk Header</param>
        /// <param name="payload">送信するデータ</param>
        /// <param name="n">送信済みデータサイズ</param>
        /// <returns></returns>
        public async Task SendAsync(ChunkHeader header, byte[] payload, int n = 0)
        {
            if (n > payload.Length)
                return;

            var data = MaxChunkSize < payload.Length - n ? payload.Skip(n).Take((int) MaxChunkSize).ToArray() : payload.Skip(n).ToArray();
            var bytes = new List<byte>();
            bytes.AddRange(n > 0 ? new ChunkHeader(3, header.ChunkStreamId, 0, 0, new byte[0]).GetBytes() : header.GetBytes());
            bytes.AddRange(data);
            await SendAsync(bytes.ToArray());
            if (MaxChunkSize < payload.Length - n)
                await SendAsync(header, payload, n + (int) MaxChunkSize);
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