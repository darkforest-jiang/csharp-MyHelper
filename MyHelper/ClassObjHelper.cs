using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHelper
{
    /// <summary>
    /// 类对象Helper
    /// </summary>
    public class ClassObjHelper
    {
        /// <summary>
        /// 类对象 复制值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="orginT"></param>
        /// <param name="targetT"></param>
        public static void CopyValue<T>(T orginT, ref T targetT, List<string> listExceptField)
        {
            try
            {
                System.Reflection.PropertyInfo[] properties = (targetT.GetType()).GetProperties();
                System.Reflection.PropertyInfo[] fields = (orginT.GetType()).GetProperties();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (listExceptField != null && listExceptField.Count > 0 && listExceptField.Contains(fields[i].Name))
                    {
                        continue;
                    }
                    for (int j = 0; j < properties.Length; j++)
                    {
                        if (fields[i].Name == properties[j].Name && properties[j].CanWrite)
                        {
                            properties[j].SetValue(targetT, fields[i].GetValue(orginT), null);
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
