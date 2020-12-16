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
    public class RabbitMqSdkHelper: IDisposable
    {
        private RabbitMqConfig _rabbitMqConfig;

        private ConnectionFactory _connFactory;

        private IConnection _conn;

        private IModel _channel;

        

        public RabbitMqSdkHelper(RabbitMqConfig rmConfig)
        {
            _rabbitMqConfig = rmConfig;
            _connFactory = new ConnectionFactory()
            {
                HostName = _rabbitMqConfig.HostName,
                UserName = _rabbitMqConfig.UserName,
                Password = _rabbitMqConfig.Pwd
            };
        }

        public bool Publish(ExchangeConfig exchangeConfig, QueueConfig queueConfig, string msg, int? msgPriority)
        {
            try
            {
                var channel = GetChannel();
                if(channel == null)
                {
                    return false;
                }                                                        

                //申明交换机
                channel.ExchangeDeclare(exchangeConfig.ExchangeName, exchangeConfig.ExchangeType.ToString(), exchangeConfig.Durable, exchangeConfig.AutoDelete, exchangeConfig.ExchangeParms);
                //申明队列
                channel.QueueDeclare(queueConfig.QueueName, queueConfig.Durable, queueConfig.Exclusive, queueConfig.AutoDelete, queueConfig.QueueParms);
                if(queueConfig.FairQos)  //设置公平分发
                {
                    channel.BasicQos(0, 1, false);
                }
                //绑定交换机和队列
                channel.QueueBind(queueConfig.QueueName, exchangeConfig.ExchangeName, queueConfig.RoutingKey);
                var properties = channel.CreateBasicProperties();
                if(queueConfig.Durable)
                {
                    properties.DeliveryMode = 2;
                }
                else
                {
                    properties.DeliveryMode = 1;
                }
                if(msgPriority != null)
                {
                    properties.Priority = (byte)msgPriority;
                }

                //发送消息
                if(queueConfig.IsConfirm)
                {
                    channel.ConfirmSelect();
                }
                channel.BasicPublish(exchangeConfig.ExchangeName, queueConfig.RoutingKey, properties, Encoding.UTF8.GetBytes(msg));
                if(queueConfig.IsConfirm)
                {
                    if(channel.WaitForConfirms())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Receive(string queueName, Action<string> receiveAction, bool autoAck)
        {
            try
            {
                var channel = GetChannel();
                if (autoAck == false)
                {
                    channel.BasicQos(0, 1, false);//公平分发
                }
                var consumer = new EventingBasicConsumer(channel);//消费者
                channel.BasicConsume(queueName, autoAck, consumer);//消费消息
                consumer.Received += (model, ea) =>
                {
                    ThreadPool.QueueUserWorkItem((o) => {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        receiveAction(message);
                        if(autoAck == false)
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
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
