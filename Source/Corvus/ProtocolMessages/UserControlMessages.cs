using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class UserControlMessages : ProtocolMessage
    {
        public UserControlMessageType EventType { get; set; }

        /// <summary>
        ///     First value of User Control Message.
        /// </summary>
        public int Value1 { get; set; }

        /// <summary>
        ///     Second value of User Control Message.
        ///     This property is use SetBufferLength Event only.
        /// </summary>
        public int Value2 { get; set; }

        public UserControlMessages(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            EventType = BitConverter.ToInt16(Reader.Body.Take(2).Reverse().ToArray(), 0).ToUserControlMessageType();
            if (EventType == UserControlMessageType.Unknown)
                return;
            Value1 = BitConverter.ToInt32(Reader.Body.Skip(2).Take(4).Reverse().ToArray(), 0);
            if (EventType == UserControlMessageType.SetBufferLength)
                Value2 = BitConverter.ToInt32(Reader.Body.Skip(6).Take(4).Reverse().ToArray(), 0);

            // TODO: PingRequest / SetBufferEmpty send request of PingResponse / pause().
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}