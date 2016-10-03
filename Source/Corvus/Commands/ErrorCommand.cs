using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Corvus.Amf.v0;

namespace Corvus.Commands
{
    // ErrorCommand is created by ResultCommand
    internal sealed class ErrorCommand : ReceiveCommand
    {
        public string Code { get; private set; }
        public string Description { get; private set; }
        public string Reason { get; private set; }
        public override string CommandName => "_error";

        public override int TransactionId
        {
            get { throw new NotSupportedException(); }
        }

        public ErrorCommand() : base(null) {}

        public override Task Read()
        {
            throw new NotSupportedException();
        }

        public void CastTo(List<AmfData> amfData)
        {
            var command = amfData.First() as AmfData<string>;
            if (command?.Value != "_error")
                throw new NotSupportedException();

            var obj = amfData[4] as AmfObject;
            if (obj == null)
                throw new RtmpInvalidFormatException();
            Code = (string) obj.Value.Single(w => w.Name == "code").Value.Value;
            Description = (string) obj.Value.First(w => w.Name == "description").Value.Value;
            Reason = (string) obj.Value.Where(w => w.Name == "description").ElementAt(1).Value.Value;
        }

        public void CastTo(IEnumerable<byte> bytes)
        {
            var amfData = AmfDecoder.Decode(bytes.ToArray());
            CastTo(amfData);
        }
    }
}