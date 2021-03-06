﻿
namespace TestHelperApp
{
    partial class TestDBHelper
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgv_result = new System.Windows.Forms.DataGridView();
            this.btn_sqlquery = new System.Windows.Forms.Button();
            this.rb_oracle = new System.Windows.Forms.RadioButton();
            this.rb_mysql = new System.Windows.Forms.RadioButton();
            this.rb_sqlserver = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_bingdinglist = new System.Windows.Forms.CheckBox();
            this.btn_procquery = new System.Windows.Forms.Button();
            this.cb_parm = new System.Windows.Forms.CheckBox();
            this.btn_execproc = new System.Windows.Forms.Button();
            this.btn_sql2same = new System.Windows.Forms.Button();
            this.btn_sql2no = new System.Windows.Forms.Button();
            this.btn_mysql_query = new System.Windows.Forms.Button();
            this.btn_mysql_procquery = new System.Windows.Forms.Button();
            this.btn_mysql_proc = new System.Windows.Forms.Button();
            this.btn_mysql_same = new System.Windows.Forms.Button();
            this.btn_mysql_nosame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_result
            // 
            this.dgv_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_result.Location = new System.Drawing.Point(39, 199);
            this.dgv_result.Name = "dgv_result";
            this.dgv_result.RowTemplate.Height = 25;
            this.dgv_result.Size = new System.Drawing.Size(736, 239);
            this.dgv_result.TabIndex = 5;
            // 
            // btn_sqlquery
            // 
            this.btn_sqlquery.Location = new System.Drawing.Point(163, 42);
            this.btn_sqlquery.Name = "btn_sqlquery";
            this.btn_sqlquery.Size = new System.Drawing.Size(136, 23);
            this.btn_sqlquery.TabIndex = 6;
            this.btn_sqlquery.Text = "sql查询";
            this.btn_sqlquery.UseVisualStyleBackColor = true;
            this.btn_sqlquery.Click += new System.EventHandler(this.btn_sql_1queryno_Click);
            // 
            // rb_oracle
            // 
            this.rb_oracle.AutoSize = true;
            this.rb_oracle.Location = new System.Drawing.Point(20, 78);
            this.rb_oracle.Name = "rb_oracle";
            this.rb_oracle.Size = new System.Drawing.Size(62, 21);
            this.rb_oracle.TabIndex = 1;
            this.rb_oracle.Text = "oracle";
            this.rb_oracle.UseVisualStyleBackColor = true;
            // 
            // rb_mysql
            // 
            this.rb_mysql.AutoSize = true;
            this.rb_mysql.Location = new System.Drawing.Point(20, 116);
            this.rb_mysql.Name = "rb_mysql";
            this.rb_mysql.Size = new System.Drawing.Size(60, 21);
            this.rb_mysql.TabIndex = 2;
            this.rb_mysql.Text = "mysql";
            this.rb_mysql.UseVisualStyleBackColor = true;
            // 
            // rb_sqlserver
            // 
            this.rb_sqlserver.AutoSize = true;
            this.rb_sqlserver.Checked = true;
            this.rb_sqlserver.Location = new System.Drawing.Point(20, 36);
            this.rb_sqlserver.Name = "rb_sqlserver";
            this.rb_sqlserver.Size = new System.Drawing.Size(79, 21);
            this.rb_sqlserver.TabIndex = 0;
            this.rb_sqlserver.TabStop = true;
            this.rb_sqlserver.Text = "sqlserver";
            this.rb_sqlserver.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rb_sqlserver);
            this.groupBox1.Controls.Add(this.rb_mysql);
            this.groupBox1.Controls.Add(this.rb_oracle);
            this.groupBox1.Location = new System.Drawing.Point(39, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(118, 162);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据库";
            // 
            // cb_bingdinglist
            // 
            this.cb_bingdinglist.AutoSize = true;
            this.cb_bingdinglist.Location = new System.Drawing.Point(305, 12);
            this.cb_bingdinglist.Name = "cb_bingdinglist";
            this.cb_bingdinglist.Size = new System.Drawing.Size(98, 21);
            this.cb_bingdinglist.TabIndex = 8;
            this.cb_bingdinglist.Text = "BingdingList";
            this.cb_bingdinglist.UseVisualStyleBackColor = true;
            // 
            // btn_procquery
            // 
            this.btn_procquery.Location = new System.Drawing.Point(318, 42);
            this.btn_procquery.Name = "btn_procquery";
            this.btn_procquery.Size = new System.Drawing.Size(134, 23);
            this.btn_procquery.TabIndex = 9;
            this.btn_procquery.Text = "proc查询";
            this.btn_procquery.UseVisualStyleBackColor = true;
            this.btn_procquery.Click += new System.EventHandler(this.btn_procqueryno_Click);
            // 
            // cb_parm
            // 
            this.cb_parm.AutoSize = true;
            this.cb_parm.Location = new System.Drawing.Point(178, 12);
            this.cb_parm.Name = "cb_parm";
            this.cb_parm.Size = new System.Drawing.Size(82, 21);
            this.cb_parm.TabIndex = 10;
            this.cb_parm.Text = "userParm";
            this.cb_parm.UseVisualStyleBackColor = true;
            // 
            // btn_execproc
            // 
            this.btn_execproc.Location = new System.Drawing.Point(475, 42);
            this.btn_execproc.Name = "btn_execproc";
            this.btn_execproc.Size = new System.Drawing.Size(59, 23);
            this.btn_execproc.TabIndex = 11;
            this.btn_execproc.Text = "proc";
            this.btn_execproc.UseVisualStyleBackColor = true;
            this.btn_execproc.Click += new System.EventHandler(this.btn_execproc_Click);
            // 
            // btn_sql2same
            // 
            this.btn_sql2same.Location = new System.Drawing.Point(555, 42);
            this.btn_sql2same.Name = "btn_sql2same";
            this.btn_sql2same.Size = new System.Drawing.Size(75, 23);
            this.btn_sql2same.TabIndex = 12;
            this.btn_sql2same.Text = "多条same";
            this.btn_sql2same.UseVisualStyleBackColor = true;
            this.btn_sql2same.Click += new System.EventHandler(this.btn_sql2same_Click);
            // 
            // btn_sql2no
            // 
            this.btn_sql2no.Location = new System.Drawing.Point(636, 42);
            this.btn_sql2no.Name = "btn_sql2no";
            this.btn_sql2no.Size = new System.Drawing.Size(139, 23);
            this.btn_sql2no.TabIndex = 13;
            this.btn_sql2no.Text = "多条nosame";
            this.btn_sql2no.UseVisualStyleBackColor = true;
            this.btn_sql2no.Click += new System.EventHandler(this.btn_sql2no_Click);
            // 
            // btn_mysql_query
            // 
            this.btn_mysql_query.Location = new System.Drawing.Point(163, 100);
            this.btn_mysql_query.Name = "btn_mysql_query";
            this.btn_mysql_query.Size = new System.Drawing.Size(136, 23);
            this.btn_mysql_query.TabIndex = 14;
            this.btn_mysql_query.Text = "sql查询";
            this.btn_mysql_query.UseVisualStyleBackColor = true;
            this.btn_mysql_query.Click += new System.EventHandler(this.btn_mysql_query_Click);
            // 
            // btn_mysql_procquery
            // 
            this.btn_mysql_procquery.Location = new System.Drawing.Point(318, 100);
            this.btn_mysql_procquery.Name = "btn_mysql_procquery";
            this.btn_mysql_procquery.Size = new System.Drawing.Size(123, 23);
            this.btn_mysql_procquery.TabIndex = 15;
            this.btn_mysql_procquery.Text = "proc查询";
            this.btn_mysql_procquery.UseVisualStyleBackColor = true;
            this.btn_mysql_procquery.Click += new System.EventHandler(this.btn_mysql_procquery_Click);
            // 
            // btn_mysql_proc
            // 
            this.btn_mysql_proc.Location = new System.Drawing.Point(475, 100);
            this.btn_mysql_proc.Name = "btn_mysql_proc";
            this.btn_mysql_proc.Size = new System.Drawing.Size(59, 23);
            this.btn_mysql_proc.TabIndex = 16;
            this.btn_mysql_proc.Text = "proc";
            this.btn_mysql_proc.UseVisualStyleBackColor = true;
            this.btn_mysql_proc.Click += new System.EventHandler(this.btn_mysql_proc_Click);
            // 
            // btn_mysql_same
            // 
            this.btn_mysql_same.Location = new System.Drawing.Point(555, 99);
            this.btn_mysql_same.Name = "btn_mysql_same";
            this.btn_mysql_same.Size = new System.Drawing.Size(75, 23);
            this.btn_mysql_same.TabIndex = 17;
            this.btn_mysql_same.Text = "多条same";
            this.btn_mysql_same.UseVisualStyleBackColor = true;
            this.btn_mysql_same.Click += new System.EventHandler(this.btn_mysql_same_Click);
            // 
            // btn_mysql_nosame
            // 
            this.btn_mysql_nosame.Location = new System.Drawing.Point(636, 100);
            this.btn_mysql_nosame.Name = "btn_mysql_nosame";
            this.btn_mysql_nosame.Size = new System.Drawing.Size(139, 23);
            this.btn_mysql_nosame.TabIndex = 18;
            this.btn_mysql_nosame.Text = "多条nosame";
            this.btn_mysql_nosame.UseVisualStyleBackColor = true;
            this.btn_mysql_nosame.Click += new System.EventHandler(this.btn_mysql_nosame_Click);
            // 
            // TestDBHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_mysql_nosame);
            this.Controls.Add(this.btn_mysql_same);
            this.Controls.Add(this.btn_mysql_proc);
            this.Controls.Add(this.btn_mysql_procquery);
            this.Controls.Add(this.btn_mysql_query);
            this.Controls.Add(this.btn_sql2no);
            this.Controls.Add(this.btn_sql2same);
            this.Controls.Add(this.btn_execproc);
            this.Controls.Add(this.cb_parm);
            this.Controls.Add(this.btn_procquery);
            this.Controls.Add(this.cb_bingdinglist);
            this.Controls.Add(this.btn_sqlquery);
            this.Controls.Add(this.dgv_result);
            this.Controls.Add(this.groupBox1);
            this.Name = "TestDBHelper";
            this.Text = "TestDBHelper";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_result;
        private System.Windows.Forms.Button btn_sqlquery;
        private System.Windows.Forms.RadioButton rb_oracle;
        private System.Windows.Forms.RadioButton rb_mysql;
        private System.Windows.Forms.RadioButton rb_sqlserver;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox cb_bingdinglist;
        private System.Windows.Forms.Button btn_procquery;
        private System.Windows.Forms.CheckBox cb_parm;
        private System.Windows.Forms.Button btn_execproc;
        private System.Windows.Forms.Button btn_sql2same;
        private System.Windows.Forms.Button btn_sql2no;
        private System.Windows.Forms.Button btn_mysql_query;
        private System.Windows.Forms.Button btn_mysql_procquery;
        private System.Windows.Forms.Button btn_mysql_proc;
        private System.Windows.Forms.Button btn_mysql_same;
        private System.Windows.Forms.Button btn_mysql_nosame;
    }
}