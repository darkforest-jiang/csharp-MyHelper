using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqHelper
{
    public class RabbitMqHelper: IDisposable
    {
        private RabbitMqConfig _rabbitMqConfig;

        private ConnectionFactory _connFactory;

        private IConnection _conn;

        private IModel _channel;

        

        public RabbitMqHelper(RabbitMqConfig rmConfig)
        {
            _rabbitMqConfig = rmConfig;
            _connFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.UserName,
                Password = _rabbitMqConfig.Pwd
            };
        }

        public bool PushMsg(string queueName, List<string> listMsg)
        {
            try
            {
                var channel = GetChannel();
                if(channel == null)
                {
                    return false;
                }
                channel.QueueDeclare(queueName, false, false, false, null);
                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 1;//消息持久化

                foreach (var msg in listMsg)
                {
                    channel.BasicPublish("", queueName, properties, Encoding.UTF8.GetBytes(msg));
                    Thread.Sleep(1 * 1000);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReceiveMsg(string queueName, Action<string> receiveAction)
        {
            try
            {
                var channel = GetChannel();

                var consumer = new EventingBasicConsumer(channel);//消费者
                channel.BasicConsume(queueName, true, consumer);//消费消息
                consumer.Received += (model, ea) =>
                {
                    ThreadPool.QueueUserWorkItem((o)=> {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        receiveAction(message);
                    }, null);
                };
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReceiveOneMsg(string queueName, out string msg)
        {
            msg = string.Empty;
            try
            {
                var channel = GetChannel();
                if(channel == null)
                {
                    return false;
                }

                var result = channel.BasicGet(queueName, true);
                if (result == null)
                {
                    return false;
                }
                msg = Encoding.UTF8.GetString(result.Body.ToArray());
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        #region 私有方法
        private IConnection GetConnection()
        {
            try
            {
                if(_conn != null)
                {
                    return _conn;
                }
                _conn = _connFactory.CreateConnection();
                return _conn; 
            }
            catch(Exception ex)
            {
                _conn = null;
                return _conn;
            }
        }

        private IModel GetChannel()
        {
            try
            {
                if(_channel != null && !_channel.IsClosed)
                {
                    return _channel;
                }

                var conn = GetConnection();
                if (conn == null)
                {
                    return null;
                }

                _channel = conn.CreateModel();
                return _channel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        public void Dispose()
        {
            try
            {
                if (_channel != null)
                {
                    if (_channel.IsClosed == false)
                    {
                        _channel.Close();
                    }
                    _channel = null;
                }
            }
            catch (Exception) { }
            try
            {
                if(_conn != null)
                {
                    if(_conn.IsOpen)
                    {
                        _conn.Close();
                    }
                    _conn = null;
                }
            }
            catch (Exception) { }
        }

    }
}
