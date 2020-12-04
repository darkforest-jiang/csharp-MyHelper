using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyHelper
{
    public class XmlHelper
    {
        /// <summary>
        /// xml文本去除CDATA标记
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string RemoveCDataTag(string xmlStr)
        {
            try
            {
                xmlStr = xmlStr.Replace("<![CDATA[", "");
                xmlStr = xmlStr.Replace("]]>", "");
                return xmlStr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将XML转换成实体对象
        /// </summary>
        /// <param name="xmlStr">XML</param>
        public static T DeserializeObj<T>(string xmlStr) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(xmlStr))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // <summary>
        /// 将XML转换成实体对象
        /// </summary>
        /// <param name="xmlStr">XML</param>
        public static T DeserializeObjFromFile<T>(string filePath) where T : class
        {
            try
            {
                string xmlStr = File.ReadAllText(filePath);
                return DeserializeObj<T>(xmlStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
