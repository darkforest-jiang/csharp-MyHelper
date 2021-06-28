using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DBHelper.MysqlDB
{
    /// <summary>
    /// Mysql数据库操作参数
    /// </summary>
    public class MysqlDbCmdParm
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
        private SortedList<string, MySqlParameter> _mysqlParms;
        /// <summary>
        /// sql命令参数列表
        /// </summary>
        public SortedList<string, MySqlParameter> MysqlParms
        {
            get
            {
                if (_mysqlParms == null)
                {
                    _mysqlParms = new SortedList<string, MySqlParameter>();
                }
                return _mysqlParms;
            }
        }
    }
}
