using System.Collections.ObjectModel;

using Corvus.Amf.v0;

namespace Corvus.Commands.NetConnection
{
    internal abstract class NetCommand : CommandBase
    {
        /// <summary>
        ///     Command information object which has the name-value pairs that encoding by AMF.
        /// </summary>
        protected ReadOnlyCollection<AmfData> CommandObject { get; set; }

        /// <summary>
        ///     Any optional information.
        /// </summary>
        public AmfData OptionalUserArgumets { get; set; }

        protected NetCommand(Packet packet) : base(packet) {}

        public abstract byte[] GetBytes();
    }
}