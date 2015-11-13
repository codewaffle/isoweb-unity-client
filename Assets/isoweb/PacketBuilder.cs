using System;
using System.Collections.Generic;
using System.Linq;

namespace isoweb
{
    public class PacketBuilder
    {
        List<byte[]> _packet;
        private int _length;

        public PacketBuilder()
        {
            _packet = new List<byte[]>();
            _length = 0;
        }

        // gotta have separate methods for everything for safety... (byte)item casts to int if byte isn't defined. Easier to not have to think about it.
        public void PushByte(byte item)
        {
            var bytes = new []{item};
            _packet.Add(bytes);
            _length += bytes.Length;
        }

        public void PushUint16(ushort item)
        {
            var bytes = BitConverter.GetBytes(item).Reverse().ToArray();
            _packet.Add(bytes);
            _length += bytes.Length;
        }

        void Clear()
        {
            _packet.Clear();
            _length = 0;
        }

        public byte[] Build()
        {
            var res = new byte[_length];
            var idx = 0;

            foreach (var x in _packet)
            {
                x.CopyTo(res, idx);
                idx += x.Length;
            }

            Clear();

            return res;
        }

        public void PushFloat32(float f)
        {
            var bytes = BitConverter.GetBytes(f).Reverse().ToArray();
            _packet.Add(bytes);
            _length += bytes.Length;
        }

        public void PushPacketType(PacketType pType)
        {
            PushByte((byte)pType);
        }
    }
}
