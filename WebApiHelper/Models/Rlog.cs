using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Models
{
    /// <summary>
    /// 请求日志
    /// </summary>
    public class Rlog
    {
        /// <summary>
        /// 请求时间
        /// </summary>
        public string RequestTime { get; set; }

        /// <summary>
        /// 请求方法名称
        /// </summary>
        public string ActionName { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string RequestParms { get; set; }

        /// <summary>
        /// 响应时间
        /// </summary>
        public string ResponseTime { get; set; }

        /// <summary>
        /// 请求返回结果
        /// </summary>
        public string ResponseResult { get; set; }

    }
}
