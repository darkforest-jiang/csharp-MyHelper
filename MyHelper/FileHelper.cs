using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyHelper
{
    public class FileHelper
    {
        /// <summary>
        /// 将文本写入文件中
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="filePath">文件路径(包含文件名)</param>
        public static void WriteTextToFile(string text, string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine(text);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
