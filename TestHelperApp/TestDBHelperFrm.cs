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

namespace TestHelperApp
{
    public partial class TestDBHelper : Form
    {
        public TestDBHelper()
        {
            InitializeComponent();
        }

        private SqlDbHelper sqldb = new SqlDbHelper("Data Source=.;user id=sa;password=sa;initial catalog=MyDb;Connection Timeout=60;Max Pool Size=300");

        private void btn_sql_1queryno_Click(object sender, EventArgs e)
        {
            string sql = "select * from tmy_user";
            DataTable dt;
            SortedList<string, SqlParameter> sqlParms = new SortedList<string, SqlParameter>();
            if(cb_parm.Checked)
            {
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
    }

    public class TMy_User
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public int Enabled { get; set; }
    }
}
