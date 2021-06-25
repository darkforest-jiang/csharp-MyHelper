using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    /// <summary>
    /// 数据库操作工厂类
    /// </summary>
    public class DbOpFactory
    {
        /// <summary>
        /// sqlserver数据库连接字符串模板
        /// </summary>
        private const string SqlDbConnStrTemplet = "Data Source={0};user id={1};password={2};initial catalog={3};Connection Timeout=60;Max Pool Size=300";

        /// <summary>
        /// Oracle数据库连接字符串
        /// </summary>
        private const string OrclDbConnStrTemplet = "Data Source={0};User Id={1};Password={2};";
    }
}
