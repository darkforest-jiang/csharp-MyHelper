using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketHelper
{
    public class SocketSdkHelper:IDisposable
    {
        private Socket _socket;
        private IPEndPoint _IPEndPoint;
        private bool _isStarted = false;

        public SocketSdkHelper(string ip, int port)
        {
            _IPEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        public Socket GetSocket()
        {
            return _socket;
        }

        public bool StartServer(int listenNum, Action<Socket> ConnectAction, Action<Socket, SocketMessage> ReceiveAction, Action stopAction)
        {
            try
            {
                if(_isStarted)
                {
                    return true;
                }

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Bind(_IPEndPoint);
                _socket.Listen(listenNum);

                _isStarted = true;
                ThreadPool.QueueUserWorkItem((o)=> {
                    while(true)
                    {
                        if(_isStarted == false)
                        {
                            Close();
                            stopAction();
                            return;
                        }
                        Socket client = null;
                        try
                        {
                            client = _socket.Accept();
                        }
                        catch(Exception)
                        {
                            CloseSocket(client);
                            Close();
                            stopAction();
                            return;
                        }
                        ThreadPool.QueueUserWorkItem((oo)=> {
                            ConnectAction(client);
                        }, null);
                        ThreadPool.QueueUserWorkItem((oo)=>{
                            while (_isStarted)
                            {
                                var msg = ReceiveMsg(client);
                                if (msg != null)
                                {
                                    ThreadPool.QueueUserWorkItem((oo) =>
                                    {
                                        ReceiveAction(client, msg);
                                    }, null);
                                }
                            }
                        }, null);

                        Thread.Sleep(1 * 1000);
                    }
                }, null);

                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public bool StartConnect(Action<SocketMessage> ReceiveAction, Action stopAction)
        { 
            try
            {
                if (_isStarted)
                {
                    return true;
                }

                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _socket.Connect(_IPEndPoint);

                _isStarted = true;
                ThreadPool.QueueUserWorkItem((o) => {
                    while (true)
                    {
                        if (_isStarted)
                        {
                            var msg = ReceiveMsg(_socket);
                            if (msg != null)
                            {
                                ThreadPool.QueueUserWorkItem((oo) =>
                                {
                                    ReceiveAction(msg);
                                }, null);
                            }
                        }
                        else
                        {
                            stopAction();
                            return;
                        }
                        Thread.Sleep(1 * 1000);
                    }
                }, null);

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private SocketMessage ReceiveMsg(Socket socket)
        {
            try
            {
                DataPack dp = new DataPack();
                byte[] data = new byte[dp.GetHeadLen()];
                int len = socket.Receive(data);
                if (len <= 0)
                {
                    return null;
                }
                var msg = dp.UnPack(data);
                msg.Data = new byte[msg.DataLen];
                len = socket.Receive(msg.Data);
                if (len <= 0)
                {
                    return null;
                }

                return msg;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public void Dispose()
        {
            Close();
        }

        public void Close()
        {
            _isStarted = false;
            CloseSocket(_socket);
        }

        public static void CloseSocket(Socket socket)
        {
            if(socket == null)
            {
                return;
            }
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch (Exception) { }
            try
            {
                socket.Close();
            }
            catch (Exception) { }
            try
            {
                socket.Dispose();
            }
            catch (Exception) { }
            socket = null;
        }

        public static void SendMsg(Socket socket, SocketMessage msg)
        {
            try
            {
                DataPack dp = new DataPack();
                var data = dp.Pack(msg);
                socket.Send(data);
            }
            catch (Exception) { }
        }

    }
}
