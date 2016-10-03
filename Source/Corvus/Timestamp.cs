using System;
using System.Linq;

namespace Corvus
{
    internal static class Timestamp
    {
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

        /// <summary>
        ///     タイムスタンプを取得します。
        /// </summary>
        /// <returns>タイムスタンプ 4byte 値</returns>
        public static byte[] GetNow()
        {
            var unix = (long) (DateTime.Now - UnixEpoch).TotalSeconds;
            return BitConverter.GetBytes(unix).Take(4).ToArray();
        }
    }
}