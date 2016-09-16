namespace Corvus
{
    internal class NetConnection
    {
        private readonly RtmpClient _rtmpClient;

        public NetConnection(RtmpClient rtmpClient)
        {
            _rtmpClient = rtmpClient;
        }

        /// <summary>
        ///     NetConnection.connect を行います。
        /// </summary>
        public void Connect() {}
    }
}