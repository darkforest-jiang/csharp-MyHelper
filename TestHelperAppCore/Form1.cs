using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
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


        RabbitMqHelper.RabbitMqSdkHelper rmsdk0 = new RabbitMqHelper.RabbitMqSdkHelper(new RabbitMqHelper.RabbitMqConfig()
        {
            HostName = "localhost",
            UserName = "guest",
            Pwd = "guest"
        });

        RabbitMqHelper.RabbitMqSdkHelper rmsdk1 = new RabbitMqHelper.RabbitMqSdkHelper(new RabbitMqHelper.RabbitMqConfig()
        {
            HostName = "localhost",
            UserName = "guest",
            Pwd = "guest"
        });

        RabbitMqHelper.RabbitMqSdkHelper rmsdk2 = new RabbitMqHelper.RabbitMqSdkHelper(new RabbitMqHelper.RabbitMqConfig()
        {
            HostName = "localhost",
            UserName = "guest",
            Pwd = "guest"
        });

        private void button1_Click(object sender, EventArgs e)
        {
            RabbitMqHelper.ExchangeConfig exchangeConfig = new RabbitMqHelper.ExchangeConfig() {
               ExchangeName = "myexchange",
               ExchangeType = RabbitMqHelper.RabbitMqEnum.ExchangeTypeEnum.direct
            };
            RabbitMqHelper.QueueConfig queueConfig = new RabbitMqHelper.QueueConfig() { 
                QueueName = "myqueue",
                RoutingKey = "mykey",
                IsConfirm = true,

            };
            queueConfig.AddQueueParm("x-max-priority", 10);

            rmsdk0.Publish(exchangeConfig, queueConfig, "1", 1);
            rmsdk0.Publish(exchangeConfig, queueConfig, "2", 2);
            rmsdk0.Publish(exchangeConfig, queueConfig, "3", 3);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            rmsdk1.Receive("myqueue", haha, false, true);
            rmsdk2.Receive("myqueue", haha1, false, true);
        }

        private void haha(string msg)
        {                                           
            richTextBox1.Invoke(new MethodInvoker(delegate ()
            {
                richTextBox1.AppendText("haha====" + msg + "\n");
            }));
            Thread.Sleep(2000);
        }

        private void haha1(string msg)
        {
            richTextBox1.Invoke(new MethodInvoker(delegate ()
            {
                richTextBox1.AppendText("haha1====" + msg + "\n");
            }));
            Thread.Sleep(5000);
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
