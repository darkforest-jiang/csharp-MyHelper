using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MyHelper.EncryptHelper
{
    public class RSAEncryptHelper
    {
        public static void GenerateKeys(out string privateKey, out string publicKey)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                privateKey = rsa.ToXmlString(true);
                publicKey = rsa.ToXmlString(false);
            }
        }

        private string _privateKey;
        private string _publicKey;

        public RSAEncryptHelper(string privateKey, string publicKey)
        {
            _privateKey = privateKey;
            _publicKey = publicKey;
        }

        #region 公钥加密  私钥解密
        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="oriStr"></param>
        /// <returns></returns>
        public string Encrypt(string oriStr)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(_publicKey);
                }
                catch(Exception)
                {
                    FromXmlString(rsa, _publicKey);
                }
                byte[] encryptedData = rsa.Encrypt(Encoding.UTF8.GetBytes(oriStr), false);
                return Convert.ToBase64String(encryptedData);
            }
        }

        /// <summary>
        /// 私钥解密
        /// </summary>
        /// <param name="secretStr"></param>
        /// <returns></returns>
        public string Decrypt(string secretStr)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    rsa.FromXmlString(_privateKey);
                }
                catch (Exception)
                {
                    FromXmlString(rsa, _privateKey);
                }
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(secretStr), false);
                return Encoding.UTF8.GetString(decryptedData);
            }
        }

        #endregion

        #region 签名验签
        /// <summary>
        /// 私钥生成签名
        /// </summary>
        /// <param name="oriStr"></param>
        /// <returns></returns>
        public string GenerateSign(string oriStr)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    //此方法有可能报异常Operation is not supported on this platform
                    rsa.FromXmlString(_privateKey);
                }
                catch (Exception)
                {
                    FromXmlString(rsa, _privateKey);
                }
                byte[] signedData = rsa.SignData(Encoding.UTF8.GetBytes(oriStr), new SHA1CryptoServiceProvider()); // 使用SHA1进行摘要算法，生成签名
                return Convert.ToBase64String(signedData);
            }
        }

        /// <summary>
        /// 公钥验证签名
        /// </summary>
        /// <param name="oriStr"></param>
        /// <param name="signedStr"></param>
        /// <returns></returns>
        public bool VerifySign(string oriStr, string signedStr)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                try
                {
                    //此方法有可能报异常Operation is not supported on this platform
                    rsa.FromXmlString(_publicKey);
                }
                catch (Exception)
                {
                    FromXmlString(rsa, _publicKey);
                }
                return rsa.VerifyData(Encoding.UTF8.GetBytes(oriStr), new SHA1CryptoServiceProvider(), Convert.FromBase64String(signedStr));
            }
        }
        #endregion


        #region 私有方法

        #region 解决RSA签名RSAalg.FromXmlString(privateKey)方法报异常：Operation is not supported on this platform 换成这个方法
        private void FromXmlString(RSACryptoServiceProvider rsa, string xml)
        {
            var csp = ExtractFromXml(xml);
            rsa.ImportParameters(csp);
        }

        private RSAParameters ExtractFromXml(string xml)
        {
            var csp = new RSAParameters();
            using (var reader = XmlReader.Create(new StringReader(xml)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType != XmlNodeType.Element)
                        continue;

                    var elName = reader.Name;
                    if (elName == "RSAKeyValue")
                        continue;

                    do
                    {
                        reader.Read();
                    } while (reader.NodeType != XmlNodeType.Text && reader.NodeType != XmlNodeType.EndElement);

                    if (reader.NodeType == XmlNodeType.EndElement)
                        continue;

                    var value = reader.Value;
                    switch (elName)
                    {
                        case "Modulus":
                            csp.Modulus = Convert.FromBase64String(value);
                            break;
                        case "Exponent":
                            csp.Exponent = Convert.FromBase64String(value);
                            break;
                        case "P":
                            csp.P = Convert.FromBase64String(value);
                            break;
                        case "Q":
                            csp.Q = Convert.FromBase64String(value);
                            break;
                        case "DP":
                            csp.DP = Convert.FromBase64String(value);
                            break;
                        case "DQ":
                            csp.DQ = Convert.FromBase64String(value);
                            break;
                        case "InverseQ":
                            csp.InverseQ = Convert.FromBase64String(value);
                            break;
                        case "D":
                            csp.D = Convert.FromBase64String(value);
                            break;
                    }
                }

                return csp;
            }
        }
        #endregion

        #endregion

    }
}
