using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Corvus
{
    // IDisposable?
    public class Packet
    {
        private readonly Socket _socket;
        private readonly Uri _uri;
        private readonly ManualResetEvent _waiter;

        /// <summary>
        ///     タイムアウト
        /// </summary>
        public TimeSpan Timeout { get; set; }

        public Packet(Uri uri)
        {
            _uri = uri;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _waiter = new ManualResetEvent(false);

            Timeout = TimeSpan.FromSeconds(5);
        }

        /// <summary>
        ///     接続要求を行います。
        /// </summary>
        public void PrepareAsync()
        {
            var socketArgs = new SocketAsyncEventArgs {RemoteEndPoint = new DnsEndPoint(_uri.Host, _uri.Port)};
            socketArgs.Completed += (sender, e) => { _waiter.Set(); };
            _waiter.Reset();
            _socket.ConnectAsync(socketArgs);
            _waiter.WaitOne((int) Timeout.TotalMilliseconds);
        }

        /// <summary>
        ///     接続を閉じます。
        /// </summary>
        public void ShutdownAsync()
        {
            _socket.Shutdown(SocketShutdown.Both);
        }

        /// <summary>
        ///     payload を送信します。
        /// </summary>
        /// <param name="payload"></param>
        public void SendAsync(byte[] payload)
        {
            var socketArgs = new SocketAsyncEventArgs {RemoteEndPoint = new DnsEndPoint(_uri.Host, _uri.Port)};
            socketArgs.SetBuffer(payload, 0, payload.Length);
            socketArgs.Completed += (sender, e) =>
            {
                if (e.SocketError != SocketError.Success)
                    throw new Exception($"Error occured: {e.SocketError.ToString()}");
                _waiter.Set();
            };
            _waiter.Reset();
            _socket.SendAsync(socketArgs);
            _waiter.WaitOne((int) Timeout.TotalMilliseconds);
        }

        /// <summary>
        ///     n バイト受信します。
        /// </summary>
        /// <param name="n">-1 を指定すると、すべて読み取ります。</param>
        /// <returns></returns>
        public byte[] ReceiveAsync(int n)
        {
            if (n == -1)
                return ReceiveAllAsync();
            var bytes = new byte[n];
            var socketArgs = new SocketAsyncEventArgs {RemoteEndPoint = new DnsEndPoint(_uri.Host, _uri.Port)};
            socketArgs.SetBuffer(bytes, 0, n);
            socketArgs.Completed += (sender, e) =>
            {
                if (e.SocketError != SocketError.Success)
                    throw new Exception($"Error occured: {e.SocketError.ToString()}");
                for (var i = 0; i < n; i++)
                    bytes[i] = e.Buffer[i];
                _waiter.Set();
            };
            _waiter.Reset();
            _socket.ReceiveAsync(socketArgs);
            _waiter.WaitOne((int) Timeout.TotalMilliseconds);

            return bytes;
        }

        private byte[] ReceiveAllAsync()
        {
            var bytes = new List<byte>();
            var socketArgs = new SocketAsyncEventArgs {RemoteEndPoint = new DnsEndPoint(_uri.Host, _uri.Port)};
            socketArgs.Completed += (sender, e) =>
            {
                if (e.SocketError != SocketError.Success)
                    throw new Exception($"Error occured: {e.SocketError.ToString()}");
                bytes.AddRange(e.Buffer);
                _waiter.Set();
            };
            _waiter.Reset();
            _socket.ReceiveAsync(socketArgs);
            _waiter.WaitOne((int) Timeout.TotalMilliseconds);

            return bytes.ToArray();
        }
    }
}