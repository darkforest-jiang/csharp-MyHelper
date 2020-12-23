using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketHelper
{
    public class SocketMessage
    {
        public uint Id { get; set; }
        public uint DataLen { get; set; }
        public byte[] Data { get; set; }
    }
}
