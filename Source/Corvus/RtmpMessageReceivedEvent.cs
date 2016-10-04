using System;

using Corvus.ProtocolMessages;

namespace Corvus
{
    public class RtmpMessageReceivedEvent : EventArgs
    {
        public MessageType MessageType { get; }

        public ProtocolMessage Message { get; }

        internal RtmpMessageReceivedEvent(MessageType messageType, ProtocolMessage message)
        {
            MessageType = messageType;
            Message = message;
        }
    }
}