using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Highgeek.McWebApp.Common.Models.mcserver_plandb;
using Highgeek.McWebApp.Common.Helpers;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverPlandbContext : DbContext
{
    private protected string ConnectionString;

    public McserverPlandbContext()
    {
        ConnectionString = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_plandb");
    }

    public McserverPlandbContext(DbContextOptions<McserverPlandbContext> options)
        : base(options)
    {
        ConnectionString = ConfigProvider.GetConnectionString("MysqlMCServerConnection_mcserver_plandb");
        Database.EnsureCreated();
    }

    public virtual DbSet<PlanAccessLog> PlanAccessLogs { get; set; }

    public virtual DbSet<PlanAllowlistBounce> PlanAllowlistBounces { get; set; }

    public virtual DbSet<PlanCookie> PlanCookies { get; set; }

    public virtual DbSet<PlanExtensionGroup> PlanExtensionGroups { get; set; }

    public virtual DbSet<PlanExtensionIcon> PlanExtensionIcons { get; set; }

    public virtual DbSet<PlanExtensionPlugin> PlanExtensionPlugins { get; set; }

    public virtual DbSet<PlanExtensionProvider> PlanExtensionProviders { get; set; }

    public virtual DbSet<PlanExtensionServerTableValue> PlanExtensionServerTableValues { get; set; }

    public virtual DbSet<PlanExtensionServerValue> PlanExtensionServerValues { get; set; }

    public virtual DbSet<PlanExtensionTab> PlanExtensionTabs { get; set; }

    public virtual DbSet<PlanExtensionTable> PlanExtensionTables { get; set; }

    public virtual DbSet<PlanExtensionUserTableValue> PlanExtensionUserTableValues { get; set; }

    public virtual DbSet<PlanExtensionUserValue> PlanExtensionUserValues { get; set; }

    public virtual DbSet<PlanGeolocation> PlanGeolocations { get; set; }

    public virtual DbSet<PlanJoinAddress> PlanJoinAddresses { get; set; }

    public virtual DbSet<PlanKill> PlanKills { get; set; }

    public virtual DbSet<PlanNickname> PlanNicknames { get; set; }

    public virtual DbSet<PlanPing> PlanPings { get; set; }

    public virtual DbSet<PlanPluginVersion> PlanPluginVersions { get; set; }

    public virtual DbSet<PlanSecurity> PlanSecurities { get; set; }

    public virtual DbSet<PlanServer> PlanServers { get; set; }

    public virtual DbSet<PlanSession> PlanSessions { get; set; }

    public virtual DbSet<PlanSetting> PlanSettings { get; set; }

    public virtual DbSet<PlanTp> PlanTps { get; set; }

    public virtual DbSet<PlanUser> PlanUsers { get; set; }

    public virtual DbSet<PlanUserInfo> PlanUserInfos { get; set; }

    public virtual DbSet<PlanVersionProtocol> PlanVersionProtocols { get; set; }

    public virtual DbSet<PlanWebGroup> PlanWebGroups { get; set; }

    public virtual DbSet<PlanWebGroupToPermission> PlanWebGroupToPermissions { get; set; }

    public virtual DbSet<PlanWebPermission> PlanWebPermissions { get; set; }

    public virtual DbSet<PlanWebUserPreference> PlanWebUserPreferences { get; set; }

    public virtual DbSet<PlanWorld> PlanWorlds { get; set; }

    public virtual DbSet<PlanWorldTime> PlanWorldTimes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<PlanAccessLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_access_log");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.FromIp)
                .HasMaxLength(45)
                .HasColumnName("from_ip");
            entity.Property(e => e.RequestMethod)
                .HasMaxLength(8)
                .HasColumnName("request_method");
            entity.Property(e => e.RequestUri)
                .HasColumnType("text")
                .HasColumnName("request_uri");
            entity.Property(e => e.ResponseCode)
                .HasColumnType("int(11)")
                .HasColumnName("response_code");
            entity.Property(e => e.Time)
                .HasColumnType("bigint(20)")
                .HasColumnName("time");
        });

        modelBuilder.Entity<PlanAllowlistBounce>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_allowlist_bounce");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.LastBounce)
                .HasColumnType("bigint(20)")
                .HasColumnName("last_bounce");
            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.Times)
                .HasColumnType("int(11)")
                .HasColumnName("times");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanAllowlistBounces)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_allowlist_bounce_ibfk_1");
        });

        modelBuilder.Entity<PlanCookie>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_cookies");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Cookie)
                .HasMaxLength(64)
                .HasColumnName("cookie");
            entity.Property(e => e.Expires)
                .HasColumnType("bigint(20)")
                .HasColumnName("expires");
            entity.Property(e => e.WebUsername)
                .HasMaxLength(100)
                .HasColumnName("web_username");
        });

        modelBuilder.Entity<PlanExtensionGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_groups");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.GroupName)
                .HasMaxLength(50)
                .HasColumnName("group_name");
            entity.Property(e => e.ProviderId)
                .HasColumnType("int(11)")
                .HasColumnName("provider_id");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");

            entity.HasOne(d => d.Provider).WithMany(p => p.PlanExtensionGroups)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_groups_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionIcon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_icons");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Color)
                .HasMaxLength(25)
                .HasDefaultValueSql("'NONE'")
                .HasColumnName("color");
            entity.Property(e => e.Family)
                .HasMaxLength(15)
                .HasDefaultValueSql("'SOLID'")
                .HasColumnName("family");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasDefaultValueSql("'question'")
                .HasColumnName("name");
        });

        modelBuilder.Entity<PlanExtensionPlugin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_plugins");

            entity.HasIndex(e => e.IconId, "icon_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IconId)
                .HasColumnType("int(11)")
                .HasColumnName("icon_id");
            entity.Property(e => e.LastUpdated)
                .HasColumnType("bigint(20)")
                .HasColumnName("last_updated");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.ServerUuid)
                .HasMaxLength(36)
                .HasColumnName("server_uuid");

            entity.HasOne(d => d.Icon).WithMany(p => p.PlanExtensionPlugins)
                .HasForeignKey(d => d.IconId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_plugins_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionProvider>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_providers");

            entity.HasIndex(e => e.IconId, "icon_id");

            entity.HasIndex(e => e.PluginId, "plugin_id");

            entity.HasIndex(e => e.TabId, "tab_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ConditionName)
                .HasMaxLength(54)
                .HasColumnName("condition_name");
            entity.Property(e => e.Description)
                .HasMaxLength(150)
                .HasColumnName("description");
            entity.Property(e => e.FormatType)
                .HasMaxLength(25)
                .HasColumnName("format_type");
            entity.Property(e => e.Groupable).HasColumnName("groupable");
            entity.Property(e => e.Hidden).HasColumnName("hidden");
            entity.Property(e => e.IconId)
                .HasColumnType("int(11)")
                .HasColumnName("icon_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PlayerName).HasColumnName("player_name");
            entity.Property(e => e.PluginId)
                .HasColumnType("int(11)")
                .HasColumnName("plugin_id");
            entity.Property(e => e.Priority)
                .HasColumnType("int(11)")
                .HasColumnName("priority");
            entity.Property(e => e.ProvidedCondition)
                .HasMaxLength(50)
                .HasColumnName("provided_condition");
            entity.Property(e => e.ShowInPlayersTable).HasColumnName("show_in_players_table");
            entity.Property(e => e.TabId)
                .HasColumnType("int(11)")
                .HasColumnName("tab_id");
            entity.Property(e => e.Text)
                .HasMaxLength(50)
                .HasColumnName("text");

            entity.HasOne(d => d.Icon).WithMany(p => p.PlanExtensionProviders)
                .HasForeignKey(d => d.IconId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_providers_ibfk_2");

            entity.HasOne(d => d.Plugin).WithMany(p => p.PlanExtensionProviders)
                .HasForeignKey(d => d.PluginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_providers_ibfk_1");

            entity.HasOne(d => d.Tab).WithMany(p => p.PlanExtensionProviders)
                .HasForeignKey(d => d.TabId)
                .HasConstraintName("plan_extension_providers_ibfk_3");
        });

        modelBuilder.Entity<PlanExtensionServerTableValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_server_table_values");

            entity.HasIndex(e => e.TableId, "table_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Col1Value)
                .HasMaxLength(250)
                .HasColumnName("col_1_value");
            entity.Property(e => e.Col2Value)
                .HasMaxLength(250)
                .HasColumnName("col_2_value");
            entity.Property(e => e.Col3Value)
                .HasMaxLength(250)
                .HasColumnName("col_3_value");
            entity.Property(e => e.Col4Value)
                .HasMaxLength(250)
                .HasColumnName("col_4_value");
            entity.Property(e => e.Col5Value)
                .HasMaxLength(250)
                .HasColumnName("col_5_value");
            entity.Property(e => e.TableId)
                .HasColumnType("int(11)")
                .HasColumnName("table_id");
            entity.Property(e => e.TableRow)
                .HasColumnType("int(11)")
                .HasColumnName("table_row");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");

            entity.HasOne(d => d.Table).WithMany(p => p.PlanExtensionServerTableValues)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_server_table_values_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionServerValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_server_values");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BooleanValue).HasColumnName("boolean_value");
            entity.Property(e => e.ComponentValue)
                .HasMaxLength(500)
                .HasColumnName("component_value");
            entity.Property(e => e.DoubleValue).HasColumnName("double_value");
            entity.Property(e => e.GroupValue)
                .HasMaxLength(50)
                .HasColumnName("group_value");
            entity.Property(e => e.LongValue)
                .HasColumnType("bigint(20)")
                .HasColumnName("long_value");
            entity.Property(e => e.PercentageValue).HasColumnName("percentage_value");
            entity.Property(e => e.ProviderId)
                .HasColumnType("int(11)")
                .HasColumnName("provider_id");
            entity.Property(e => e.StringValue)
                .HasMaxLength(50)
                .HasColumnName("string_value");

            entity.HasOne(d => d.Provider).WithMany(p => p.PlanExtensionServerValues)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_server_values_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionTab>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_tabs");

            entity.HasIndex(e => e.IconId, "icon_id");

            entity.HasIndex(e => e.PluginId, "plugin_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ElementOrder)
                .HasMaxLength(100)
                .HasDefaultValueSql("'VALUES,GRAPH,TABLE'")
                .HasColumnName("element_order");
            entity.Property(e => e.IconId)
                .HasColumnType("int(11)")
                .HasColumnName("icon_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PluginId)
                .HasColumnType("int(11)")
                .HasColumnName("plugin_id");
            entity.Property(e => e.TabPriority)
                .HasColumnType("int(11)")
                .HasColumnName("tab_priority");

            entity.HasOne(d => d.Icon).WithMany(p => p.PlanExtensionTabs)
                .HasForeignKey(d => d.IconId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_tabs_ibfk_2");

            entity.HasOne(d => d.Plugin).WithMany(p => p.PlanExtensionTabs)
                .HasForeignKey(d => d.PluginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_tabs_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_tables");

            entity.HasIndex(e => e.Icon1Id, "icon_1_id");

            entity.HasIndex(e => e.Icon2Id, "icon_2_id");

            entity.HasIndex(e => e.Icon3Id, "icon_3_id");

            entity.HasIndex(e => e.Icon4Id, "icon_4_id");

            entity.HasIndex(e => e.Icon5Id, "icon_5_id");

            entity.HasIndex(e => e.PluginId, "plugin_id");

            entity.HasIndex(e => e.TabId, "tab_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Col1Name)
                .HasMaxLength(50)
                .HasColumnName("col_1_name");
            entity.Property(e => e.Col2Name)
                .HasMaxLength(50)
                .HasColumnName("col_2_name");
            entity.Property(e => e.Col3Name)
                .HasMaxLength(50)
                .HasColumnName("col_3_name");
            entity.Property(e => e.Col4Name)
                .HasMaxLength(50)
                .HasColumnName("col_4_name");
            entity.Property(e => e.Col5Name)
                .HasMaxLength(50)
                .HasColumnName("col_5_name");
            entity.Property(e => e.Color)
                .HasMaxLength(25)
                .HasDefaultValueSql("'NONE'")
                .HasColumnName("color");
            entity.Property(e => e.ConditionName)
                .HasMaxLength(54)
                .HasColumnName("condition_name");
            entity.Property(e => e.Format1)
                .HasMaxLength(15)
                .HasColumnName("format_1");
            entity.Property(e => e.Format2)
                .HasMaxLength(15)
                .HasColumnName("format_2");
            entity.Property(e => e.Format3)
                .HasMaxLength(15)
                .HasColumnName("format_3");
            entity.Property(e => e.Format4)
                .HasMaxLength(15)
                .HasColumnName("format_4");
            entity.Property(e => e.Format5)
                .HasMaxLength(15)
                .HasColumnName("format_5");
            entity.Property(e => e.Icon1Id)
                .HasColumnType("int(11)")
                .HasColumnName("icon_1_id");
            entity.Property(e => e.Icon2Id)
                .HasColumnType("int(11)")
                .HasColumnName("icon_2_id");
            entity.Property(e => e.Icon3Id)
                .HasColumnType("int(11)")
                .HasColumnName("icon_3_id");
            entity.Property(e => e.Icon4Id)
                .HasColumnType("int(11)")
                .HasColumnName("icon_4_id");
            entity.Property(e => e.Icon5Id)
                .HasColumnType("int(11)")
                .HasColumnName("icon_5_id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.PluginId)
                .HasColumnType("int(11)")
                .HasColumnName("plugin_id");
            entity.Property(e => e.TabId)
                .HasColumnType("int(11)")
                .HasColumnName("tab_id");
            entity.Property(e => e.ValuesFor)
                .HasDefaultValueSql("'0'")
                .HasColumnType("int(11)")
                .HasColumnName("values_for");

            entity.HasOne(d => d.Icon1).WithMany(p => p.PlanExtensionTableIcon1s)
                .HasForeignKey(d => d.Icon1Id)
                .HasConstraintName("plan_extension_tables_ibfk_2");

            entity.HasOne(d => d.Icon2).WithMany(p => p.PlanExtensionTableIcon2s)
                .HasForeignKey(d => d.Icon2Id)
                .HasConstraintName("plan_extension_tables_ibfk_3");

            entity.HasOne(d => d.Icon3).WithMany(p => p.PlanExtensionTableIcon3s)
                .HasForeignKey(d => d.Icon3Id)
                .HasConstraintName("plan_extension_tables_ibfk_4");

            entity.HasOne(d => d.Icon4).WithMany(p => p.PlanExtensionTableIcon4s)
                .HasForeignKey(d => d.Icon4Id)
                .HasConstraintName("plan_extension_tables_ibfk_5");

            entity.HasOne(d => d.Icon5).WithMany(p => p.PlanExtensionTableIcon5s)
                .HasForeignKey(d => d.Icon5Id)
                .HasConstraintName("plan_extension_tables_ibfk_6");

            entity.HasOne(d => d.Plugin).WithMany(p => p.PlanExtensionTables)
                .HasForeignKey(d => d.PluginId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_tables_ibfk_1");

            entity.HasOne(d => d.Tab).WithMany(p => p.PlanExtensionTables)
                .HasForeignKey(d => d.TabId)
                .HasConstraintName("plan_extension_tables_ibfk_7");
        });

        modelBuilder.Entity<PlanExtensionUserTableValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_user_table_values");

            entity.HasIndex(e => e.TableId, "table_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Col1Value)
                .HasMaxLength(250)
                .HasColumnName("col_1_value");
            entity.Property(e => e.Col2Value)
                .HasMaxLength(250)
                .HasColumnName("col_2_value");
            entity.Property(e => e.Col3Value)
                .HasMaxLength(250)
                .HasColumnName("col_3_value");
            entity.Property(e => e.Col4Value)
                .HasMaxLength(250)
                .HasColumnName("col_4_value");
            entity.Property(e => e.TableId)
                .HasColumnType("int(11)")
                .HasColumnName("table_id");
            entity.Property(e => e.TableRow)
                .HasColumnType("int(11)")
                .HasColumnName("table_row");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");

            entity.HasOne(d => d.Table).WithMany(p => p.PlanExtensionUserTableValues)
                .HasForeignKey(d => d.TableId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_user_table_values_ibfk_1");
        });

        modelBuilder.Entity<PlanExtensionUserValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_extension_user_values");

            entity.HasIndex(e => e.ProviderId, "provider_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.BooleanValue).HasColumnName("boolean_value");
            entity.Property(e => e.ComponentValue)
                .HasMaxLength(500)
                .HasColumnName("component_value");
            entity.Property(e => e.DoubleValue).HasColumnName("double_value");
            entity.Property(e => e.GroupValue)
                .HasMaxLength(50)
                .HasColumnName("group_value");
            entity.Property(e => e.LongValue)
                .HasColumnType("bigint(20)")
                .HasColumnName("long_value");
            entity.Property(e => e.PercentageValue).HasColumnName("percentage_value");
            entity.Property(e => e.ProviderId)
                .HasColumnType("int(11)")
                .HasColumnName("provider_id");
            entity.Property(e => e.StringValue)
                .HasMaxLength(50)
                .HasColumnName("string_value");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");

            entity.HasOne(d => d.Provider).WithMany(p => p.PlanExtensionUserValues)
                .HasForeignKey(d => d.ProviderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_extension_user_values_ibfk_1");
        });

        modelBuilder.Entity<PlanGeolocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_geolocations");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Geolocation)
                .HasMaxLength(50)
                .HasColumnName("geolocation");
            entity.Property(e => e.LastUsed)
                .HasColumnType("bigint(20)")
                .HasColumnName("last_used");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.PlanGeolocations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_geolocations_ibfk_1");
        });

        modelBuilder.Entity<PlanJoinAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_join_address");

            entity.HasIndex(e => e.JoinAddress, "join_address").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.JoinAddress)
                .HasMaxLength(191)
                .HasColumnName("join_address");
        });

        modelBuilder.Entity<PlanKill>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_kills");

            entity.HasIndex(e => e.Date, "plan_kills_date_index");

            entity.HasIndex(e => new { e.KillerUuid, e.VictimUuid, e.ServerUuid }, "plan_kills_uuid_index");

            entity.HasIndex(e => e.SessionId, "session_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("bigint(20)")
                .HasColumnName("date");
            entity.Property(e => e.KillerUuid)
                .HasMaxLength(36)
                .HasColumnName("killer_uuid");
            entity.Property(e => e.ServerUuid)
                .HasMaxLength(36)
                .HasColumnName("server_uuid");
            entity.Property(e => e.SessionId)
                .HasColumnType("int(11)")
                .HasColumnName("session_id");
            entity.Property(e => e.VictimUuid)
                .HasMaxLength(36)
                .HasColumnName("victim_uuid");
            entity.Property(e => e.Weapon)
                .HasMaxLength(30)
                .HasColumnName("weapon");

            entity.HasOne(d => d.Session).WithMany(p => p.PlanKills)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_kills_ibfk_1");
        });

        modelBuilder.Entity<PlanNickname>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_nicknames");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.LastUsed)
                .HasColumnType("bigint(20)")
                .HasColumnName("last_used");
            entity.Property(e => e.Nickname)
                .HasMaxLength(75)
                .HasColumnName("nickname");
            entity.Property(e => e.ServerUuid)
                .HasMaxLength(36)
                .HasColumnName("server_uuid");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<PlanPing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_ping");

            entity.HasIndex(e => e.Date, "plan_ping_date_index");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AvgPing).HasColumnName("avg_ping");
            entity.Property(e => e.Date)
                .HasColumnType("bigint(20)")
                .HasColumnName("date");
            entity.Property(e => e.MaxPing)
                .HasColumnType("int(11)")
                .HasColumnName("max_ping");
            entity.Property(e => e.MinPing)
                .HasColumnType("int(11)")
                .HasColumnName("min_ping");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanPings)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_ping_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.PlanPings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_ping_ibfk_1");
        });

        modelBuilder.Entity<PlanPluginVersion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_plugin_versions");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Modified)
                .HasColumnType("bigint(20)")
                .HasColumnName("modified");
            entity.Property(e => e.PluginName)
                .HasMaxLength(100)
                .HasColumnName("plugin_name");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .HasColumnName("version");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanPluginVersions)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_plugin_versions_ibfk_1");
        });

        modelBuilder.Entity<PlanSecurity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_security");

            entity.HasIndex(e => e.GroupId, "group_id");

            entity.HasIndex(e => e.SaltedPassHash, "salted_pass_hash").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.LinkedToUuid)
                .HasMaxLength(36)
                .HasDefaultValueSql("''")
                .HasColumnName("linked_to_uuid");
            entity.Property(e => e.SaltedPassHash)
                .HasMaxLength(100)
                .HasColumnName("salted_pass_hash");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .HasColumnName("username");

            entity.HasOne(d => d.Group).WithMany(p => p.PlanSecurities)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_security_ibfk_1");
        });

        modelBuilder.Entity<PlanServer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_servers");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.IsInstalled)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("is_installed");
            entity.Property(e => e.IsProxy).HasColumnName("is_proxy");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PlanVersion)
                .HasMaxLength(18)
                .HasColumnName("plan_version");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.WebAddress)
                .HasMaxLength(100)
                .HasColumnName("web_address");
        });

        modelBuilder.Entity<PlanSession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_sessions");

            entity.HasIndex(e => e.JoinAddressId, "plan_session_join_address_index");

            entity.HasIndex(e => e.SessionStart, "plan_sessions_date_index");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AfkTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("afk_time");
            entity.Property(e => e.Deaths)
                .HasColumnType("int(11)")
                .HasColumnName("deaths");
            entity.Property(e => e.JoinAddressId)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)")
                .HasColumnName("join_address_id");
            entity.Property(e => e.MobKills)
                .HasColumnType("int(11)")
                .HasColumnName("mob_kills");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.SessionEnd)
                .HasColumnType("bigint(20)")
                .HasColumnName("session_end");
            entity.Property(e => e.SessionStart)
                .HasColumnType("bigint(20)")
                .HasColumnName("session_start");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanSessions)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_sessions_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.PlanSessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_sessions_ibfk_1");
        });

        modelBuilder.Entity<PlanSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_settings");

            entity.HasIndex(e => e.ServerUuid, "server_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Content)
                .HasColumnType("text")
                .HasColumnName("content");
            entity.Property(e => e.ServerUuid)
                .HasMaxLength(39)
                .HasColumnName("server_uuid");
            entity.Property(e => e.Updated)
                .HasColumnType("bigint(20)")
                .HasColumnName("updated");
        });

        modelBuilder.Entity<PlanTp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_tps");

            entity.HasIndex(e => e.Date, "plan_tps_date_index");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ChunksLoaded)
                .HasColumnType("int(11)")
                .HasColumnName("chunks_loaded");
            entity.Property(e => e.CpuUsage).HasColumnName("cpu_usage");
            entity.Property(e => e.Date)
                .HasColumnType("bigint(20)")
                .HasColumnName("date");
            entity.Property(e => e.Entities)
                .HasColumnType("int(11)")
                .HasColumnName("entities");
            entity.Property(e => e.FreeDiskSpace)
                .HasColumnType("bigint(20)")
                .HasColumnName("free_disk_space");
            entity.Property(e => e.PlayersOnline)
                .HasColumnType("int(11)")
                .HasColumnName("players_online");
            entity.Property(e => e.RamUsage)
                .HasColumnType("bigint(20)")
                .HasColumnName("ram_usage");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.Tps).HasColumnName("tps");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanTps)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_tps_ibfk_1");
        });

        modelBuilder.Entity<PlanUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_users");

            entity.HasIndex(e => e.Uuid, "plan_users_uuid_index").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
            entity.Property(e => e.Registered)
                .HasColumnType("bigint(20)")
                .HasColumnName("registered");
            entity.Property(e => e.TimesKicked)
                .HasColumnType("int(11)")
                .HasColumnName("times_kicked");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<PlanUserInfo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_user_info");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Banned).HasColumnName("banned");
            entity.Property(e => e.JoinAddress)
                .HasMaxLength(191)
                .HasColumnName("join_address");
            entity.Property(e => e.Opped).HasColumnName("opped");
            entity.Property(e => e.Registered)
                .HasColumnType("bigint(20)")
                .HasColumnName("registered");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanUserInfos)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_user_info_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.PlanUserInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_user_info_ibfk_1");
        });

        modelBuilder.Entity<PlanVersionProtocol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_version_protocol");

            entity.HasIndex(e => e.Uuid, "uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ProtocolVersion)
                .HasColumnType("int(11)")
                .HasColumnName("protocol_version");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<PlanWebGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_web_group");

            entity.HasIndex(e => e.GroupName, "group_name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.GroupName)
                .HasMaxLength(100)
                .HasColumnName("group_name");
        });

        modelBuilder.Entity<PlanWebGroupToPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_web_group_to_permission");

            entity.HasIndex(e => e.GroupId, "group_id");

            entity.HasIndex(e => e.PermissionId, "permission_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.GroupId)
                .HasColumnType("int(11)")
                .HasColumnName("group_id");
            entity.Property(e => e.PermissionId)
                .HasColumnType("int(11)")
                .HasColumnName("permission_id");

            entity.HasOne(d => d.Group).WithMany(p => p.PlanWebGroupToPermissions)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_web_group_to_permission_ibfk_1");

            entity.HasOne(d => d.Permission).WithMany(p => p.PlanWebGroupToPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_web_group_to_permission_ibfk_2");
        });

        modelBuilder.Entity<PlanWebPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_web_permission");

            entity.HasIndex(e => e.Permission, "permission").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Permission)
                .HasMaxLength(100)
                .HasColumnName("permission");
        });

        modelBuilder.Entity<PlanWebUserPreference>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_web_user_preferences");

            entity.HasIndex(e => e.WebUserId, "web_user_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Preferences)
                .HasColumnType("text")
                .HasColumnName("preferences");
            entity.Property(e => e.WebUserId)
                .HasColumnType("int(11)")
                .HasColumnName("web_user_id");

            entity.HasOne(d => d.WebUser).WithMany(p => p.PlanWebUserPreferences)
                .HasForeignKey(d => d.WebUserId)
                .HasConstraintName("plan_web_user_preferences_ibfk_1");
        });

        modelBuilder.Entity<PlanWorld>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_worlds");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ServerUuid)
                .HasMaxLength(36)
                .HasColumnName("server_uuid");
            entity.Property(e => e.WorldName)
                .HasMaxLength(100)
                .HasColumnName("world_name");
        });

        modelBuilder.Entity<PlanWorldTime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("plan_world_times");

            entity.HasIndex(e => e.ServerId, "server_id");

            entity.HasIndex(e => e.SessionId, "session_id");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.HasIndex(e => e.WorldId, "world_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AdventureTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("adventure_time");
            entity.Property(e => e.CreativeTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("creative_time");
            entity.Property(e => e.ServerId)
                .HasColumnType("int(11)")
                .HasColumnName("server_id");
            entity.Property(e => e.SessionId)
                .HasColumnType("int(11)")
                .HasColumnName("session_id");
            entity.Property(e => e.SpectatorTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("spectator_time");
            entity.Property(e => e.SurvivalTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("survival_time");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.WorldId)
                .HasColumnType("int(11)")
                .HasColumnName("world_id");

            entity.HasOne(d => d.Server).WithMany(p => p.PlanWorldTimes)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_world_times_ibfk_4");

            entity.HasOne(d => d.Session).WithMany(p => p.PlanWorldTimes)
                .HasForeignKey(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_world_times_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.PlanWorldTimes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_world_times_ibfk_3");

            entity.HasOne(d => d.World).WithMany(p => p.PlanWorldTimes)
                .HasForeignKey(d => d.WorldId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("plan_world_times_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
