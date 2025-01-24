using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models.mcserver_ecodata;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverEcodbContext : DbContext
{
    private protected string ConnectionString;

    public McserverEcodbContext()
    {
        ConnectionString = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
    }

    public McserverEcodbContext(DbContextOptions<McserverEcodbContext> options)
        : base(options)
    {
        ConnectionString = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_ecodb");
    }

    public virtual DbSet<EcoBigDecimal> EcoBigDecimals { get; set; }

    public virtual DbSet<EcoBoolean> EcoBooleans { get; set; }

    public virtual DbSet<EcoConfig> EcoConfigs { get; set; }

    public virtual DbSet<EcoDatum> EcoData { get; set; }

    public virtual DbSet<EcoDouble> EcoDoubles { get; set; }

    public virtual DbSet<EcoInt> EcoInts { get; set; }

    public virtual DbSet<EcoString> EcoStrings { get; set; }

    public virtual DbSet<EcoStringList> EcoStringLists { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<EcoBigDecimal>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_big_decimal");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_big_decimal_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue)
                .HasPrecision(34, 4)
                .HasColumnName("dataValue");
        });

        modelBuilder.Entity<EcoBoolean>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_boolean");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_boolean_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue).HasColumnName("dataValue");
        });

        modelBuilder.Entity<EcoConfig>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_config");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_config_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue)
                .HasColumnType("text")
                .HasColumnName("dataValue");
        });

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

        modelBuilder.Entity<EcoDouble>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_double");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_double_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue).HasColumnName("dataValue");
        });

        modelBuilder.Entity<EcoInt>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_int");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_int_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue)
                .HasColumnType("int(11)")
                .HasColumnName("dataValue");
        });

        modelBuilder.Entity<EcoString>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("eco_string");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey }, "eco_string_profileUUID_dataKey_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.DataValue)
                .HasMaxLength(256)
                .HasColumnName("dataValue");
        });

        modelBuilder.Entity<EcoStringList>(entity =>
        {
            entity.HasKey(e => new { e.ProfileUuid, e.DataKey, e.ListIndex })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("eco_string_list");

            entity.HasIndex(e => new { e.ProfileUuid, e.DataKey, e.ListIndex }, "eco_string_list_profileUUID_dataKey_listIndex_unique").IsUnique();

            entity.Property(e => e.ProfileUuid)
                .HasMaxLength(16)
                .IsFixedLength()
                .HasColumnName("profileUUID");
            entity.Property(e => e.DataKey)
                .HasMaxLength(128)
                .HasColumnName("dataKey");
            entity.Property(e => e.ListIndex)
                .HasColumnType("int(11)")
                .HasColumnName("listIndex");
            entity.Property(e => e.DataValue)
                .HasMaxLength(256)
                .HasColumnName("dataValue");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
