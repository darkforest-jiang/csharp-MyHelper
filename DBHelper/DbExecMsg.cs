using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    /// <summary>
    /// 数据库操作执行信息
    /// </summary>
    public class DbExecMsg
    {
        /// <summary>
        /// 是否执行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误类型：true : 业务逻辑产生的错误   false:程序异常错误
        /// </summary>
        public bool ErrType { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public Exception ErrMsg { get; set; }
    }

    /// <summary>
    /// 数据库操作执行信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbExecMsg<T> : DbExecMsg
    {
        /// <summary>
        /// 结果信息
        /// </summary>
        public T ResultInfo { get; set; }
    }

}
