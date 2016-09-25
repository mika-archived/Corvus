namespace Corvus.Commands
{
    internal interface ICommand
    {
        // XML document command based RTMP secification "7.2.1.1 connect".
        /// <summary>
        ///     Name of the command.
        /// </summary>
        string CommandName { get; }

        /// <summary>
        ///     Transcation ID.
        /// </summary>
        int TransactionId { get; }
    }
}