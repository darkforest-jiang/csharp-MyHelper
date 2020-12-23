using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketHelper;

namespace TestHelperApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public SocketSdkHelper socketSdk1 = new SocketSdkHelper("127.0.0.1", 8999);
        public SocketSdkHelper socketSdk2 = new SocketSdkHelper("127.0.0.1", 8999);


        private void button1_Click(object sender, EventArgs e)
        {
            socketSdk1.StartServer(10, (socket)=> {
                showlog($"客户端：{socket.RemoteEndPoint} 已连接至服务器...");
            },
            (socket, smsg) => {
                showlog($"接受到客户端：{socket.RemoteEndPoint} 的消息，消息id：{smsg.Id}");
            },
            () => {
                showlog($"服务已关闭");
            }
            );
        }

        private void showlog(string text)
        {
            try
            {
                rtxt_log.Invoke(new MethodInvoker(delegate ()
                {
                    rtxt_log.AppendText(text + "\n");
                }));
            }
            catch (Exception ex) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            socketSdk1.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            socketSdk2.StartConnect((smg)=> {
                showlog($"收到服务器的消息：{smg.Id}");
            },
            ()=> {
                showlog($"已断开与服务器的连接");
            });
        }


        private void button4_Click(object sender, EventArgs e)
        {

            socketSdk2.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SocketSdkHelper.SendMsg(socketSdk2.GetSocket(), new SocketMessage() { 
                Id = 100,
                DataLen = (uint)Convert.FromBase64String("1111").Length,
                Data = Convert.FromBase64String("1111")
            });
        }
    }
}
