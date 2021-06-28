using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;

namespace DBHelper.MysqlDB
{
    public class MysqlDbOp : IDisposable
    {
        #region 数据库操作所需对象
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string _connStr = string.Empty;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected MySqlConnection _mysqlConn;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected MySqlCommand _mysqlCmd;
        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected MySqlCommand MysqlCmd
        {
            get
            {
                if (_mysqlCmd == null)
                {
                    _mysqlCmd = _mysqlConn.CreateCommand();
                }
                if (_mysqlCmd.Connection != _mysqlConn)
                {
                    _mysqlCmd.Connection = _mysqlConn;
                }
                return _mysqlCmd;
            }
        }

        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected MySqlDataAdapter _mysqlDa;
        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected MySqlDataAdapter MysqlDa
        {
            get
            {
                if (_mysqlDa == null)
                {
                    _mysqlDa = new MySqlDataAdapter(MysqlCmd);
                }
                if (_mysqlDa.SelectCommand != MysqlCmd)
                {
                    _mysqlDa.SelectCommand = MysqlCmd;
                }
                return _mysqlDa;
            }
        }

        /// <summary>
        /// Transact-SQL 事务
        /// </summary>
        protected MySqlTransaction _mysqlTrans;

        /// <summary>
        /// 数据集对象
        /// </summary>
        protected MySqlDataReader _mysqlDrs;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        public MysqlDbOp(string connStr)
        {
            _connStr = connStr;
            _mysqlConn = new MySqlConnection(_connStr);
        }

        #region 打开、关闭数据库、释放资源
        /// <summary>
        /// 打开数据库 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                if (_mysqlConn.State != ConnectionState.Open)
                {
                    _mysqlConn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 关闭数据库
        /// </summary>
        public void CloseDb()
        {
            try
            {
                if (_mysqlCmd != null)
                {
                    _mysqlCmd.Dispose();
                    _mysqlCmd = null;
                }
                if ((_mysqlConn != null) && (_mysqlConn.State != ConnectionState.Closed))
                {
                    _mysqlConn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 释放数据库资源
        /// </summary>
        public void DisposeDb()
        {
            try
            {
                CloseDb();
                _mysqlConn.Dispose();
                _mysqlConn = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 实现接口IDisposable方法释放资源
        /// </summary>
        void IDisposable.Dispose()
        {
            DisposeDb();
        }
        #endregion

        #region 开启、提交、回滚事务
        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTrans()
        {
            try
            {
                if (_mysqlConn.State != ConnectionState.Open)
                {
                    _mysqlConn.Open();
                }
                _mysqlTrans = _mysqlConn.BeginTransaction();
                MysqlCmd.Transaction = _mysqlTrans;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void CommitTrans()
        {
            try
            {
                _mysqlTrans.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void RollbackTrans()
        {
            try
            {
                _mysqlTrans.Rollback();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 执行sql语句
        /// <summary>
        /// 执行命令参数 
        /// </summary>
        private MysqlDbCmdParm _dbCmdParm;
        /// <summary>
        /// 执行命令参数
        /// </summary>
        public MysqlDbCmdParm DbCmdParm
        {
            get
            {
                if (_dbCmdParm == null)
                {
                    _dbCmdParm = new MysqlDbCmdParm();
                }
                return _dbCmdParm;
            }
            set
            {
                _dbCmdParm = value;
            }
        }

        /// <summary>
        /// 创建Sql命令
        /// </summary>
        /// <returns></returns>
        private bool CreateSqlCmd()
        {
            if (string.IsNullOrEmpty(DbCmdParm.CmdText))
            {
                return false;
            }
            MysqlCmd.CommandType = DbCmdParm.CmdType;
            MysqlCmd.CommandText = DbCmdParm.CmdText;
            MysqlCmd.Parameters.Clear();
            foreach (var item in DbCmdParm.MysqlParms.Values)
            {
                MysqlCmd.Parameters.Add(item);
            }
            return true;
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <returns></returns>
        public DataTable GetTable()
        {
            try
            {
                OpenDb();
                CreateSqlCmd();
                DataTable dt = new DataTable();
                MysqlDa.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                CloseDb();
            }
        }

        /// <summary>
        /// 执行命令返回受影响行数
        /// </summary>
        /// <returns></returns>
        public int ExecNonCmd()
        {
            try
            {
                OpenDb();
                CreateSqlCmd();
                int i = 0;
                i = MysqlCmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                CloseDb();
            }
        }

        /// <summary>
        /// 执行命令返回是否操作成功，执行完不关闭连接
        /// 适用于外部启用事务调用
        /// </summary>
        /// <returns></returns>
        public bool BExecNonCmd()
        {
            bool retBool = false;
            try
            {
                OpenDb();
                CreateSqlCmd();
                int i = 0;
                i = MysqlCmd.ExecuteNonQuery();
                retBool = true;
            }
            catch (Exception ex)
            {
                retBool = false;
                throw new Exception(ex.Message, ex);
            }

            return retBool;
        }

        #endregion
    }
}
