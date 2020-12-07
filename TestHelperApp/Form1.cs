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
            MyHelper.EncryptHelper.RSAEncryptHelper en = new MyHelper.EncryptHelper.RSAEncryptHelper(textBox3.Text, textBox4.Text);
            textBox2.Text = en.GenerateSign(textBox1.Text);
        }

        //1nJys3HiJrMUT0ggrV4XTw==
        private void button2_Click(object sender, EventArgs e)
        {
            MyHelper.EncryptHelper.RSAEncryptHelper en = new MyHelper.EncryptHelper.RSAEncryptHelper(textBox3.Text, textBox4.Text);
            textBox1.Text = en.VerifySign(textBox1.Text, textBox2.Text).ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string privatekey;
            string publickey;
            MyHelper.EncryptHelper.RSAEncryptHelper.GenerateKeys(out privatekey, out publickey);
            textBox3.Text = privatekey;
            textBox4.Text = publickey;
        }
    }
}
