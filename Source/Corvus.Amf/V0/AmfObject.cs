﻿using System.Collections.Generic;

namespace Corvus.Amf.V0
{
    public class AmfObject : AmfData<List<AmfProperty>>
    {
        public override AmfMarker Marker => AmfMarker.Object;

        public AmfObject()
        {
            Value = new List<AmfProperty>();
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte> {(byte) AmfMarker.Object};
            foreach (var value in Value)
                bytes.AddRange(value.GetBytes());
            bytes.AddRange(new AmfObjectEnd().GetBytes());
            return bytes.ToArray();
        }
    }
}