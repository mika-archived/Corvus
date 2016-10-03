using System;

namespace Corvus.Clients
{
    public class DefaultRtmpClient : RtmpClient
    {
        /// <summary>
        /// </summary>
        /// <param name="rtmpUri">RTMP URI</param>
        public DefaultRtmpClient(Uri rtmpUri) : base(rtmpUri) {}
    }
}