using System;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Amf.v0;
using Corvus.Chunking;

namespace Corvus.Commands
{
    internal class ResultCommand : ReceiveCommand
    {
        public override string CommandName => "_result";

        public override int TransactionId
        {
            get { throw new NotSupportedException(); }
        }

        public ResultCommand(Packet packet) : base(packet) {}

        public override async Task Read()
        {
            var reader = new ChunkReader(Packet);
            await reader.Read();

            var amfData = AmfDecoder.Decode(reader.Body.ToArray());
            var result = amfData[0] as AmfData<string>;
            if (result?.Value != "_result")
            {
                // _error()
                var errorCommand = new ErrorCommand();
                errorCommand.CastTo(amfData);
                throw new RtmpCommandErrorException(errorCommand, "Previous command returned _error().");
            }
        }
    }
}