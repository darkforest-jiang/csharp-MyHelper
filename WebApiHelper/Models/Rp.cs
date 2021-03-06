﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiHelper.Models
{
    /// <summary>
    /// 请求参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Rp<T>
    {
        /// <summary>
        /// Token令牌
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public T InterfaceParm { get; set; }
    }
}
