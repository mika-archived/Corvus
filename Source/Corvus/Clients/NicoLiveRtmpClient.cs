using System;

namespace Corvus.Clients
{
    public class NicoLiveRtmpClient : RtmpClient
    {
        private readonly string _nlPlaypath;

        /// <summary>
        /// </summary>
        /// <param name="rtmpUri">RTMP URI</param>
        /// <param name="nlPlaypath">Stream Contents</param>
        /// <param name="connection">Ticket</param>
        public NicoLiveRtmpClient(Uri rtmpUri, string nlPlaypath, string connection) : base(rtmpUri)
        {
            _nlPlaypath = nlPlaypath;
            var path = rtmpUri.AbsolutePath;
            App = path.Substring(1, path.IndexOf("lv", StringComparison.Ordinal) - 2);
            TcUrl = $"{rtmpUri.Scheme}://{rtmpUri.Host}/{App}";
            UserArguments = connection;
        }
    }
}