namespace Corvus.Chunking
{
    internal enum ChunkStreamId : byte
    {
        /// <summary>
        ///     Protocol Control Messages
        /// </summary>
        LowLevel = 2,

        /// <summary>
        ///     Action
        /// </summary>
        HighLevel = 3,

        ControlStream = 4,

        Video = 5,

        Audio = 6
    }
}