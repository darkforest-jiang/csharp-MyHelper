using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketHelper
{
    public class DataPack
    {
        public uint GetHeadLen()
        {
            return 8;
        }

        public byte[] Pack(SocketMessage msg)
        {
            byte[] data = new byte[GetHeadLen() + msg.DataLen];
            BitConverter.GetBytes(msg.Id).CopyTo(data, 0);
            BitConverter.GetBytes(msg.DataLen).CopyTo(data, 4);
            msg.Data.CopyTo(data, 8);
            return data;
        }

        public SocketMessage UnPack(byte[] data)
        {
            SocketMessage msg = new SocketMessage();
            msg.Id = BitConverter.ToUInt32(data, 0);
            msg.DataLen = BitConverter.ToUInt32(data, 4);
            return msg;
        }
    }
}
