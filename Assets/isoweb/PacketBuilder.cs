using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;
using Debug = UnityEngine.Debug;

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

        public void PushHash(string hash)
        {
            var bytes = StringToByteArray(hash);
            if (bytes.Length != 8)
            {
                Debug.LogError("Hash too damn big!");
                return;
            }

            _packet.Add(bytes);
            _length += 8;
        }

        public void PushSmallString(string hash)
        {
            PushByte((byte)hash.Length);
            _packet.Add(Encoding.ASCII.GetBytes(hash));
            _length += 1 + hash.Length;
        }

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
    }
}
