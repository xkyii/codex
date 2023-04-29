﻿using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Kx.Codex.Db;

public class CodexDbContext : DbContext
{
    public DbSet<Table> Tables { get; set; }

    /// <summary>
    /// 迁移需要
    /// </summary>
    public CodexDbContext()
    {
    }

    /// <summary>
    /// 为注入提供配置入口
    /// </summary>
    /// <param name="options"></param>
    public CodexDbContext(DbContextOptions<CodexDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 迁移需要
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySQL("server=localhost;port=3306;database=yf_data;user=yfty_admin;password=Yfty!23456");
        base.OnConfiguring(options);
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.DefaultTypeMapping<Table>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Table>(builder =>
        {
            builder.ToSqlQuery("select * from information_schema.tables where TABLE_SCHEMA=(select database());")
                .HasKey(c => new { c.TableCatalog, c.TableSchema, c.TableName, c.TableType });
        });

        base.OnModelCreating(modelBuilder);
    }
}
