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
    /// Sqlserver数据库操作
    /// </summary>
    public class SqlDbOp : IDisposable
    {
        #region 数据库操作所需对象
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string _connStr = string.Empty; 

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected SqlConnection _sqlConn;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected SqlCommand _sqlCmd;
        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected SqlCommand SqlCmd
        {
            get
            {
                if(_sqlCmd == null)
                {
                    _sqlCmd = _sqlConn.CreateCommand();
                }
                if(_sqlCmd.Connection != _sqlConn)
                {
                    _sqlCmd.Connection = _sqlConn;
                }
                return _sqlCmd;
            }
        }

        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected SqlDataAdapter _sqlDa;
        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected SqlDataAdapter SqlDa
        {
            get
            {
                if(_sqlDa == null)
                {
                    _sqlDa = new SqlDataAdapter(SqlCmd);
                }
                if(_sqlDa.SelectCommand != SqlCmd)
                {
                    _sqlDa.SelectCommand = SqlCmd;
                }
                return _sqlDa;
            }
        }

        /// <summary>
        /// Transact-SQL 事务
        /// </summary>
        protected SqlTransaction _sqlTrans;

        /// <summary>
        /// 数据集对象
        /// </summary>
        protected SqlDataReader _sqlDrs;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStr">数据库连接字符串</param>
        public SqlDbOp(string connStr)
        {
            _connStr = connStr;
            _sqlConn = new SqlConnection(_connStr);
        }

        #region 打开、关闭数据库、释放资源
        /// <summary>
        /// 打开数据库 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                if (_sqlConn.State != ConnectionState.Open)
                {
                    _sqlConn.Open();
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
                if (_sqlCmd != null)
                {
                    _sqlCmd.Dispose();
                    _sqlCmd = null;
                }
                if ((_sqlConn != null) && (_sqlConn.State != ConnectionState.Closed))
                {
                    _sqlConn.Close();
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
                _sqlConn.Dispose();
                _sqlConn = null;
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
                if(_sqlConn.State != ConnectionState.Open)
                {
                    _sqlConn.Open();
                }
                _sqlTrans = _sqlConn.BeginTransaction();
                SqlCmd.Transaction = _sqlTrans;
            }catch(Exception ex)
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
                _sqlTrans.Commit();
            }catch(Exception ex)
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
                _sqlTrans.Rollback();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region 执行sql语句
        /// <summary>
        /// 执行命令参数 
        /// </summary>
        private SqlDbCmdParm _dbCmdParm;
        /// <summary>
        /// 执行命令参数
        /// </summary>
        public SqlDbCmdParm DbCmdParm
        {
            get
            {
                if(_dbCmdParm == null)
                {
                    _dbCmdParm = new SqlDbCmdParm();
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
            if(string.IsNullOrEmpty(DbCmdParm.CmdText))
            {
                return false;
            }
            SqlCmd.CommandType = DbCmdParm.CmdType;
            SqlCmd.CommandText = DbCmdParm.CmdText;
            SqlCmd.Parameters.Clear();
            foreach(var item in DbCmdParm.SqlParms.Values)
            {
                SqlCmd.Parameters.Add(item);
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
                SqlDa.Fill(dt);
                return dt;
            }catch(Exception ex)
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
                i = SqlCmd.ExecuteNonQuery();
                return i;
            }catch(Exception ex)
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
                i = SqlCmd.ExecuteNonQuery();
                retBool = true;
            }
            catch(Exception ex)
            {
                retBool = false;
                throw new Exception(ex.Message, ex);
            }

            return retBool;
        }

        #endregion

    }
}
