using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Models
{
    /// <summary>
    /// jwt Token配置
    /// </summary>
    public class JwtToken
    {
        /// <summary>
        /// 是否验证签发者
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Token的颁发者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 是否验证受众
        /// </summary>
        public bool ValidateAudience { get; set; }

        /// <summary>
        /// 受众
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 是否验证失效时间
        /// </summary>
        public bool ValidateLifetime { get; set; }

        /// <summary>
        /// token有效持续时间间隔/s
        /// </summary>
        public int ValidTimeSpan { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        public string Secretkey { get; set; }
    }
}
