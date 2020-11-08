using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace MyHelper
{
    /// <summary>
    /// Http请求帮助类
    /// </summary>
    public class HttpRequestHelper
    {
        /// <summary>
        /// HttpWebRequest方式Post数据
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="contentType">Content-Type:application/x-www-form-urlencoded、application/json</param>
        /// <param name="requestParm">请求参数</param>
        /// <returns></returns>
        public static string HttpWebRequestPost(string url, object datajson)
        {
            System.GC.Collect();//垃圾回收，回收没有正常关闭的http连接

            string result = "";//返回结果

            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream reqStream = null;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Proxy = null;//注意设置代理为null，否则在一些低版本系统上使用时，调用接口特别慢
                request.Timeout = 60 * 1000;

                request.ContentType = "application/json";// "application/json;charset=UTF-8";
                byte[] byteData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(datajson));
                request.ContentLength = byteData.Length;

                //往服务器写入数据
                reqStream = request.GetRequestStream();
                reqStream.Write(byteData, 0, byteData.Length);
                reqStream.Close();

                //获取服务端返回
                response = (HttpWebResponse)request.GetResponse();

                //获取服务端返回数据
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                result = sr.ReadToEnd().Trim();
                sr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //关闭连接和流
                if (response != null)
                {
                    response.Close();
                }
                if (request != null)
                {
                    request.Abort();
                }
            }

            return result;
        }
    }
}
