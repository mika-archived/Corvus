namespace Corvus
{
    internal enum MessageType : byte
    {
        /// <summary>
        ///     Set Chunk Size (1)
        /// </summary>
        SetChunkSize = 0x01,

        /// <summary>
        ///     Abort Message (2)
        /// </summary>
        AbortMessage = 0x02,

        /// <summary>
        ///     Acknowledgement (3)
        /// </summary>
        Acknowledgement = 0x03,

        /// <summary>
        ///     User Control Messages (4)
        /// </summary>
        UserControlMessages = 0x04,

        /// <summary>
        ///     Window Acknowledgement Size (5)
        /// </summary>
        WindowAckSize = 0x05,

        /// <summary>
        ///     Set Peer Bandwidth (6)
        /// </summary>
        SetPeerBandwidth = 0x06,

        /// <summary>
        ///     Audio Message (8)
        /// </summary>
        AudioMessage = 0x08,

        /// <summary>
        ///     Video Message (9)
        /// </summary>
        VideoMessage = 0x09,

        /// <summary>
        ///     Data Message - AMF3 (15)
        /// </summary>
        DataMessage3 = 0x0F,

        /// <summary>
        ///     Shared Object Message - AMF3 (16)
        /// </summary>
        SharedObjectMsg3 = 0x10,

        /// <summary>
        ///     Command Message - AMF3 (17)
        /// </summary>
        CommandMessage3 = 0x11,

        /// <summary>
        ///     Data Message - AMF0 (18)
        /// </summary>
        DataMessage0 = 0x12,

        /// <summary>
        ///     Shared Object Message - AMF0 (19)
        /// </summary>
        SharedObjectMsg0 = 0x13,

        /// <summary>
        ///     Command Message - AMF0 (20)
        /// </summary>
        CommandMessage0 = 0x14,

        /// <summary>
        ///     Aggregate Message (22)
        /// </summary>
        AggregateMessage = 0x16
    }
}