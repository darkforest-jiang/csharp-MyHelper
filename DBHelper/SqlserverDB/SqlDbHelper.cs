using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.SqlserverDB
{
    /// <summary>
    /// Sqlserver 数据库操作帮助类
    /// </summary>
    public class SqlDbHelper : DbHelperBase
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnStr;

        /// <summary>
        /// 约定的数据库返回参数
        /// 如果不为空 则说明执行有业务逻辑错误
        /// </summary>
        public string CmdExecErrFlag = "Msg";

        public SqlDbHelper()
        {

        }

        public SqlDbHelper(string connStr)
        {
            ConnStr = connStr;
        }

        #region 通用获取数据

        /// <summary>
        /// 通用获取DataTable
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <param name="dt">返回数据表</param>
        /// <returns></returns>
        public DbExecMsg TGetTable(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms, out DataTable dt)
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;
            dt = null;

            using (SqlDbOp dbOp = new SqlDbOp(ConnStr))
            {
                dbOp.DbCmdParm.CmdType = cmdType;
                dbOp.DbCmdParm.CmdText = cmdText;
                foreach(var pnItem in sqlParms.Keys)
                {
                    dbOp.DbCmdParm.SqlParms.Add(pnItem, sqlParms[pnItem]);
                }

                try
                {
                    dt = dbOp.GetTable();
                    dt.TableName = "Table";
                    //判断是否有业务逻辑错误
                    foreach(var pnItem in dbOp.DbCmdParm.SqlParms.Keys)
                    {
                        if(sqlParms.ContainsKey(pnItem) && dbOp.DbCmdParm.SqlParms[pnItem].Direction == ParameterDirection.Output)
                        {
                            if(pnItem == CmdExecErrFlag)
                            {
                                if(!string.IsNullOrEmpty(sqlParms[pnItem].Value.ToString().Trim()))
                                {
                                    execMsg.IsSuccess = false;
                                    execMsg.ErrType = true;
                                    execMsg.ErrMsg = new Exception(sqlParms[pnItem].Value.ToString());
                                    break;
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    execMsg.IsSuccess = false;
                    execMsg.ErrMsg = ex;
                }
            }
            
            return execMsg;
        }

        /// <summary>
        /// 通用获取数据表对象(只返回第一行数据)
        /// </summary>
        /// <typeparam name="T">返回对象类型</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <param name="tInfo">返回对象</param>
        /// <returns></returns>
        public DbExecMsg TLGetInfo<T>(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms, out T tInfo) where T : new ()
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;
            tInfo = new T();

            try
            {
                DataTable dt;
                execMsg = TGetTable(cmdType, cmdText, ref sqlParms, out dt);
                if(execMsg.IsSuccess)
                {
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        if(!DataRowToInfoObj(dr, tInfo))
                        {
                            execMsg.IsSuccess = false;
                            execMsg.ErrMsg = new Exception("转换对象异常");
                        }
                    }
                }
            }catch(Exception ex)
            {
                execMsg.IsSuccess = false;
                execMsg.ErrMsg = ex;
            }

            return execMsg;
        }

        /// <summary>
        /// 通用获取数据表List
        /// </summary>
        /// <typeparam name="T">返回数据对象</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <param name="tInfo">返回数据库List</param>
        /// <returns></returns>
        public DbExecMsg TLGetInfo<T>(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms, out List<T> tInfo) where T : new()
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;
            tInfo = new List<T>();

            try
            {
                DataTable dt;
                execMsg = TGetTable(cmdType, cmdText, ref sqlParms, out dt);
                if(execMsg.IsSuccess)
                {
                    if(dt != null && dt.Rows.Count > 0)
                    {
                        for(int i = 0; i < dt.Rows.Count; i ++)
                        {
                            DataRow dr = dt.Rows[i];
                            T tmp = new T();
                            if(DataRowToInfoObj(dr, tmp))
                            {
                                tInfo.Add(tmp);
                            }
                            else
                            {
                                execMsg.IsSuccess = false;
                                execMsg.ErrMsg = new Exception("转换对象异常");
                                break;
                            }
                        }
                    }
                }
            }catch(Exception ex)
            {
                execMsg.IsSuccess = false;
                execMsg.ErrMsg = ex;
            }

            return execMsg;
        }

        /// <summary>
        /// 通用获取数据表BindingList
        /// </summary>
        /// <typeparam name="T">返回数据对象</typeparam>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <param name="tInfo">返回数据库BindingList</param>
        /// <returns></returns>
        public DbExecMsg TLGetInfo<T>(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms, out BindingList<T> tInfo) where T : new()
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;
            tInfo = new BindingList<T>();

            try
            {
                DataTable dt;
                execMsg = TGetTable(cmdType, cmdText, ref sqlParms, out dt);
                if (execMsg.IsSuccess)
                {
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow dr = dt.Rows[i];
                            T tmp = new T();
                            if (DataRowToInfoObj(dr, tmp))
                            {
                                tInfo.Add(tmp);
                            }
                            else
                            {
                                execMsg.IsSuccess = false;
                                execMsg.ErrMsg = new Exception("转换对象异常");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                execMsg.IsSuccess = false;
                execMsg.ErrMsg = ex;
            }

            return execMsg;
        }

        #endregion

        #region 通用更新或删除数据

        /// <summary>
        /// 通用更新或删除数据
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <returns></returns>
        public DbExecMsg TOpData(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms)
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;

            using (SqlDbOp dbOp = new SqlDbOp(ConnStr))
            {
                dbOp.DbCmdParm.CmdType = cmdType;
                dbOp.DbCmdParm.CmdText = cmdText;
                foreach (var pnItem in sqlParms.Keys)
                {
                    dbOp.DbCmdParm.SqlParms.Add(pnItem, sqlParms[pnItem]);
                }

                try
                {
                    dbOp.ExecNonCmd();
                    foreach (var pnItem in dbOp.DbCmdParm.SqlParms.Keys)
                    {
                        if (sqlParms.ContainsKey(pnItem) && dbOp.DbCmdParm.SqlParms[pnItem].Direction == ParameterDirection.Output)
                        {
                            if (!string.IsNullOrEmpty(sqlParms[pnItem].Value.ToString().Trim()))
                            {
                                execMsg.IsSuccess = false;
                                execMsg.ErrType = true;
                                execMsg.ErrMsg = new Exception(sqlParms[pnItem].Value.ToString());
                                break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    execMsg.IsSuccess = false;
                    execMsg.ErrMsg = ex;
                }
            }

            return execMsg;
        }

        /// <summary>
        /// 通用更新或删除数据
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程名称</param>
        /// <param name="sqlParms">参数</param>
        /// <param name="opRowCount">返回受影响行数</param>
        /// <returns></returns>
        public DbExecMsg TOpData(CommandType cmdType, string cmdText, ref SortedList<string, SqlParameter> sqlParms, out int opRowCount)
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;
            opRowCount = 0;

            using (SqlDbOp dbOp = new SqlDbOp(ConnStr))
            {
                dbOp.DbCmdParm.CmdType = cmdType;
                dbOp.DbCmdParm.CmdText = cmdText;
                foreach(var pnItem in sqlParms.Keys)
                {
                    dbOp.DbCmdParm.SqlParms.Add(pnItem, sqlParms[pnItem]);
                }

                try
                {
                    opRowCount = dbOp.ExecNonCmd();
                    foreach(var pnItem in dbOp.DbCmdParm.SqlParms.Keys)
                    {
                        if(sqlParms.ContainsKey(pnItem) && dbOp.DbCmdParm.SqlParms[pnItem].Direction == ParameterDirection.Output)
                        {
                            if(!string.IsNullOrEmpty(sqlParms[pnItem].Value.ToString().Trim()))
                            {
                                execMsg.IsSuccess = false;
                                execMsg.ErrType = true;
                                execMsg.ErrMsg = new Exception(sqlParms[pnItem].Value.ToString());
                                opRowCount = -1;
                                break;
                            }
                        }
                    }
                }catch(Exception ex)
                {
                    execMsg.IsSuccess = false;
                    execMsg.ErrMsg = ex;
                }
            }

            return execMsg;
        }

        /// <summary>
        /// 通用更新或删除数据(相同的sql语句)
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程</param>
        /// <param name="sqlParms">参数集合</param>
        /// <returns></returns>
        public DbExecMsg TOpListData(CommandType cmdType, string cmdText, ref List<SortedList<string, SqlParameter>> sqlParms)
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;

            using (SqlDbOp dbOp = new SqlDbOp(ConnStr))
            {
                try
                {
                    bool isOpera = false;
                    dbOp.BeginTrans();
                    dbOp.DbCmdParm.CmdType = cmdType;
                    foreach(var spItems in sqlParms)
                    {
                        dbOp.DbCmdParm.CmdText = cmdText;
                        dbOp.DbCmdParm.SqlParms.Clear();
                        foreach(var pnItem in spItems.Keys)
                        {
                            dbOp.DbCmdParm.SqlParms.Add(pnItem, spItems[pnItem]);
                        }

                        if(dbOp.BExecNonCmd())
                        {
                            isOpera = true;
                        }
                        else
                        {
                            isOpera = false;
                            break;
                        }
                    }
                    if(isOpera)
                    {
                        dbOp.CommitTrans();
                    }
                    else
                    {
                        dbOp.RollbackTrans();
                    }
                    dbOp.CloseDb();
                }catch(Exception ex)
                {
                    execMsg.IsSuccess = false;
                    execMsg.ErrMsg = ex;
                }
            }

            return execMsg;
        }

        /// <summary>
        /// 通用更新或删除数据(不同的sql语句)
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">sql语句或存储过程</param>
        /// <param name="sqlParms">参数集合</param>
        /// <returns></returns>
        public DbExecMsg TOpListData(CommandType cmdType, List<string> cmdText, ref List<SortedList<string, SqlParameter>> sqlParms)
        {
            DbExecMsg execMsg = new DbExecMsg();
            execMsg.IsSuccess = true;
            execMsg.ErrType = false;

            using (SqlDbOp dbOp = new SqlDbOp(ConnStr))
            {
                try
                {
                    bool isOpera = false;
                    dbOp.BeginTrans();
                    dbOp.DbCmdParm.CmdType = cmdType;
                    int index = 0;
                    foreach (var sqlItem in cmdText)
                    {
                        dbOp.DbCmdParm.CmdText = sqlItem;
                        dbOp.DbCmdParm.SqlParms.Clear();
                        foreach (var pnItem in sqlParms[index].Keys)
                        {
                            dbOp.DbCmdParm.SqlParms.Add(pnItem, sqlParms[index][pnItem]);
                        }

                        if (dbOp.BExecNonCmd())
                        {
                            isOpera = true;
                        }
                        else
                        {
                            isOpera = false;
                            break;
                        }

                        index++;
                    }
                    if (isOpera)
                    {
                        dbOp.CommitTrans();
                    }
                    else
                    {
                        dbOp.RollbackTrans();
                    }
                    dbOp.CloseDb();
                }
                catch (Exception ex)
                {
                    execMsg.IsSuccess = false;
                    execMsg.ErrMsg = ex;
                }
            }

            return execMsg;
        }

        #endregion

    }
}
