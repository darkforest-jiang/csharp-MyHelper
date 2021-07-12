using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Models
{
    /// <summary>
    /// 用户Token信息
    /// </summary>
    public class UserToken
    {
        public string Token { get; set; }

        public DateTime Expiration { get; set; }
    }
}
