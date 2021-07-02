using EFHelper.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFHelper.Context
{
    /// <summary>
    /// 数据库Context基类
    /// </summary>
    public class MyDBContextBase : DbContext
    {
        /// <summary>
        /// 数据库连接字符串
        /// Sqlserver: Data Source=.;user id=sa;password=sa;initial catalog=MyDb;Connection Timeout=60;Max Pool Size=300
        /// Mysql: Server=localhost;Database=mydb;Uid=root;Pwd=root;sslmode=none
        /// </summary>
        public static string ConnStr;
        /// <summary>
        /// 数据库类型
        /// </summary>
        public static DbEnum DbType;

        public MyDBContextBase()
        {
        }

        public MyDBContextBase(DbContextOptions<MyDBSqlserverContext> options) : base(options)
        {
        }

        public MyDBContextBase(DbContextOptions<MyDBMysqlContext> options) : base(options)
        {
        }

        public virtual DbSet<TmyUser> TmyUsers { get; set; }


        /// <summary>
        /// 获取对应数据库Context
        /// </summary>
        /// <returns></returns>
        public static MyDBContextBase GetContext()
        {
            switch(DbType)
            {
                case DbEnum.Sqlserver:
                    return new MyDBSqlserverContext();
                case DbEnum.Mysql:
                    return new MyDBMysqlContext();
                default:
                    return new MyDBSqlserverContext();
            }
        }
    }

    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbEnum
    {
        Sqlserver,
        Mysql,
        Oracle
    }
}
