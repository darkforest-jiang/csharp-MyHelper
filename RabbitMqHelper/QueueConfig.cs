using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMqHelper
{
    public class QueueConfig
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        /// 是否返送消息 需要确认
        /// </summary>
        public bool IsConfirm { get; set; } = false;

        /// <summary>
        /// 绑定队列 和交换机的Key
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// 是否持久化
        /// true: 服务重启后队列消息不会丢失 false:服务重启后队列消息会丢失
        /// </summary>
        public bool Durable { get; set; } = false;


        /// <summary>
        /// 排它性
        /// 只对首次声明它的连接（Connection）可见，会在其连接断开的时候自动删除
        /// 只区别连接（Connection）而不是通道（Channel）同一个连接下的通道（Channel）可以访问
        /// </summary>
        public bool Exclusive { get; set; } = false;

        /// <summary>
        /// 自动删除
        /// @Queue: 当所有消费客户端连接断开后，是否自动删除队列 true：删除false：不删除
        /// 当Queue中的 autoDelete 属性被设置为true时，那么，当消息接收着宕机，关闭后，消息队列则会删除，消息发送者一直发送消息，当消息接收者重新启动恢复正常后，会接收最新的消息，而宕机期间的消息则会丢失 
        ///  当Quere中的 autoDelete 属性被设置为false时，那么，当消息接收者宕机，关闭后，消息队列不会删除，消息发送者一直发送消息，当消息接收者重新启动恢复正常后，会接收包括宕机期间的消息。
        /// 当Quere中的 autoDelete 属性被设置为false时，那么，当消息接收者宕机，关闭后，消息队列不会删除，消息发送者一直发送消息，当消息接收者重新启动恢复正常后，会接收包括宕机期间的消息。
        /// autoDelete设置是否为临时的，临时的当消息接收者关闭时，队列、交换器则会被删除。为true时，则不会被删除。
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        /// 队列参数
        /// </summary>
        public IDictionary<string, object> QueueParms { get; set; }

        /// <summary>
        /// 添加队列参数
        /// </summary>
        public void AddQueueParm(string k, string v)
        {
            if(QueueParms == null)
            {
                QueueParms = new Dictionary<string, object>();
            }
            if(QueueParms.ContainsKey(k))
            {
                QueueParms[k] = v;
            }
            else
            {
                QueueParms.Add(k, v);
            }
        }
    }
}
