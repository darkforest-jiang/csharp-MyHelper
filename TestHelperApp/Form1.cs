using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHelperApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MyHelper.EncryptHelper.DESEncryptHelper en = new MyHelper.EncryptHelper.DESEncryptHelper(textBox3.Text, textBox4.Text);
            //textBox2.Text = en.Encrypt(textBox1.Text);
            textBox2.Text = MyHelper.EncryptHelper.MD5EncryptHelper.Encrypt(textBox1.Text);
        }

        //1nJys3HiJrMUT0ggrV4XTw==
        private void button2_Click(object sender, EventArgs e)
        {
            MyHelper.EncryptHelper.DESEncryptHelper en = new MyHelper.EncryptHelper.DESEncryptHelper(textBox3.Text, textBox4.Text);
            textBox1.Text = en.Decrypt(textBox2.Text);
        }
    }
}
