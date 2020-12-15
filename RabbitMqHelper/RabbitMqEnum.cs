using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqHelper
{
    public class RabbitMqEnum
    {
        public enum ExchangeTypeEnum
        {
            direct = 0,
            fanout,
            headers,
            topic
        }
    }
}
