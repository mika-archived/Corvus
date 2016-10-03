using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class DataMessage : ProtocolMessage
    {
        public ReadOnlyCollection<AmfData> Data { get; set; }

        public DataMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            var type = Reader.MessageHeader.MessageTypeId;
            if (type == (byte) MessageType.DataMessage3)
                throw new NotSupportedException("AMF3 Data Message is not suppored.");

            Data = AmfDecoder.Decode(Reader.Body.ToArray()).AsReadOnly();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}