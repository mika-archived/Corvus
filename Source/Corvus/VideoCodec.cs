using System;

// ReSharper disable InconsistentNaming

namespace Corvus
{
    [Flags]
    public enum VideoCodec
    {
        /// <summary>
        ///     非推奨
        /// </summary>
        Unused = 0x0001,

        /// <summary>
        ///     非推奨
        /// </summary>
        JPEG = 0x0002,

        /// <summary>
        ///     Sorenson Flash Video
        /// </summary>
        Sorenson = 0x0004,

        /// <summary>
        ///     V1 screen sharing
        /// </summary>
        Homebrew = 0x0008,

        /// <summary>
        ///     On2 video
        /// </summary>
        VP6 = 0x0010,

        /// <summary>
        ///     On2 video with alpha channel
        /// </summary>
        VP6Alpha = 0x0020,

        /// <summary>
        ///     Screen sharing version 2
        /// </summary>
        HomebrewV = 0x0040,

        /// <summary>
        ///     H264
        /// </summary>
        H264 = 0x0080,

        /// <summary>
        ///     All
        /// </summary>
        All = 0x00FF
    }
}