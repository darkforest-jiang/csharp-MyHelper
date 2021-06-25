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
            sqldb.TGetTable(CommandType.Text, sql, ref sqlParms, out dt);
            dgv_result.DataSource = null;
            dgv_result.DataSource = dt;
        }

        private void btn_1queryyes_Click(object sender, EventArgs e)
        {
            string sql = "select * from tmy_user where usercode=@usercode";
            DataTable dt;
            SortedList<string, SqlParameter> sqlParms = new SortedList<string, SqlParameter>();
            sqlParms.Add("usercode", new SqlParameter("@usercode", SqlDbType.VarChar));
            sqlParms["usercode"].Value = "admin";
            sqldb.TGetTable(CommandType.Text, sql, ref sqlParms, out dt);
            dgv_result.DataSource = null;
            dgv_result.DataSource = dt;
        }
    }
}
