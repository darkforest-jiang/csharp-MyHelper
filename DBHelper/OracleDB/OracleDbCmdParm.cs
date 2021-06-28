using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.OracleDB
{
    /// <summary>
    /// Oracle数据库操作 参数
    /// </summary>
    public class OracleDbCmdParm
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
        private SortedList<string, OracleParameter> _oracleParms;
        /// <summary>
        /// sql命令参数列表
        /// </summary>
        public SortedList<string, OracleParameter> OracleParms
        {
            get
            {
                if (_oracleParms == null)
                {
                    _oracleParms = new SortedList<string, OracleParameter>();
                }
                return _oracleParms;
            }
        }
    }
}
