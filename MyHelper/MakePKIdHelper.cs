using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snowflake.Core;

namespace MyHelper
{
    /// <summary>
    /// 生成主键ID帮助类
    /// </summary>
    public class MakePKIdHelper
    {
        /// <summary>
        /// 只读对象
        /// </summary>
        private static readonly object obj = new object();

        /// <summary>
        /// 主键IDWorker
        /// </summary>
        private static Dictionary<string, IdWorker> Dic_IdWoker = new Dictionary<string, IdWorker>();

        /// <summary>
        /// 获取主键ID
        /// </summary>
        /// <param name="tableName">表名</param>
        public static string GetPKNewID(string tableName)
        {
            try
            {
                lock (obj)
                {
                    if (!Dic_IdWoker.ContainsKey(tableName))
                    {
                        Dic_IdWoker.Add(tableName, new IdWorker(1, 1));
                    }
                    string id = Dic_IdWoker[tableName].NextId().ToString();

                    return id;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
