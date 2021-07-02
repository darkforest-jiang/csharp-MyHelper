using System;
using EFHelper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace EFHelper.Context
{
    /// <summary>
    /// SqlServer数据库Context
    /// 使用 VS 程序包管理器生成Context
    /// 需先安装下边3个Nuget包 
    /// Microsoft.EntityFrameworkCore
    /// Microsoft.EntityFrameworkCore.SqlServer     
    /// Microsoft.EntityFrameworkCore.Tools         
    /// 执行下边命令(注意所在类库必须编译通过没有错误否则执行失败)
    /// Scaffold-DbContext "Data Source=.; Initial Catalog=MyDB; Pooling=True; UID=sa;PWD=sa;connect Timeout=60” Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models  -Force
    /// </summary>
    public partial class MyDBSqlserverContext : MyDBContextBase
    {
        public MyDBSqlserverContext()
        {
        }

        public MyDBSqlserverContext(DbContextOptions<MyDBSqlserverContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //默认使用offset/fetch方式进行分页
                optionsBuilder.UseSqlServer(ConnStr);
                //下边这种支持rownumber分页(sqlserver2008只支持rownumber分页) core高版本不支持了   core 2.0版本支持
                //optionsBuilder.UseSqlServer(ConnStr, b => b.UseRowNumberForPaging());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_PRC_CI_AS");

            modelBuilder.Entity<TmyUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("TMy_User");

                entity.Property(e => e.UserId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
