using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestHelperAppCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        RabbitMqHelper.RabbitMqHelper rabbitMqHelper = new RabbitMqHelper.RabbitMqHelper(new RabbitMqHelper.RabbitMqConfig()
        {
            HostName = "localhost",
            UserName = "guest",
            Pwd = "guest"
        });

        private void button1_Click(object sender, EventArgs e)
        {
            //rabbitMqHelper.Publish(new RabbitMqHelper.ExchangeConfig() { 
            //}, "1");
            //rabbitMqHelper.Publish("HelloQ", "2");
            //rabbitMqHelper.Publish("HelloQ", "3");
        }

        //1nJys3HiJrMUT0ggrV4XTw==
        private void button2_Click(object sender, EventArgs e)
        {
            rabbitMqHelper.ReceiveMsg("HelloQ", haha);
        }

        private void haha(string msg)
        {                                           
            richTextBox1.Invoke(new MethodInvoker(delegate ()
            {
                richTextBox1.AppendText(msg + "\n");
            }));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string msg;
            if(rabbitMqHelper.ReceiveOneMsg("HelloQ", out msg))
            {
                richTextBox1.AppendText(msg + "\n");
            }
            else
            {
                richTextBox1.AppendText("无数据" + "\n");
            }
        }
    }
}
