using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.EncryptHelper
{
    public class DESEncryptHelper
    {
        /// <summary>
        /// 必须为8位
        /// </summary>
        private string _key;
        /// <summary>
        /// 必须为8位
        /// </summary>
        private string _iv;
                       
        /// <summary>
        /// 密钥和向量必须8位
        /// </summary>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        public DESEncryptHelper(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }

        public string Encrypt(string oriStr)
        {
            try
            {
                using (DESCryptoServiceProvider desCryptoService = new DESCryptoServiceProvider())
                {
                    desCryptoService.Key = GetLegalKey(desCryptoService);
                    desCryptoService.IV = GetLegalIV(desCryptoService);
                    byte[] inputByteArray = Encoding.UTF8.GetBytes(oriStr);
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, desCryptoService.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Decrypt(string secretStr)
        {
            try
            {
                using (DESCryptoServiceProvider desCryptoService = new DESCryptoServiceProvider())
                {
                    desCryptoService.Key = GetLegalKey(desCryptoService);
                    desCryptoService.IV = GetLegalIV(desCryptoService);
                    byte[] inputByteArray = Convert.FromBase64String(secretStr);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, desCryptoService.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(inputByteArray, 0, inputByteArray.Length);
                            cs.FlushFinalBlock();
                            cs.Close();
                        }

                        return Encoding.UTF8.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region 私有方法

        /// <summary>
        /// 获取初始密钥
        /// </summary>
        /// <returns></returns>
        private byte[] GetLegalKey(DESCryptoServiceProvider desCryptoService)
        {
            string strTemp = _key;
            desCryptoService.GenerateKey();
            byte[] bytTemp = desCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (strTemp.Length > KeyLength)
                strTemp = strTemp.Substring(0, KeyLength);
            else if (strTemp.Length < KeyLength)
                strTemp = strTemp.PadRight(KeyLength, ' ');

            return ASCIIEncoding.UTF8.GetBytes(strTemp);
        }

        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private byte[] GetLegalIV(DESCryptoServiceProvider desCryptoService)
        {
            string strTemp = _iv;
            desCryptoService.GenerateIV();
            byte[] bytTemp = desCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (strTemp.Length > IVLength)
                strTemp = strTemp.Substring(0, IVLength);
            else if (strTemp.Length < IVLength)
                strTemp = strTemp.PadRight(IVLength, ' ');

            return ASCIIEncoding.UTF8.GetBytes(strTemp);
        }

        #endregion

    }
}
