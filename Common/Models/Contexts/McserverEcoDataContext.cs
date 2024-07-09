using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Models.mcserver_ecodata;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverEcoDataContext : DbContext
{
    private Helpers.ConfigProvider manager = Helpers.ConfigProvider.Instance;

    private protected string ConnectionString;

    public McserverEcoDataContext()
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
    }

    public McserverEcoDataContext(DbContextOptions<McserverEcoDataContext> options)
        : base(options)
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
    }

    public virtual DbSet<EcoDatum> EcoData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<EcoDatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("eco_data");

            entity.Property(e => e.Id)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.JsonData).HasColumnName("json_data");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
