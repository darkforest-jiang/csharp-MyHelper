using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMqHelper
{
    public class ExchangeConfig
    {
        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 交换机类型
        /// </summary>
        public RabbitMqEnum.ExchangeTypeEnum ExchangeType { get; set; }

        /// <summary>
        /// 持久化
        /// </summary>
        public bool Durable { get; set; } = false;

        /// <summary>
        /// 自动删除
        /// </summary>
        public bool AutoDelete { get; set; } = false;

        /// <summary>
        /// 交换机参数
        /// </summary>
        public Dictionary<string, object> ExchangeParms { get; set; }

        /// <summary>
        /// 添加队列参数
        /// </summary>
        public void AddExchangeParm(string k, string v)
        {
            if (ExchangeParms == null)
            {
                ExchangeParms = new Dictionary<string, object>();
            }
            if (ExchangeParms.ContainsKey(k))
            {
                ExchangeParms[k] = v;
            }
            else
            {
                ExchangeParms.Add(k, v);
            }
        }

    }
}
