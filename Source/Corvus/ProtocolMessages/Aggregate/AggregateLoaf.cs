using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Corvus.ProtocolMessages.Aggregate
{
    internal class AggregateLoaf
    {
        public AggregateHeader Header { get; set; }

        public ReadOnlyCollection<byte> Data { get; set; }

        public int PreviousTagSize { get; set; }

        public AggregateLoaf(BinaryReader reader)
        {
            Header = new AggregateHeader(reader);
            Data = reader.ReadBytes(Header.Size).ToList().AsReadOnly();
            PreviousTagSize = BitConverter.ToInt32(reader.ReadBytes(4).Reverse().ToArray(), 0);
        }
    }
}