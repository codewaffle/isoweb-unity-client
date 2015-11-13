// a bit of code duplication here but wrapping the count instead of using naked BitConverter is really nice.

using System;
using System.Linq;
using System.Text;
using isoweb.Entity;

namespace isoweb
{
    public class PacketReader
    {
        byte[] _data;
        int idx;
        int end;

        public void SetData(byte[] data)
        {
            _data = data.Reverse().ToArray();

            end = data.Length;
            idx = 0;
        }

        public PacketType ReadPacketType()
        {
            var pt = (PacketType)_data[end - ++idx];

            // Debug.Log("[" + idx + "] " + pt);

            return pt;
        }

        public float ReadFloat32()
        {
            idx += 4;
            return BitConverter.ToSingle(_data, end - idx);
        }

        public Entity.Entity ReadEntityId()
        {
            idx += 4;
            var entId = BitConverter.ToUInt32(_data, end - idx);

            return EntityManager.Get(entId);
        }

        public EntityDef ReadEntityDefId()
        {
            idx += 8;
            var hash = BitConverter.ToUInt64(_data, end - idx);

            return EntityManager.GetDef(hash);
        }

        public string ReadString()
        {
            idx += 2;
            var len = BitConverter.ToUInt16(_data, end - idx);
            idx += len;

            var bytes = _data.Skip(end - idx).Take(len).Reverse().ToArray();
            var str = Encoding.UTF8.GetString(bytes);

            return str;
        }

        public JSONNode ReadJsonObject()
        {
            var str = ReadString();

            return JSON.Parse(str);
        }

        public string ReadSmallString()
        {
            idx += 1;
            var len = (int) _data[end - idx];
            idx += len;
            var bytes = _data.Skip(end - idx).Take(len).Reverse().ToArray();
            var str = Encoding.UTF8.GetString(bytes);
            return str;
        }

        public ushort ReadUint16()
        {
            idx += 2;
            return BitConverter.ToUInt16(_data, end - idx);
        }

        public double ReadFloat64()
        {
            idx += 8;
            return BitConverter.ToDouble(_data, end - idx);
        }
    }
}
