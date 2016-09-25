using System.Threading.Tasks;

namespace Corvus.Protocol
{
    /// <summary>
    ///     Protocol Control Messages
    /// </summary>
    internal abstract class Protocol
    {
        public abstract Task Read(Packet packet);
    }
}