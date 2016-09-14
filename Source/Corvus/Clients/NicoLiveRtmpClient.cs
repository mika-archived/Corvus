using System;

namespace Corvus.Clients
{
    public class NicoLiveRtmpClient : RtmpClient
    {
        private readonly string _nlPlaypath;

        /// <summary>
        /// </summary>
        /// <param name="rtmpUri">有効な RTMP URI</param>
        /// <param name="nlPlaypath">ニコニコ生放送 Playpath</param>
        public NicoLiveRtmpClient(Uri rtmpUri, string nlPlaypath) : base(rtmpUri)
        {
            _nlPlaypath = nlPlaypath;
        }
    }
}