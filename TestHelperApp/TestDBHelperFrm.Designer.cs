
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
            this.btn_sql_1queryno = new System.Windows.Forms.Button();
            this.rb_oracle = new System.Windows.Forms.RadioButton();
            this.rb_mysql = new System.Windows.Forms.RadioButton();
            this.rb_sqlserver = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_1queryyes = new System.Windows.Forms.Button();
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
            // btn_sql_1queryno
            // 
            this.btn_sql_1queryno.Location = new System.Drawing.Point(163, 42);
            this.btn_sql_1queryno.Name = "btn_sql_1queryno";
            this.btn_sql_1queryno.Size = new System.Drawing.Size(136, 23);
            this.btn_sql_1queryno.TabIndex = 6;
            this.btn_sql_1queryno.Text = "单条查询(不带参数)";
            this.btn_sql_1queryno.UseVisualStyleBackColor = true;
            this.btn_sql_1queryno.Click += new System.EventHandler(this.btn_sql_1queryno_Click);
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
            // btn_1queryyes
            // 
            this.btn_1queryyes.Location = new System.Drawing.Point(305, 42);
            this.btn_1queryyes.Name = "btn_1queryyes";
            this.btn_1queryyes.Size = new System.Drawing.Size(134, 23);
            this.btn_1queryyes.TabIndex = 7;
            this.btn_1queryyes.Text = "单条查询(带参数)";
            this.btn_1queryyes.UseVisualStyleBackColor = true;
            this.btn_1queryyes.Click += new System.EventHandler(this.btn_1queryyes_Click);
            // 
            // TestDBHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_1queryyes);
            this.Controls.Add(this.btn_sql_1queryno);
            this.Controls.Add(this.dgv_result);
            this.Controls.Add(this.groupBox1);
            this.Name = "TestDBHelper";
            this.Text = "TestDBHelper";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_result)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_result;
        private System.Windows.Forms.Button btn_sql_1queryno;
        private System.Windows.Forms.RadioButton rb_oracle;
        private System.Windows.Forms.RadioButton rb_mysql;
        private System.Windows.Forms.RadioButton rb_sqlserver;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_1queryyes;
    }
}