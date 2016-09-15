using System;
using System.Diagnostics;
using System.Linq;

namespace Corvus
{
    internal static class RtmpDump
    {
        public static void DumpHex(byte[] bytes)
        {
            var s = BitConverter.ToString(bytes).Replace("-", " ").Split(' ').ToList();
            var chunkedStrings = s.Select((w, i) => new {Index = i, Value = w})
                                  .GroupBy(w => w.Index / 16)
                                  .Select(w => string.Join(" ", w.Select(v => v.Value)))
                                  .ToList();
            foreach (var result in chunkedStrings.Select((w, i) => new {Index = i, Value = w}))
                Debug.WriteLine(string.Format("{0:D3}", result.Index) + ":" + result.Value);
        }
    }
}