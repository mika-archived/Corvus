using System;
using System.Collections.Generic;
using System.Linq;

namespace Corvus
{
    internal static class BitHelper
    {
        /// <summary>
        ///     3 bit Big Endian を int に変換します。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int ToInt32(IEnumerable<byte> bytes)
        {
            var a = new byte[1];
            var b = a.Concat(bytes).ToArray();
            return BitConverter.ToInt32(b.Reverse().ToArray(), 0);
        }
    }
}