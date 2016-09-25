using System.Collections.ObjectModel;

using Corvus.Amf.v0;

namespace Corvus.Commands.NetConnection
{
    internal interface INetCommand : ICommand
    {
        /// <summary>
        ///     Command information object which has the name-value pairs that encoding by AMF.
        /// </summary>
        ReadOnlyCollection<AmfData> CommandObject { get; }

        /// <summary>
        ///     Any optional information.
        /// </summary>
        AmfData OptionalUserArgumets { get; }

        byte[] GetBytes();
    }
}