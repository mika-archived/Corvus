using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Chunking;
using Corvus.ProtocolMessages.Aggregate;

namespace Corvus.ProtocolMessages
{
    internal class AggregateMessage : ProtocolMessage
    {
        public ReadOnlyCollection<AggregateLoaf> Loaves { get; set; }

        public AggregateMessage(Packet packet, ChunkReader reader) : base(packet, reader) {}

        public override void Read()
        {
            // Format
            // [Header 0] [Body 0] [Back Pointer 0] ...
            // Header ......... [Tag (1)] [Size (3)] [Timestamp (3)] [Timestamp Extended (1)] [Stream ID (3)]
            // Body ........... [Body (Variable)]
            // Back Pointer ... [Tag Size (4)]
            var list = new List<AggregateLoaf>();
            using (var ms = new MemoryStream(Reader.Body.ToArray()))
            {
                using (var br = new BinaryReader(ms))
                {
                    while (br.PeekChar() != -1)
                    {
                        var loaf = new AggregateLoaf(br);
                        list.Add(loaf);
                    }
                }
            }
            Loaves = list.AsReadOnly();
        }

        public override Task Write()
        {
            throw new NotImplementedException();
        }
    }
}