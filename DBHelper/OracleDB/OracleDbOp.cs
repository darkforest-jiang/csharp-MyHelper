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
    /// Oracle数据库操作
    /// </summary>
    public class OracleDbOp : IDisposable
    {
        #region 数据库连接对象
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected string _connStr = string.Empty;

        /// <summary>
        /// 数据库连接对象
        /// </summary>
        protected OracleConnection _oracleConn;

        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected OracleCommand _oracleCmd;
        /// <summary>
        /// 数据库命令对象
        /// </summary>
        protected OracleCommand OracleCmd
        {
            get
            {
                if(_oracleCmd == null)
                {
                    _oracleCmd = _oracleConn.CreateCommand();
                }
                if(_oracleCmd.Connection != _oracleConn)
                {
                    _oracleCmd.Connection = _oracleConn;
                }
                return _oracleCmd;
            }
        }

        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected OracleDataAdapter _oracleDa;
        /// <summary>
        /// 数据库命令和连接对象
        /// </summary>
        protected OracleDataAdapter OracleDa 
        {
            get
            {
                if(_oracleDa == null)
                {
                    _oracleDa = new OracleDataAdapter();
                }
                if(_oracleDa.SelectCommand != OracleCmd)
                {
                    _oracleDa.SelectCommand = OracleCmd;
                }
                return _oracleDa;
            }
        }

        /// <summary>
        /// 数据库事务
        /// </summary>
        protected OracleTransaction _oracleTrans;

        /// <summary>
        /// 数据集对象
        /// </summary>
        protected OracleDataReader _oracleDrs;
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connStr"></param>
        public OracleDbOp(string connStr)
        {
            _connStr = connStr;
            _oracleConn = new OracleConnection(_connStr);
        }

        #region 打开、关闭数据库、释放资源
        /// <summary>
        /// 打开数据库 
        /// </summary>
        public void OpenDb()
        {
            try
            {
                if (_oracleConn.State != ConnectionState.Open)
                {
                    _oracleConn.Open();
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
                if (_oracleCmd != null)
                {
                    _oracleCmd.Dispose();
                    _oracleCmd = null;
                }
                if ((_oracleConn != null) && (_oracleConn.State != ConnectionState.Closed))
                {
                    _oracleConn.Close();
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
                _oracleConn.Dispose();
                _oracleConn = null;
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
                if (_oracleConn.State != ConnectionState.Open)
                {
                    _oracleConn.Open();
                }
                _oracleTrans = _oracleConn.BeginTransaction();
                OracleCmd.Transaction = _oracleTrans;
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
                _oracleTrans.Commit();
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
                _oracleTrans.Rollback();
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
        private OracleDbCmdParm _dbCmdParm;
        /// <summary>
        /// 执行命令参数
        /// </summary>
        public OracleDbCmdParm DbCmdParm
        {
            get
            {
                if (_dbCmdParm == null)
                {
                    _dbCmdParm = new OracleDbCmdParm();
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
        private bool CreateOracleCmd()
        {
            if (string.IsNullOrEmpty(DbCmdParm.CmdText))
            {
                return false;
            }
            OracleCmd.CommandType = DbCmdParm.CmdType;
            OracleCmd.CommandText = DbCmdParm.CmdText;
            OracleCmd.Parameters.Clear();
            foreach (var item in DbCmdParm.OracleParms.Values)
            {
                OracleCmd.Parameters.Add(item);
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
                CreateOracleCmd();
                DataTable dt = new DataTable();
                OracleDa.Fill(dt);
                return dt;
                //或者下边这种方式用dataset
                //DataSet ds = new DataSet();
                //DataTable tmpDate = new DataTable();
                //OracleDa.Fill(ds);
                //if (ds.Tables.Count > 0)
                //    return ds.Tables[0];
                //else
                //    return tmpDate;
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
                CreateOracleCmd();
                int i = 0;
                i = OracleCmd.ExecuteNonQuery();
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
                CreateOracleCmd();
                int i = 0;
                i = OracleCmd.ExecuteNonQuery();
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
