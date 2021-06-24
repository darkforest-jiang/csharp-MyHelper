using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    /// <summary>
    /// 数据库帮助类基类
    /// </summary>
    public class DbHelperBase
    {
        /// <summary>
        /// 行信息映射到Model对象信息
        /// </summary>
        /// <param name="rs">数据表行信息</param>
        /// <param name="objInfo">Model信息对象</param>
        /// <returns>true:映射成功 false:映射失败</returns>
        public bool DataRowToInfoObj(DataRow rs, object objInfo)
        {
            if (rs == null)
                return false;

            for (int i = 0; i < rs.Table.Columns.Count; i++)
            {
                System.Reflection.PropertyInfo pInfo = objInfo.GetType().GetProperty(rs.Table.Columns[i].ColumnName);
                if (pInfo != null)
                {
                    if (rs[i] != DBNull.Value)
                    {
                        if (pInfo.PropertyType.IsEnum)
                        {
                            pInfo.SetValue(objInfo, Enum.ToObject(pInfo.PropertyType, rs[i]), null);
                        }
                        else
                        {
                            pInfo.SetValue(objInfo, rs[i], null);
                        }
                    }
                }
            }


            return true;
        }
    }
}
