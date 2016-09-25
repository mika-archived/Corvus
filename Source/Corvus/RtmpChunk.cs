using System;
using System.Collections.Generic;
using System.Linq;

namespace Corvus
{
    /// <summary>
    ///     RTMP chunking
    /// </summary>
    internal class RtmpChunk
    {
        private readonly byte[] _basicHeader;
        private readonly byte[] _chunkData;
        private readonly byte[] _extendedTimestamp = new byte[0];
        private readonly byte[] _messageHeader;

        public RtmpChunk(int fmt, int csid, byte msgTypeId, int msgStreamId, byte[] body)
        {
            int bytes;

            // Basic Header
            if (csid < 62)
                _basicHeader = new byte[1];
            else if ((62 <= csid) && (csid < 320))
                _basicHeader = new byte[2];
            else // 320 <= csid
                _basicHeader = new byte[3];

            // Message Header
            if (fmt == 0)
                bytes = 11; // fmt = 0, Type 0, 11 bytes.
            else if (fmt == 1)
                bytes = 7; // fmt = 1, Type 1, 7 bytes.
            else if (fmt == 2)
                bytes = 3; // fmt = 2, Type 2, 3 bytes.
            else if (fmt == 3)
                bytes = 0; // fmt = 3, Type 3, 0 byte.
            else
                throw new NotSupportedException($"{nameof(fmt)} is 0 <= {nameof(fmt)} <= 3.");
            _messageHeader = new byte[bytes];
            _chunkData = body;
            InitializeBasicHeader(fmt, csid);
            InitializeMessageHader(fmt, msgTypeId, msgStreamId);
        }

        private void InitializeBasicHeader(int fmt, int csid)
        {
            if (csid < 64)
                _basicHeader[0] = (byte) ((fmt << 6) + csid);
            else if ((62 <= csid) && (csid < 320))
            {
                _basicHeader[0] = (byte) (fmt << 6);
                _basicHeader[1] = (byte) (csid - 64);
            }
            else
            {
                _basicHeader[0] = (byte) ((fmt << 6) + 1);
                _basicHeader[1] = BitConverter.GetBytes(csid - 64)[0];
                _basicHeader[2] = BitConverter.GetBytes(csid - 64)[1];
            }
        }

        private void InitializeMessageHader(int fmt, byte msgTypeId, int msgStreamId)
        {
            if (fmt == 3)
                return;

            ArrayHelper.Fill(_messageHeader, 0x00, 3);
            var size = _basicHeader.Length + _messageHeader.Length + _chunkData.Length;
            if (fmt == 0)
            {
                ArrayHelper.Concat(_messageHeader, BitConverter.GetBytes(size).Take(3).Reverse().ToArray(), 3);
                _messageHeader[6] = msgTypeId;
                ArrayHelper.Concat(_messageHeader, BitConverter.GetBytes(msgStreamId), 7);
            }
            else if (fmt == 1)
            {
                ArrayHelper.Concat(_messageHeader, BitConverter.GetBytes(size).Take(3).Reverse().ToArray(), 3);
                _messageHeader[6] = msgTypeId;
            }
        }

        public byte[] GetBytes()
        {
            var list = new List<byte>();
            list.AddRange(_basicHeader);
            list.AddRange(_messageHeader);
            list.AddRange(_extendedTimestamp);
            list.AddRange(_chunkData);
            return list.ToArray();
        }
    }
}