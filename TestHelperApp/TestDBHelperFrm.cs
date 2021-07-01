using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DBHelper;
using DBHelper.SqlserverDB;
using DBHelper.MysqlDB;
using MySqlConnector;

namespace TestHelperApp
{
    public partial class TestDBHelper : Form
    {
        public TestDBHelper()
        {
            InitializeComponent();
        }

        private SqlDbHelper sqldb = new SqlDbHelper("Data Source=.;user id=sa;password=sa;initial catalog=MyDb;Connection Timeout=60;Max Pool Size=300");

        #region sqlServer
        private void btn_sql_1queryno_Click(object sender, EventArgs e)
        {
            string sql = "select * from tmy_user";
            DataTable dt;
            SortedList<string, SqlParameter> sqlParms = new SortedList<string, SqlParameter>();
            if(cb_parm.Checked)
            {
                sql = "select * from tmy_user where usercode=@usercode";
                sqlParms.Add("usercode", new SqlParameter("@usercode", SqlDbType.VarChar));
                sqlParms["usercode"].Value = "admin";
            }
            if(cb_bingdinglist.Checked)
            {
                var ds = new BindingList<TMy_User>();
                sqldb.TLGetInfo<TMy_User>(CommandType.Text, sql, ref sqlParms, out ds);
                dgv_result.DataSource = null;
                dgv_result.DataSource = ds;
            }
            else
            {
                sqldb.TGetTable(CommandType.Text, sql, ref sqlParms, out dt);
                dgv_result.DataSource = null;
                dgv_result.DataSource = dt;
            }
        }

        private void btn_procqueryno_Click(object sender, EventArgs e)
        {
            string sql = "queryuser";
            DataTable dt;
            SortedList<string, SqlParameter> sqlParms = new SortedList<string, SqlParameter>();
            if (cb_parm.Checked)
            {
                sqlParms.Add("UserCode", new SqlParameter("@UserCode", SqlDbType.VarChar));
                sqlParms["UserCode"].Value = "admin1";
            }
            if (cb_bingdinglist.Checked == false)
            {
                sqldb.TGetTable(CommandType.StoredProcedure, sql, ref sqlParms, out dt);
                dgv_result.DataSource = null;
                dgv_result.DataSource = dt;
            }
            else
            {
                var ds = new BindingList<TMy_User>();
                sqldb.TLGetInfo<TMy_User>(CommandType.StoredProcedure, sql, ref sqlParms, out ds);
                dgv_result.DataSource = null;
                dgv_result.DataSource = ds;
            }
        }

