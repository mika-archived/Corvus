using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Chunking;

namespace Corvus.ProtocolMessages
{
    internal class CommandMessage : ProtocolMessage
    {
        public ReadOnlyCollection<AmfData> Data { get; set; }

        public CommandMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            Data = AmfDecoder.Decode(Reader.Body.ToArray()).AsReadOnly();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}