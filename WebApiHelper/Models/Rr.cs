using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Models
{
    /// <summary>
    /// 请求结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Rr<T>
    {
        /// <summary>
        /// 返回操作编码：1-成功  0-失败
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 返回的消息提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 返回的数据
        /// </summary>
        public T Data { get; set; }
    }
}
