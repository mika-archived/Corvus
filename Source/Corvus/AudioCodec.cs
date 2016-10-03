// ReSharper disable InconsistentNaming

using System;

namespace Corvus
{
    [Flags]
    public enum AudioCodec
    {
        /// <summary>
        ///     無圧縮
        /// </summary>
        None = 0x0001,

        /// <summary>
        ///     ADPCM
        /// </summary>
        ADPCM = 0x0002,

        /// <summary>
        ///     MP3
        /// </summary>
        MP3 = 0x0004,

        /// <summary>
        ///     未使用
        /// </summary>
        LITEL = 0x0008,

        /// <summary>
        ///     未使用
        /// </summary>
        UNUSED = 0x0010,

        /// <summary>
        ///     NellyMouser 8kHz
        /// </summary>
        NellyMouser8 = 0x0020,

        /// <summary>
        ///     NellyMouser
        /// </summary>
        NellyMouser = 0x0040,

        /// <summary>
        ///     G711A
        /// </summary>
        G711A = 0x0080,

        /// <summary>
        ///     G711U
        /// </summary>
        G711U = 0x0100,

        /// <summary>
        ///     NellyMouser 16kHz
        /// </summary>
        NellyMouser16 = 0x0200,

        /// <summary>
        ///     AAC
        /// </summary>
        AAC = 0x0400,

        /// <summary>
        ///     Speex Audio
        /// </summary>
        SpeexAudio = 0x0800,

        /// <summary>
        ///     All
        /// </summary>
        All = 0x0FFF
    }
}