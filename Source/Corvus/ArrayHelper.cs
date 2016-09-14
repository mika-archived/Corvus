using System.Linq;

namespace Corvus
{
    internal static class ArrayHelper
    {
        /// <summary>
        ///     array1 に対して array2 の内容を追加します。
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        public static void Concat(byte[] array1, byte[] array2) => Concat(array1, array2, 0);

        /// <summary>
        ///     array1 に対して array2 を offset の位置から追加します。
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <param name="offset"></param>
        public static void Concat(byte[] array1, byte[] array2, int offset)
        {
            for (int i = offset, j = 0; j < array2.Length; i++, j++)
                array1[i] = array2[j];
        }

        /// <summary>
        ///     array に対して b を count 個追加します。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="b"></param>
        /// <param name="count"></param>
        public static void Fill(byte[] array, byte b, int count) => Fill(array, b, count, 0);

        /// <summary>
        ///     array に対して b を count 個、 offset の位置から追加します。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="b"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        public static void Fill(byte[] array, byte b, int count, int offset)
        {
            for (int i = offset, j = 0; j < count; i++, j++)
                array[i] = b;
        }

        /// <summary>
        ///     array から count 個、 offset の位置から要素を取り出します。
        /// </summary>
        /// <param name="array"></param>
        /// <param name="count"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static byte[] Take(byte[] array, int count, int offset)
        {
            return array.Skip(offset).Take(count).ToArray();
        }
    }
}