        private void btn_execproc_Click(object sender, EventArgs e)
        {
            string sql = "updateuser";
            DataTable dt;
            SortedList<string, SqlParameter> sqlParms = new SortedList<string, SqlParameter>();
            sqlParms.Add("UserCode", new SqlParameter("UserCode", SqlDbType.VarChar));
            sqlParms["UserCode"].Value = "admin1";
            sqlParms.Add("UserName", new SqlParameter("UserName", SqlDbType.VarChar));
            sqlParms["UserName"].Value = "哈哈";
            sqlParms.Add("Msg", new SqlParameter("Msg", SqlDbType.VarChar, 100));
            sqlParms["Msg"].Direction = ParameterDirection.Output;

            var result = sqldb.TOpData(CommandType.StoredProcedure, sql, ref sqlParms);
            if(result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }

        private void btn_sql2same_Click(object sender, EventArgs e)
        {
            string sql = "";
            sql = "insert TMy_User values(@UserId, @UserCode, @UserName, @Enabled)";
            List<SortedList<string, SqlParameter>> sqlParms = new List<SortedList<string, SqlParameter>>();
            SortedList<string, SqlParameter> sqlp1 = new SortedList<string, SqlParameter>();
            sqlp1.Add("UserId", new SqlParameter("UserId", SqlDbType.VarChar));
            sqlp1["UserId"].Value = "3";
            sqlp1.Add("UserCode", new SqlParameter("UserCode", SqlDbType.VarChar));
            sqlp1["UserCode"].Value = "dark";
            sqlp1.Add("UserName", new SqlParameter("UserName", SqlDbType.VarChar));
            sqlp1["UserName"].Value = "黑夜";
            sqlp1.Add("Enabled", new SqlParameter("Enabled", SqlDbType.Int));
            sqlp1["Enabled"].Value = 1;
            sqlParms.Add(sqlp1);

            SortedList<string, SqlParameter> sqlp2 = new SortedList<string, SqlParameter>();
            sqlp2.Add("UserId", new SqlParameter("UserId", SqlDbType.VarChar));
            sqlp2["UserId"].Value = "4";
            sqlp2.Add("UserCode", new SqlParameter("UserCode", SqlDbType.VarChar));
            sqlp2["UserCode"].Value = "forest";
            sqlp2.Add("UserName", new SqlParameter("UserName", SqlDbType.VarChar));
            sqlp2["UserName"].Value = "森林";
            sqlp2.Add("Enabled", new SqlParameter("Enabled", SqlDbType.Int));
            sqlp2["Enabled"].Value = 1;
            sqlParms.Add(sqlp2);

            var result = sqldb.TOpListData(CommandType.Text, sql, ref sqlParms);
            if(result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }

        private void btn_sql2no_Click(object sender, EventArgs e)
        {
            List<string> listsql = new List<string>();
            List<SortedList<string, SqlParameter>> sqlParms = new List<SortedList<string, SqlParameter>>();
            if (cb_parm.Checked)
            {
                string sql1 = "update TMy_User set UserCode=@UserCode where UserId=@UserId";
                listsql.Add(sql1);
                SortedList<string, SqlParameter> sqlp1 = new SortedList<string, SqlParameter>();
                sqlp1.Add("UserId", new SqlParameter("UserId", SqlDbType.VarChar));
                sqlp1["UserId"].Value = "0";
                sqlp1.Add("UserCode", new SqlParameter("UserCode", SqlDbType.VarChar));
                sqlp1["UserCode"].Value = "000000";
                sqlParms.Add(sqlp1);

                string sql2 = "update TMy_User set UserCode=@UserCode where UserId=@UserId";
                listsql.Add(sql2);
                SortedList<string, SqlParameter> sqlp2 = new SortedList<string, SqlParameter>();
                sqlp2.Add("UserId", new SqlParameter("UserId", SqlDbType.VarChar));
                sqlp2["UserId"].Value = "3";
                sqlp2.Add("UserCode", new SqlParameter("UserCode", SqlDbType.VarChar));
                sqlp2["UserCode"].Value = "333333";
                sqlParms.Add(sqlp2);
            }
            else
            {
                string sql1 = "update TMy_User set UserName='000000' where UserId=0";
                string sql2 = "update TMy_User set UserName='森林氧吧' where UserId=4";
                listsql.Add(sql1);
                listsql.Add(sql2);
            }

            var result = sqldb.TOpListData(CommandType.Text, listsql, ref sqlParms);
            if (result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }
        #endregion

        private MysqlDbHelper mysqlDb = new MysqlDbHelper("Server=localhost;Database=mydb;Uid=root;Pwd=root");
        private void btn_mysql_query_Click(object sender, EventArgs e)
        {
            string sql = "select * from tmy_user";
            DataTable dt;
            SortedList<string, MySqlParameter> sqlParms = new SortedList<string, MySqlParameter>();
            if (cb_parm.Checked)
            {
                sql = "select * from tmy_user where usercode=@usercode";
                sqlParms.Add("usercode", new MySqlParameter("@usercode", SqlDbType.VarChar));
                sqlParms["usercode"].Value = "admin";
            }
            if (cb_bingdinglist.Checked)
            {
                var ds = new BindingList<TMy_User>();
                mysqlDb.TLGetInfo<TMy_User>(CommandType.Text, sql, ref sqlParms, out ds);
                dgv_result.DataSource = null;
                dgv_result.DataSource = ds;
            }
            else
            {
                mysqlDb.TGetTable(CommandType.Text, sql, ref sqlParms, out dt);
                dgv_result.DataSource = null;
                dgv_result.DataSource = dt;
            }
        }

        private void btn_mysql_procquery_Click(object sender, EventArgs e)
        {
            string sql = "queryuser";
            DataTable dt;
            //mysql 存储过程有参数 必须传参数 参数名不能和表中字段相同
            SortedList<string, MySqlParameter> sqlParms = new SortedList<string, MySqlParameter>();
            sqlParms.Add("UserCode0", new MySqlParameter("UserCode0", MySqlDbType.VarChar));
            sqlParms["UserCode0"].Value = "";
            if (cb_bingdinglist.Checked == false)
            {
                var result = mysqlDb.TGetTable(CommandType.StoredProcedure, sql, ref sqlParms, out dt);
                dgv_result.DataSource = null;
                dgv_result.DataSource = dt;
            }
            else
            {
                var ds = new BindingList<TMy_User>();
                var result = mysqlDb.TLGetInfo<TMy_User>(CommandType.StoredProcedure, sql, ref sqlParms, out ds);
                dgv_result.DataSource = null;
                dgv_result.DataSource = ds;
            }
        }

        private void btn_mysql_proc_Click(object sender, EventArgs e)
        {
            string sql = "updateuser";
            DataTable dt;
            SortedList<string, MySqlParameter> sqlParms = new SortedList<string, MySqlParameter>();
            sqlParms.Add("UserCode0", new MySqlParameter("UserCode0", MySqlDbType.VarChar));
            sqlParms["UserCode0"].Value = "admin1";
            sqlParms.Add("UserName0", new MySqlParameter("UserName0", MySqlDbType.VarChar));
            sqlParms["UserName0"].Value = "哈哈超级管理员";
            sqlParms.Add("Msg", new MySqlParameter("Msg", MySqlDbType.VarChar, 50));
            sqlParms["Msg"].Direction = ParameterDirection.Output;

            var result = mysqlDb.TOpData(CommandType.StoredProcedure, sql, ref sqlParms);
            if (result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }

        private void btn_mysql_same_Click(object sender, EventArgs e)
        {
            string sql = "";
            // 参数名前加 @ 或者 ?
            //mysql 参数名  不能与 表字段相同 否则不执行where语句的条件
            sql = "insert TMy_User values(@pUserId, @pUserCode, @pUserName, @pEnabled)";
            List<SortedList<string, MySqlParameter>> sqlParms = new List<SortedList<string, MySqlParameter>>();
            SortedList<string, MySqlParameter> sqlp1 = new SortedList<string, MySqlParameter>();
            sqlp1.Add("pUserId", new MySqlParameter("pUserId", MySqlDbType.VarChar));
            sqlp1["pUserId"].Value = "3";
            sqlp1.Add("pUserCode", new MySqlParameter("pUserCode", MySqlDbType.VarChar));
            sqlp1["pUserCode"].Value = "dark";
            sqlp1.Add("pUserName", new MySqlParameter("pUserName", MySqlDbType.VarChar));
            sqlp1["pUserName"].Value = "黑夜";
            sqlp1.Add("pEnabled", new MySqlParameter("pEnabled", MySqlDbType.Int32));
            sqlp1["pEnabled"].Value = 1;
            sqlParms.Add(sqlp1);

            SortedList<string, MySqlParameter> sqlp2 = new SortedList<string, MySqlParameter>();
            sqlp2.Add("pUserId", new MySqlParameter("pUserId", MySqlDbType.VarChar));
            sqlp2["pUserId"].Value = "4";
            sqlp2.Add("pUserCode", new MySqlParameter("pUserCode", MySqlDbType.VarChar));
            sqlp2["pUserCode"].Value = "forest";
            sqlp2.Add("pUserName", new MySqlParameter("pUserName", MySqlDbType.VarChar));
            sqlp2["pUserName"].Value = "森林";
            sqlp2.Add("pEnabled", new MySqlParameter("pEnabled", MySqlDbType.Int32));
            sqlp2["pEnabled"].Value = 1;
            sqlParms.Add(sqlp2);

            var result = mysqlDb.TOpListData(CommandType.Text, sql, ref sqlParms);
            if (result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }

        private void btn_mysql_nosame_Click(object sender, EventArgs e)
        {
            List<string> listsql = new List<string>();
            List<SortedList<string, MySqlParameter>> sqlParms = new List<SortedList<string, MySqlParameter>>();
            if (cb_parm.Checked)
            {
                string sql1 = "update TMy_User set UserCode=@pUserCode where UserId=@pUserId";
                listsql.Add(sql1);
                SortedList<string, MySqlParameter> sqlp1 = new SortedList<string, MySqlParameter>();
                sqlp1.Add("pUserId", new MySqlParameter("pUserId", MySqlDbType.VarChar));
                sqlp1["pUserId"].Value = "0";
                sqlp1.Add("pUserCode", new MySqlParameter("pUserCode", MySqlDbType.VarChar));
                sqlp1["pUserCode"].Value = "000000";
                sqlParms.Add(sqlp1);

                string sql2 = "update TMy_User set UserCode=@pUserCode where UserId=@pUserId";
                listsql.Add(sql2);
                SortedList<string, MySqlParameter> sqlp2 = new SortedList<string, MySqlParameter>();
                sqlp2.Add("pUserId", new MySqlParameter("pUserId", MySqlDbType.VarChar));
                sqlp2["pUserId"].Value = "3";
                sqlp2.Add("pUserCode", new MySqlParameter("pUserCode", MySqlDbType.VarChar));
                sqlp2["pUserCode"].Value = "333333";
                sqlParms.Add(sqlp2);
            }
            else
            {
                string sql1 = "update TMy_User set UserName='000000' where UserId=0";
                string sql2 = "update TMy_User set UserName='森林氧吧' where UserId=4";
                listsql.Add(sql1);
                listsql.Add(sql2);
            }

            var result = mysqlDb.TOpListData(CommandType.Text, listsql, ref sqlParms);
            if (result.IsSuccess)
            {
                MessageBox.Show("OK");
            }
            else
            {
                MessageBox.Show($"error:{result.ErrMsg.Message}");
            }
        }
    }

    public class TMy_User
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int Enabled { get; set; }
    }
}
