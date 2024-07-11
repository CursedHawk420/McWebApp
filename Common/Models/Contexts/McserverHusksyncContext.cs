using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Models.mcserver_husksync;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverHusksyncContext : DbContext
{
    private Helpers.ConfigProvider manager = Helpers.ConfigProvider.Instance;

    private protected string ConnectionString;

    public McserverHusksyncContext()
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_husksync");
    }

    public McserverHusksyncContext(DbContextOptions<McserverHusksyncContext> options)
        : base(options)
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_husksync");
    }

    public virtual DbSet<HusksyncUser> HusksyncUsers { get; set; }

    public virtual DbSet<HusksyncUserDatum> HusksyncUserData { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<HusksyncUser>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity
                .ToTable("husksync_users")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Username, "husksync_users_username");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Uuid).HasColumnName("uuid");
            entity.Property(e => e.Username)
                .HasMaxLength(16)
                .HasColumnName("username");
        });

        modelBuilder.Entity<HusksyncUserDatum>(entity =>
        {
            entity.HasKey(e => new { e.VersionUuid, e.PlayerUuid })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity
                .ToTable("husksync_user_data")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid");

            entity.HasIndex(e => e.VersionUuid, "version_uuid").IsUnique();

            entity.Property(e => e.VersionUuid).HasColumnName("version_uuid");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.Data).HasColumnName("data");
            entity.Property(e => e.Pinned).HasColumnName("pinned");
            entity.Property(e => e.SaveCause)
                .HasMaxLength(32)
                .HasColumnName("save_cause");
            entity.Property(e => e.Timestamp)
                .HasColumnType("datetime")
                .HasColumnName("timestamp");

            entity.HasOne(d => d.PlayerUu).WithMany(p => p.HusksyncUserData)
                .HasForeignKey(d => d.PlayerUuid)
                .HasConstraintName("husksync_user_data_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
