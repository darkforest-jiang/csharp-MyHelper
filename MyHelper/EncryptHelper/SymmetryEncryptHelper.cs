using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper.EncryptHelper
{
    public class SymmetryEncryptHelper
    {
        private string _key;
        private string _iv;

        private SymmetricAlgorithm mobjCryptoService = new RijndaelManaged();

        public SymmetryEncryptHelper(string key, string iv)
        {
            _key = key;
            _iv = iv;
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="oriStr"></param>
        /// <returns></returns>
        public string Encrypt(string oriStr)
        {
            byte[] bytIn = UTF8Encoding.UTF8.GetBytes(oriStr);
            MemoryStream ms = new MemoryStream();
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateEncryptor(mobjCryptoService.Key, mobjCryptoService.IV);
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
            cs.Write(bytIn, 0, bytIn.Length);
            cs.FlushFinalBlock();
            ms.Close();
            byte[] bytOut = ms.ToArray();
            return Convert.ToBase64String(bytOut);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="secretStr"></param>
        /// <returns></returns>
        public string Decrypto(string secretStr)
        {
            byte[] bytIn = Convert.FromBase64String(secretStr);
            MemoryStream ms = new MemoryStream(bytIn, 0, bytIn.Length);
            mobjCryptoService.Key = GetLegalKey();
            mobjCryptoService.IV = GetLegalIV();
            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor(mobjCryptoService.Key, mobjCryptoService.IV);
            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }


        #region 私有方法

        /// <summary>
        /// 获取初始密钥
        /// </summary>
        /// <returns></returns>
        private byte[] GetLegalKey()
        {
            string strTemp = _key;
            mobjCryptoService.GenerateKey();
            byte[] bytTemp = mobjCryptoService.Key;
            int KeyLength = bytTemp.Length;
            if (strTemp.Length > KeyLength)
                strTemp = strTemp.Substring(0, KeyLength);
            else if (strTemp.Length < KeyLength)
                strTemp = strTemp.PadRight(KeyLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(strTemp);
        }

        /// <summary>    
        /// 获得初始向量IV    
        /// </summary>    
        /// <returns>初试向量IV</returns>    
        private byte[] GetLegalIV()
        {
            string strTemp = _iv;
            mobjCryptoService.GenerateIV();
            byte[] bytTemp = mobjCryptoService.IV;
            int IVLength = bytTemp.Length;
            if (strTemp.Length > IVLength)
                strTemp = strTemp.Substring(0, IVLength);
            else if (strTemp.Length < IVLength)
                strTemp = strTemp.PadRight(IVLength, ' ');

            return ASCIIEncoding.ASCII.GetBytes(strTemp);
        }

        #endregion

    }
}
