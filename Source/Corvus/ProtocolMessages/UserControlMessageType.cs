// ReSharper disable InconsistentNaming

using System.Diagnostics;

namespace Corvus.ProtocolMessages
{
    internal enum UserControlMessageType
    {
        StreamBegin = 0,

        StreamEOF = 1,

        StreamDry = 2,

        SetBufferLength = 3,

        StreamIsRecorded = 4,

        PingRequest = 6,

        PingResponse = 7,

        /// <summary>
        ///     This User Control Message Type is not specified in official documentation, but FMS 3.5 servers sent this command.
        ///     Corvus use the implementation of rtmpdump/librtmp as a referemce.
        /// </summary>
        SetBufferEmpty = 31,

        /// <summary>
        ///     This User Control Message Type is not specified in official documentation, but FMS 3.5 servers sent this command.
        ///     Corvus use the implementation of rtmpdump/librtmp as a referemce.
        /// </summary>
        SetBufferReady = 32,

        Unknown
    }

    internal static class UserControlMessageTypeExt
    {
        public static UserControlMessageType ToUserControlMessageType(this short obj)
        {
            switch (obj)
            {
                case 0:
                    return UserControlMessageType.StreamBegin;

                case 1:
                    return UserControlMessageType.StreamEOF;

                case 2:
                    return UserControlMessageType.StreamDry;

                case 3:
                    return UserControlMessageType.SetBufferLength;

                case 4:
                    return UserControlMessageType.StreamIsRecorded;

                case 6:
                    return UserControlMessageType.PingRequest;

                case 7:
                    return UserControlMessageType.PingResponse;

                case 31:
                    return UserControlMessageType.SetBufferEmpty;

                case 32:
                    return UserControlMessageType.SetBufferReady;

                default:
                    Debug.WriteLine($"WARN: RTMP server sent unknown message type ({obj}).");
                    return UserControlMessageType.Unknown;
            }
        }
    }
}