using System;

namespace Corvus.Clients
{
    public class DefaultRtmpClient : RtmpClient
    {
        /// <summary>
        /// </summary>
        /// <param name="rtmpUri">有効な RTMP URI</param>
        public DefaultRtmpClient(Uri rtmpUri) : base(rtmpUri) {}
    }
}