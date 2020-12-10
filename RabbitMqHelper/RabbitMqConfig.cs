using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMqHelper
{
    public class RabbitMqConfig
    {
        /// <summary>
        /// 主机名
        /// </summary>
        public string HostName { get; set; }

        public string UserName { get; set; }
        
        public string Pwd { get; set; }
    }
}
