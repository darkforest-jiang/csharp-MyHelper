using System;
using EFHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EFHelper.Context
{
    /// <summary>
    /// Mysql数据库Context
    /// 使用 VS 程序包管理器生成Context
    /// 需先安装下边3个Nuget包 
    /// MySql.EntityFrameworkCore
    /// Pomelo.EntityFrameworkCore.MySql     
    /// Microsoft.EntityFrameworkCore.Tools
    /// 执行下边命令(注意所在类库必须编译通过没有错误否则执行失败)
    /// Scaffold-DbContext "Server=localhost;Database=mydb;Uid=root;Pwd=root" Pomelo.EntityFrameworkCore.MySql -OutputDir Models -Force
    /// </summary>
    public partial class MyDBMysqlContext : MyDBContextBase
    {
        public MyDBMysqlContext()
        {
        }

        public MyDBMysqlContext(DbContextOptions<MyDBMysqlContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConnStr, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.20-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8")
                .UseCollation("utf8_general_ci");

            modelBuilder.Entity<TmyUser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PRIMARY");

                entity.ToTable("tmy_user");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.Property(e => e.UserCode).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
