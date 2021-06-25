using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.SqlserverDB
{
    /// <summary>
    /// Sqlserver 执行命令参数
    /// </summary>
    public class SqlDbCmdParm
    {
        /// <summary>
        /// 命令类型
        /// </summary>
        public CommandType CmdType { get; set; } = CommandType.Text;

        /// <summary>
        /// 执行sql语句或存储过程名称
        /// </summary>
        public string CmdText { get; set; }

        /// <summary>
        /// sql命令参数列表
        /// </summary>
        private SortedList<string, SqlParameter> _sqlParms;
        /// <summary>
        /// sql命令参数列表
        /// </summary>
        public SortedList<string, SqlParameter> SqlParms 
        {
            get
            {
                if(_sqlParms == null)
                {
                    _sqlParms = new SortedList<string, SqlParameter>();
                }
                return _sqlParms;
            }
        }
    }
}
