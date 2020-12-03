using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MyHelper
{
    /// <summary>
    /// WebService帮助类
    /// </summary>
    public class WebServiceHelper
    {
        #region 生成SOAP报文请求WebService

        /// <summary>
        /// 缓存xmlNamespace，避免重复调用GetNamespace
        /// </summary>
        private static Hashtable _xmlNamespaces = new Hashtable();

        /// <summary>
        /// Post调用 需要WebService支持Post调用
        /// </summary>
        public static XmlDocument QueryPostWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            byte[] data = EncodePars(Pars);
            WriteRequestData(request, data);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        /// Get调用  需要WebService支持Get调用
        /// </summary>
        public static XmlDocument QueryGetWebService(String URL, String MethodName, Hashtable Pars)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL + "/" + MethodName + "?" + ParsToString(Pars));
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            SetWebRequest(request);
            return ReadXmlResponse(request.GetResponse());
        }

        /// <summary>
        /// 通用WebService调用(Soap)
        /// </summary>
        /// <param name="URL">请求地址，不包含后缀?WSDL</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="Prefix_MethodName">方法前缀</param>
        /// <param name="Pars">参数集合</param>
        public static XmlDocument QuerySoapWebService(String URL, String MethodName, String Prefix_MethodName, Dictionary<string, string> Pars)
        {
            if (_xmlNamespaces.ContainsKey(URL))
            {
                return QuerySoapWebService(URL, MethodName, Prefix_MethodName, Pars, _xmlNamespaces[URL].ToString());
            }
            else
            {
                return QuerySoapWebService(URL, MethodName, Prefix_MethodName, Pars, GetNamespace(URL));
            }
        }

        #region 私有方法

        /// <summary>
        /// 通用WebService调用(Soap)
        /// </summary>
        /// <param name="URL">请求地址，不包含后缀?WSDL</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="Pars">参数集合</param>
        /// <param name="XmlNs">方法命名空间</param>
        /// <returns></returns>
        private static XmlDocument QuerySoapWebService(String URL, String MethodName, String Prefix_MethodName, Dictionary<string, string> Pars, string XmlNs)
        {
            _xmlNamespaces[URL] = XmlNs;//加入缓存，提高效率
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "text/xml; charset=utf-8";
            request.Headers.Add("SOAPAction", "\"" + XmlNs + (XmlNs.EndsWith("/") ? "" : "/") + MethodName + "\"");
            SetWebRequest(request);
            byte[] data = EncodeParsToSoap(Pars, XmlNs, MethodName, Prefix_MethodName);
            WriteRequestData(request, data);
            XmlDocument doc = new XmlDocument(), doc2 = new XmlDocument();
            doc = ReadXmlResponse(request.GetResponse());
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            String RetXml = doc.SelectSingleNode("//soap:Body/*/*", mgr).InnerXml;
            doc2.LoadXml("<root>" + RetXml + "</root>");
            AddDelaration(doc2);
            return doc2;
        }

        /// <summary>
        /// 通过WebService的WSDL获取XML名称空间
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static string GetNamespace(String URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL + "?WSDL");
            SetWebRequest(request);
            WebResponse response = request.GetResponse();
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sr.ReadToEnd());
            sr.Close();
            return doc.SelectSingleNode("//@targetNamespace").Value;
        }

        /// <summary>
        /// 动态生成SOP请求报文内容
        /// </summary>
        /// <param name="Pars">参数集合</param>
        /// <param name="XmlNs">命名空间</param>
        /// <param name="MethodName">方法名称</param>
        /// <param name="Prefix_MethodName">方法前缀</param>
        /// <returns></returns>
        private static byte[] EncodeParsToSoap(Dictionary<string, string> Pars, String XmlNs, String MethodName, String Prefix_MethodName)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml($"<soap:Envelope xmlns=\"{XmlNs}\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\"></soap:Envelope>");
            AddDelaration(doc);
            XmlElement soapBody = doc.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            XmlElement soapMethod;
            if (string.IsNullOrEmpty(Prefix_MethodName))
            {
                soapMethod = doc.CreateElement(MethodName);
                soapMethod.SetAttribute("xmlns", XmlNs);
            }
            else
            {
                soapMethod = doc.CreateElement(Prefix_MethodName, MethodName, XmlNs);
            }
            //soapMethod.SetAttribute("xmlns:ws", XmlNs);
            foreach (var item in Pars)
            {
                XmlElement soapPar = doc.CreateElement(item.Key);
                soapPar.InnerXml = ObjectToSoapXml(item.Value);
                soapMethod.AppendChild(soapPar);
            }
            soapBody.AppendChild(soapMethod);
            doc.DocumentElement.AppendChild(soapBody);
            return Encoding.UTF8.GetBytes(doc.OuterXml);
        }

        /// <summary>
        /// 将对象转换成XML节点格式
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string ObjectToSoapXml(object o)
        {
            XmlSerializer mySerializer = new XmlSerializer(o.GetType());
            MemoryStream ms = new MemoryStream();
            mySerializer.Serialize(ms, o);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(ms.ToArray()));
            if (doc.DocumentElement != null)
            {
                return doc.DocumentElement.InnerXml;
            }
            else
            {
                return o.ToString();
            }
        }

        /// <summary>
        /// 设置WEB请求
        /// </summary>
        /// <param name="request"></param>
        private static void SetWebRequest(HttpWebRequest request)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
        }

        /// <summary>
        /// 设置请求数据
        /// </summary>
        /// <param name="request"></param>
        /// <param name="data"></param>
        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            Stream writer = request.GetRequestStream();
            writer.Write(data, 0, data.Length);
            writer.Close();
        }

        /// <summary>
        /// 获取字符串的UTF8码字符串
        /// </summary>
        /// <param name="Pars"></param>
        /// <returns></returns>
        private static byte[] EncodePars(Hashtable Pars)
        {
            return Encoding.UTF8.GetBytes(ParsToString(Pars));
        }

        /// <summary>
        /// 将Hashtable转换成WEB请求键值对字符串
        /// </summary>
        /// <param name="Pars"></param>
        /// <returns></returns>
        private static String ParsToString(Hashtable Pars)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string k in Pars.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }
                sb.Append(HttpUtility.UrlEncode(k) + "=" + HttpUtility.UrlEncode(Pars[k].ToString()));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取Webservice响应报文XML
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static XmlDocument ReadXmlResponse(WebResponse response)
        {
            StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            String retXml = sr.ReadToEnd();
            sr.Close();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(retXml);
            return doc;
        }

        /// <summary>
        /// 设置XML文档版本声明
        /// </summary>
        /// <param name="doc"></param>
        private static void AddDelaration(XmlDocument doc)
        {
            XmlDeclaration decl = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.InsertBefore(decl, doc.DocumentElement);
        }
        #endregion

        #endregion
    }
}
