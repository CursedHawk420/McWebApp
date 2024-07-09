using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Highgeek.McWebApp.Common.Models.mcserver_datadb;
using Microsoft.Extensions.Configuration;
using Highgeek.McWebApp.Common.Helpers;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverDatadbContext : DbContext
{
    private Helpers.ConfigProvider manager = Helpers.ConfigProvider.Instance;

    private protected string ConnectionString;

    public McserverDatadbContext()
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
    }

    public McserverDatadbContext(DbContextOptions<McserverDatadbContext> options)
        : base(options)
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection_mcserver_datadb");
    }

    public virtual DbSet<Atm9CmiInventory> Atm9CmiInventories { get; set; }

    public virtual DbSet<Atm9CmiPlaytime> Atm9CmiPlaytimes { get; set; }

    public virtual DbSet<Atm9CmiPlaytimereward> Atm9CmiPlaytimerewards { get; set; }

    public virtual DbSet<Atm9CmiUser> Atm9CmiUsers { get; set; }

    public virtual DbSet<Atm9MpdbEnderchest> Atm9MpdbEnderchests { get; set; }

    public virtual DbSet<Atm9MpdbExperience> Atm9MpdbExperiences { get; set; }

    public virtual DbSet<Atm9MpdbHealthFoodAir> Atm9MpdbHealthFoodAirs { get; set; }

    public virtual DbSet<Atm9MpdbInventory> Atm9MpdbInventories { get; set; }

    public virtual DbSet<Atm9MpdbPotionEffect> Atm9MpdbPotionEffects { get; set; }

    public virtual DbSet<Build1CmiInventory> Build1CmiInventories { get; set; }

    public virtual DbSet<Build1CmiPlaytime> Build1CmiPlaytimes { get; set; }

    public virtual DbSet<Build1CmiPlaytimereward> Build1CmiPlaytimerewards { get; set; }

    public virtual DbSet<Build1CmiUser> Build1CmiUsers { get; set; }

    public virtual DbSet<Build1MpdbEnderchest> Build1MpdbEnderchests { get; set; }

    public virtual DbSet<Build1MpdbExperience> Build1MpdbExperiences { get; set; }

    public virtual DbSet<Build1MpdbHealthFoodAir> Build1MpdbHealthFoodAirs { get; set; }

    public virtual DbSet<Build1MpdbInventory> Build1MpdbInventories { get; set; }

    public virtual DbSet<Build1MpdbPotionEffect> Build1MpdbPotionEffects { get; set; }

    public virtual DbSet<KidsCmiInventory> KidsCmiInventories { get; set; }

    public virtual DbSet<KidsCmiPlaytime> KidsCmiPlaytimes { get; set; }

    public virtual DbSet<KidsCmiPlaytimereward> KidsCmiPlaytimerewards { get; set; }

    public virtual DbSet<KidsCmiUser> KidsCmiUsers { get; set; }

    public virtual DbSet<MpdbEconomy> MpdbEconomies { get; set; }

    public virtual DbSet<Survivaltest1CmiInventory> Survivaltest1CmiInventories { get; set; }

    public virtual DbSet<Survivaltest1CmiPlaytime> Survivaltest1CmiPlaytimes { get; set; }

    public virtual DbSet<Survivaltest1CmiPlaytimereward> Survivaltest1CmiPlaytimerewards { get; set; }

    public virtual DbSet<Survivaltest1CmiUser> Survivaltest1CmiUsers { get; set; }

    public virtual DbSet<Survivaltest1MpdbEnderchest> Survivaltest1MpdbEnderchests { get; set; }

    public virtual DbSet<Survivaltest1MpdbExperience> Survivaltest1MpdbExperiences { get; set; }

    public virtual DbSet<Survivaltest1MpdbHealthFoodAir> Survivaltest1MpdbHealthFoodAirs { get; set; }

    public virtual DbSet<Survivaltest1MpdbInventory> Survivaltest1MpdbInventories { get; set; }

    public virtual DbSet<Survivaltest1MpdbPotionEffect> Survivaltest1MpdbPotionEffects { get; set; }

    public virtual DbSet<Test1CmiInventory> Test1CmiInventories { get; set; }

    public virtual DbSet<Test1CmiPlaytime> Test1CmiPlaytimes { get; set; }

    public virtual DbSet<Test1CmiPlaytimereward> Test1CmiPlaytimerewards { get; set; }

    public virtual DbSet<Test1CmiUser> Test1CmiUsers { get; set; }

    public virtual DbSet<Test1MpdbEnderchest> Test1MpdbEnderchests { get; set; }

    public virtual DbSet<Test1MpdbExperience> Test1MpdbExperiences { get; set; }

    public virtual DbSet<Test1MpdbHealthFoodAir> Test1MpdbHealthFoodAirs { get; set; }

    public virtual DbSet<Test1MpdbInventory> Test1MpdbInventories { get; set; }

    public virtual DbSet<Test1MpdbPotionEffect> Test1MpdbPotionEffects { get; set; }

    public virtual DbSet<Playerdatum> Playerdata { get; set; }

    public virtual DbSet<Syncredisdatum> Syncredisdata { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Atm9CmiInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_cmi_inventories");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Inventories).HasColumnName("inventories");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Atm9CmiPlaytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_cmi_playtime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("int(11)")
                .HasColumnName("date");
            entity.Property(e => e.H0)
                .HasColumnType("bigint(20)")
                .HasColumnName("h0");
            entity.Property(e => e.H1)
                .HasColumnType("bigint(20)")
                .HasColumnName("h1");
            entity.Property(e => e.H10)
                .HasColumnType("bigint(20)")
                .HasColumnName("h10");
            entity.Property(e => e.H11)
                .HasColumnType("bigint(20)")
                .HasColumnName("h11");
            entity.Property(e => e.H12)
                .HasColumnType("bigint(20)")
                .HasColumnName("h12");
            entity.Property(e => e.H13)
                .HasColumnType("bigint(20)")
                .HasColumnName("h13");
            entity.Property(e => e.H14)
                .HasColumnType("bigint(20)")
                .HasColumnName("h14");
            entity.Property(e => e.H15)
                .HasColumnType("bigint(20)")
                .HasColumnName("h15");
            entity.Property(e => e.H16)
                .HasColumnType("bigint(20)")
                .HasColumnName("h16");
            entity.Property(e => e.H17)
                .HasColumnType("bigint(20)")
                .HasColumnName("h17");
            entity.Property(e => e.H18)
                .HasColumnType("bigint(20)")
                .HasColumnName("h18");
            entity.Property(e => e.H19)
                .HasColumnType("bigint(20)")
                .HasColumnName("h19");
            entity.Property(e => e.H2)
                .HasColumnType("bigint(20)")
                .HasColumnName("h2");
            entity.Property(e => e.H20)
                .HasColumnType("bigint(20)")
                .HasColumnName("h20");
            entity.Property(e => e.H21)
                .HasColumnType("bigint(20)")
                .HasColumnName("h21");
            entity.Property(e => e.H22)
                .HasColumnType("bigint(20)")
                .HasColumnName("h22");
            entity.Property(e => e.H23)
                .HasColumnType("bigint(20)")
                .HasColumnName("h23");
            entity.Property(e => e.H3)
                .HasColumnType("bigint(20)")
                .HasColumnName("h3");
            entity.Property(e => e.H4)
                .HasColumnType("bigint(20)")
                .HasColumnName("h4");
            entity.Property(e => e.H5)
                .HasColumnType("bigint(20)")
                .HasColumnName("h5");
            entity.Property(e => e.H6)
                .HasColumnType("bigint(20)")
                .HasColumnName("h6");
            entity.Property(e => e.H7)
                .HasColumnType("bigint(20)")
                .HasColumnName("h7");
            entity.Property(e => e.H8)
                .HasColumnType("bigint(20)")
                .HasColumnName("h8");
            entity.Property(e => e.H9)
                .HasColumnType("bigint(20)")
                .HasColumnName("h9");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Atm9CmiPlaytimereward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_cmi_playtimereward");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Onetime).HasColumnName("onetime");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
            entity.Property(e => e.Repeatable)
                .HasColumnType("text")
                .HasColumnName("repeatable");
        });

        modelBuilder.Entity<Atm9CmiUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_cmi_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Afrecharge)
                .HasColumnType("text")
                .HasColumnName("AFRecharge");
            entity.Property(e => e.AlertReason).HasColumnType("text");
            entity.Property(e => e.AlertUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.BanReason).HasColumnType("text");
            entity.Property(e => e.BannedAt).HasColumnType("bigint(20)");
            entity.Property(e => e.BannedBy).HasColumnType("text");
            entity.Property(e => e.BannedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Charges).HasColumnType("text");
            entity.Property(e => e.ChatColor).HasColumnType("text");
            entity.Property(e => e.Cooldowns).HasColumnType("mediumtext");
            entity.Property(e => e.DeathLocation).HasColumnType("text");
            entity.Property(e => e.DisplayName).HasColumnType("text");
            entity.Property(e => e.Economy).HasColumnType("text");
            entity.Property(e => e.FlightChargeEnabled).HasColumnName("flightChargeEnabled");
            entity.Property(e => e.Glow).HasColumnType("text");
            entity.Property(e => e.Homes).HasColumnType("text");
            entity.Property(e => e.Ignores).HasColumnType("text");
            entity.Property(e => e.Ips).HasColumnType("text");
            entity.Property(e => e.Jail).HasColumnType("text");
            entity.Property(e => e.JailReason).HasColumnType("text");
            entity.Property(e => e.JailedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Kits).HasColumnType("text");
            entity.Property(e => e.LastLoginTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LastLogoffTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LockedIps).HasColumnType("text");
            entity.Property(e => e.LogOutLocation).HasColumnType("text");
            entity.Property(e => e.Mail).HasColumnType("mediumtext");
            entity.Property(e => e.Muted).HasColumnType("text");
            entity.Property(e => e.NameColor).HasColumnType("text");
            entity.Property(e => e.NamePrefix).HasColumnType("text");
            entity.Property(e => e.NameSuffix).HasColumnType("text");
            entity.Property(e => e.Nickname)
                .HasColumnType("text")
                .HasColumnName("nickname");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Options).HasColumnType("text");
            entity.Property(e => e.PTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("pTime");
            entity.Property(e => e.PlayerUuid)
                .HasColumnType("text")
                .HasColumnName("player_uuid");
            entity.Property(e => e.PlaytimeOptimized).HasColumnType("bigint(20)");
            entity.Property(e => e.Rank).HasColumnType("text");
            entity.Property(e => e.Skin).HasColumnType("text");
            entity.Property(e => e.TFly)
                .HasColumnType("bigint(20)")
                .HasColumnName("tFly");
            entity.Property(e => e.TGod)
                .HasColumnType("bigint(20)")
                .HasColumnName("tGod");
            entity.Property(e => e.TeleportLocation).HasColumnType("text");
            entity.Property(e => e.TotalPlayTime).HasColumnType("bigint(20)");
            entity.Property(e => e.UserMeta).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasColumnType("text")
                .HasColumnName("username");
            entity.Property(e => e.Vanish).HasColumnType("text");
            entity.Property(e => e.Votifier).HasColumnType("int(11)");
            entity.Property(e => e.Warnings).HasColumnType("text");
        });

        modelBuilder.Entity<Atm9MpdbEnderchest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_mpdb_enderchest");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Enderchest).HasColumnName("enderchest");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Atm9MpdbExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_mpdb_experience");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Exp)
                .HasColumnType("float(60,30)")
                .HasColumnName("exp");
            entity.Property(e => e.ExpLvl)
                .HasColumnType("int(10)")
                .HasColumnName("exp_lvl");
            entity.Property(e => e.ExpToLevel)
                .HasColumnType("int(10)")
                .HasColumnName("exp_to_level");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
            entity.Property(e => e.TotalExp)
                .HasColumnType("int(10)")
                .HasColumnName("total_exp");
        });

        modelBuilder.Entity<Atm9MpdbHealthFoodAir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_mpdb_health_food_air");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Air)
                .HasColumnType("int(10)")
                .HasColumnName("air");
            entity.Property(e => e.Food)
                .HasColumnType("int(10)")
                .HasColumnName("food");
            entity.Property(e => e.Health)
                .HasColumnType("double(10,2)")
                .HasColumnName("health");
            entity.Property(e => e.HealthScale)
                .HasColumnType("double(10,2)")
                .HasColumnName("health_scale");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.MaxAir)
                .HasColumnType("int(10)")
                .HasColumnName("max_air");
            entity.Property(e => e.MaxHealth)
                .HasColumnType("double(10,2)")
                .HasColumnName("max_health");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.Saturation)
                .HasMaxLength(20)
                .HasColumnName("saturation");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Atm9MpdbInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_mpdb_inventory");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Armor)
                .HasColumnType("text")
                .HasColumnName("armor");
            entity.Property(e => e.Gamemode)
                .HasColumnType("int(1)")
                .HasColumnName("gamemode");
            entity.Property(e => e.HotbarSlot)
                .HasColumnType("int(2)")
                .HasColumnName("hotbar_slot");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Atm9MpdbPotionEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("atm9_mpdb_potionEffects");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.PotionEffects)
                .HasColumnType("text")
                .HasColumnName("potion_effects");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Build1CmiInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_cmi_inventories");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Inventories).HasColumnName("inventories");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Build1CmiPlaytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_cmi_playtime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("int(11)")
                .HasColumnName("date");
            entity.Property(e => e.H0)
                .HasColumnType("bigint(20)")
                .HasColumnName("h0");
            entity.Property(e => e.H1)
                .HasColumnType("bigint(20)")
                .HasColumnName("h1");
            entity.Property(e => e.H10)
                .HasColumnType("bigint(20)")
                .HasColumnName("h10");
            entity.Property(e => e.H11)
                .HasColumnType("bigint(20)")
                .HasColumnName("h11");
            entity.Property(e => e.H12)
                .HasColumnType("bigint(20)")
                .HasColumnName("h12");
            entity.Property(e => e.H13)
                .HasColumnType("bigint(20)")
                .HasColumnName("h13");
            entity.Property(e => e.H14)
                .HasColumnType("bigint(20)")
                .HasColumnName("h14");
            entity.Property(e => e.H15)
                .HasColumnType("bigint(20)")
                .HasColumnName("h15");
            entity.Property(e => e.H16)
                .HasColumnType("bigint(20)")
                .HasColumnName("h16");
            entity.Property(e => e.H17)
                .HasColumnType("bigint(20)")
                .HasColumnName("h17");
            entity.Property(e => e.H18)
                .HasColumnType("bigint(20)")
                .HasColumnName("h18");
            entity.Property(e => e.H19)
                .HasColumnType("bigint(20)")
                .HasColumnName("h19");
            entity.Property(e => e.H2)
                .HasColumnType("bigint(20)")
                .HasColumnName("h2");
            entity.Property(e => e.H20)
                .HasColumnType("bigint(20)")
                .HasColumnName("h20");
            entity.Property(e => e.H21)
                .HasColumnType("bigint(20)")
                .HasColumnName("h21");
            entity.Property(e => e.H22)
                .HasColumnType("bigint(20)")
                .HasColumnName("h22");
            entity.Property(e => e.H23)
                .HasColumnType("bigint(20)")
                .HasColumnName("h23");
            entity.Property(e => e.H3)
                .HasColumnType("bigint(20)")
                .HasColumnName("h3");
            entity.Property(e => e.H4)
                .HasColumnType("bigint(20)")
                .HasColumnName("h4");
            entity.Property(e => e.H5)
                .HasColumnType("bigint(20)")
                .HasColumnName("h5");
            entity.Property(e => e.H6)
                .HasColumnType("bigint(20)")
                .HasColumnName("h6");
            entity.Property(e => e.H7)
                .HasColumnType("bigint(20)")
                .HasColumnName("h7");
            entity.Property(e => e.H8)
                .HasColumnType("bigint(20)")
                .HasColumnName("h8");
            entity.Property(e => e.H9)
                .HasColumnType("bigint(20)")
                .HasColumnName("h9");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Build1CmiPlaytimereward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_cmi_playtimereward");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Onetime).HasColumnName("onetime");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
            entity.Property(e => e.Repeatable)
                .HasColumnType("text")
                .HasColumnName("repeatable");
        });

        modelBuilder.Entity<Build1CmiUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_cmi_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Afrecharge)
                .HasColumnType("text")
                .HasColumnName("AFRecharge");
            entity.Property(e => e.AlertReason).HasColumnType("text");
            entity.Property(e => e.AlertUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.BanReason).HasColumnType("text");
            entity.Property(e => e.BannedAt).HasColumnType("bigint(20)");
            entity.Property(e => e.BannedBy).HasColumnType("text");
            entity.Property(e => e.BannedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Charges).HasColumnType("text");
            entity.Property(e => e.ChatColor).HasColumnType("text");
            entity.Property(e => e.Cooldowns).HasColumnType("mediumtext");
            entity.Property(e => e.DeathLocation).HasColumnType("text");
            entity.Property(e => e.DisplayName).HasColumnType("text");
            entity.Property(e => e.Economy).HasColumnType("text");
            entity.Property(e => e.FlightChargeEnabled).HasColumnName("flightChargeEnabled");
            entity.Property(e => e.Glow).HasColumnType("text");
            entity.Property(e => e.Homes).HasColumnType("text");
            entity.Property(e => e.Ignores).HasColumnType("text");
            entity.Property(e => e.Ips).HasColumnType("text");
            entity.Property(e => e.Jail).HasColumnType("text");
            entity.Property(e => e.JailReason).HasColumnType("text");
            entity.Property(e => e.JailedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Kits).HasColumnType("text");
            entity.Property(e => e.LastLoginTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LastLogoffTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LockedIps).HasColumnType("text");
            entity.Property(e => e.LogOutLocation).HasColumnType("text");
            entity.Property(e => e.Mail).HasColumnType("mediumtext");
            entity.Property(e => e.Muted).HasColumnType("text");
            entity.Property(e => e.NameColor).HasColumnType("text");
            entity.Property(e => e.NamePrefix).HasColumnType("text");
            entity.Property(e => e.NameSuffix).HasColumnType("text");
            entity.Property(e => e.Nickname)
                .HasColumnType("text")
                .HasColumnName("nickname");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Options).HasColumnType("text");
            entity.Property(e => e.PTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("pTime");
            entity.Property(e => e.PlayerUuid)
                .HasColumnType("text")
                .HasColumnName("player_uuid");
            entity.Property(e => e.PlaytimeOptimized).HasColumnType("bigint(20)");
            entity.Property(e => e.Rank).HasColumnType("text");
            entity.Property(e => e.Skin).HasColumnType("text");
            entity.Property(e => e.TFly)
                .HasColumnType("bigint(20)")
                .HasColumnName("tFly");
            entity.Property(e => e.TGod)
                .HasColumnType("bigint(20)")
                .HasColumnName("tGod");
            entity.Property(e => e.TeleportLocation).HasColumnType("text");
            entity.Property(e => e.TotalPlayTime).HasColumnType("bigint(20)");
            entity.Property(e => e.UserMeta).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasColumnType("text")
                .HasColumnName("username");
            entity.Property(e => e.Vanish).HasColumnType("text");
            entity.Property(e => e.Votifier).HasColumnType("int(11)");
            entity.Property(e => e.Warnings).HasColumnType("text");
        });

        modelBuilder.Entity<Build1MpdbEnderchest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_mpdb_enderchest");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Enderchest).HasColumnName("enderchest");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Build1MpdbExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_mpdb_experience");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Exp)
                .HasColumnType("float(60,30)")
                .HasColumnName("exp");
            entity.Property(e => e.ExpLvl)
                .HasColumnType("int(10)")
                .HasColumnName("exp_lvl");
            entity.Property(e => e.ExpToLevel)
                .HasColumnType("int(10)")
                .HasColumnName("exp_to_level");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
            entity.Property(e => e.TotalExp)
                .HasColumnType("int(10)")
                .HasColumnName("total_exp");
        });

        modelBuilder.Entity<Build1MpdbHealthFoodAir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_mpdb_health_food_air");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Air)
                .HasColumnType("int(10)")
                .HasColumnName("air");
            entity.Property(e => e.Food)
                .HasColumnType("int(10)")
                .HasColumnName("food");
            entity.Property(e => e.Health)
                .HasColumnType("double(10,2)")
                .HasColumnName("health");
            entity.Property(e => e.HealthScale)
                .HasColumnType("double(10,2)")
                .HasColumnName("health_scale");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.MaxAir)
                .HasColumnType("int(10)")
                .HasColumnName("max_air");
            entity.Property(e => e.MaxHealth)
                .HasColumnType("double(10,2)")
                .HasColumnName("max_health");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.Saturation)
                .HasMaxLength(20)
                .HasColumnName("saturation");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Build1MpdbInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_mpdb_inventory");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Armor)
                .HasColumnType("text")
                .HasColumnName("armor");
            entity.Property(e => e.Gamemode)
                .HasColumnType("int(1)")
                .HasColumnName("gamemode");
            entity.Property(e => e.HotbarSlot)
                .HasColumnType("int(2)")
                .HasColumnName("hotbar_slot");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Build1MpdbPotionEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("build1_mpdb_potionEffects");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.PotionEffects)
                .HasColumnType("text")
                .HasColumnName("potion_effects");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<KidsCmiInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kids_cmi_inventories");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Inventories).HasColumnName("inventories");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<KidsCmiPlaytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kids_cmi_playtime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("int(11)")
                .HasColumnName("date");
            entity.Property(e => e.H0)
                .HasColumnType("bigint(20)")
                .HasColumnName("h0");
            entity.Property(e => e.H1)
                .HasColumnType("bigint(20)")
                .HasColumnName("h1");
            entity.Property(e => e.H10)
                .HasColumnType("bigint(20)")
                .HasColumnName("h10");
            entity.Property(e => e.H11)
                .HasColumnType("bigint(20)")
                .HasColumnName("h11");
            entity.Property(e => e.H12)
                .HasColumnType("bigint(20)")
                .HasColumnName("h12");
            entity.Property(e => e.H13)
                .HasColumnType("bigint(20)")
                .HasColumnName("h13");
            entity.Property(e => e.H14)
                .HasColumnType("bigint(20)")
                .HasColumnName("h14");
            entity.Property(e => e.H15)
                .HasColumnType("bigint(20)")
                .HasColumnName("h15");
            entity.Property(e => e.H16)
                .HasColumnType("bigint(20)")
                .HasColumnName("h16");
            entity.Property(e => e.H17)
                .HasColumnType("bigint(20)")
                .HasColumnName("h17");
            entity.Property(e => e.H18)
                .HasColumnType("bigint(20)")
                .HasColumnName("h18");
            entity.Property(e => e.H19)
                .HasColumnType("bigint(20)")
                .HasColumnName("h19");
            entity.Property(e => e.H2)
                .HasColumnType("bigint(20)")
                .HasColumnName("h2");
            entity.Property(e => e.H20)
                .HasColumnType("bigint(20)")
                .HasColumnName("h20");
            entity.Property(e => e.H21)
                .HasColumnType("bigint(20)")
                .HasColumnName("h21");
            entity.Property(e => e.H22)
                .HasColumnType("bigint(20)")
                .HasColumnName("h22");
            entity.Property(e => e.H23)
                .HasColumnType("bigint(20)")
                .HasColumnName("h23");
            entity.Property(e => e.H3)
                .HasColumnType("bigint(20)")
                .HasColumnName("h3");
            entity.Property(e => e.H4)
                .HasColumnType("bigint(20)")
                .HasColumnName("h4");
            entity.Property(e => e.H5)
                .HasColumnType("bigint(20)")
                .HasColumnName("h5");
            entity.Property(e => e.H6)
                .HasColumnType("bigint(20)")
                .HasColumnName("h6");
            entity.Property(e => e.H7)
                .HasColumnType("bigint(20)")
                .HasColumnName("h7");
            entity.Property(e => e.H8)
                .HasColumnType("bigint(20)")
                .HasColumnName("h8");
            entity.Property(e => e.H9)
                .HasColumnType("bigint(20)")
                .HasColumnName("h9");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<KidsCmiPlaytimereward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kids_cmi_playtimereward");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Onetime).HasColumnName("onetime");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
            entity.Property(e => e.Repeatable)
                .HasColumnType("text")
                .HasColumnName("repeatable");
        });

        modelBuilder.Entity<KidsCmiUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("kids_cmi_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Afrecharge)
                .HasColumnType("text")
                .HasColumnName("AFRecharge");
            entity.Property(e => e.AlertReason).HasColumnType("text");
            entity.Property(e => e.AlertUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.BanReason).HasColumnType("text");
            entity.Property(e => e.BannedAt).HasColumnType("bigint(20)");
            entity.Property(e => e.BannedBy).HasColumnType("text");
            entity.Property(e => e.BannedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Charges).HasColumnType("text");
            entity.Property(e => e.ChatColor).HasColumnType("text");
            entity.Property(e => e.Cooldowns).HasColumnType("mediumtext");
            entity.Property(e => e.DeathLocation).HasColumnType("text");
            entity.Property(e => e.DisplayName).HasColumnType("text");
            entity.Property(e => e.Economy).HasColumnType("text");
            entity.Property(e => e.FlightChargeEnabled).HasColumnName("flightChargeEnabled");
            entity.Property(e => e.Glow).HasColumnType("text");
            entity.Property(e => e.Homes).HasColumnType("text");
            entity.Property(e => e.Ignores).HasColumnType("text");
            entity.Property(e => e.Ips).HasColumnType("text");
            entity.Property(e => e.Jail).HasColumnType("text");
            entity.Property(e => e.JailReason).HasColumnType("text");
            entity.Property(e => e.JailedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Kits).HasColumnType("text");
            entity.Property(e => e.LastLoginTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LastLogoffTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LockedIps).HasColumnType("text");
            entity.Property(e => e.LogOutLocation).HasColumnType("text");
            entity.Property(e => e.Mail).HasColumnType("mediumtext");
            entity.Property(e => e.Muted).HasColumnType("text");
            entity.Property(e => e.NameColor).HasColumnType("text");
            entity.Property(e => e.NamePrefix).HasColumnType("text");
            entity.Property(e => e.NameSuffix).HasColumnType("text");
            entity.Property(e => e.Nickname)
                .HasColumnType("text")
                .HasColumnName("nickname");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Options).HasColumnType("text");
            entity.Property(e => e.PTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("pTime");
            entity.Property(e => e.PlayerUuid)
                .HasColumnType("text")
                .HasColumnName("player_uuid");
            entity.Property(e => e.PlaytimeOptimized).HasColumnType("bigint(20)");
            entity.Property(e => e.Rank).HasColumnType("text");
            entity.Property(e => e.Skin).HasColumnType("text");
            entity.Property(e => e.TFly)
                .HasColumnType("bigint(20)")
                .HasColumnName("tFly");
            entity.Property(e => e.TGod)
                .HasColumnType("bigint(20)")
                .HasColumnName("tGod");
            entity.Property(e => e.TeleportLocation).HasColumnType("text");
            entity.Property(e => e.TotalPlayTime).HasColumnType("bigint(20)");
            entity.Property(e => e.UserMeta).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasColumnType("text")
                .HasColumnName("username");
            entity.Property(e => e.Vanish).HasColumnType("text");
            entity.Property(e => e.Votifier).HasColumnType("int(11)");
            entity.Property(e => e.Warnings).HasColumnType("text");
        });

        modelBuilder.Entity<MpdbEconomy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mpdb_economy");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.Money)
                .HasColumnType("double(30,2)")
                .HasColumnName("money");
            entity.Property(e => e.OfflineMoney)
                .HasColumnType("double(30,2)")
                .HasColumnName("offline_money");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Survivaltest1CmiInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_cmi_inventories");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Inventories).HasColumnName("inventories");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Survivaltest1CmiPlaytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_cmi_playtime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("int(11)")
                .HasColumnName("date");
            entity.Property(e => e.H0)
                .HasColumnType("bigint(20)")
                .HasColumnName("h0");
            entity.Property(e => e.H1)
                .HasColumnType("bigint(20)")
                .HasColumnName("h1");
            entity.Property(e => e.H10)
                .HasColumnType("bigint(20)")
                .HasColumnName("h10");
            entity.Property(e => e.H11)
                .HasColumnType("bigint(20)")
                .HasColumnName("h11");
            entity.Property(e => e.H12)
                .HasColumnType("bigint(20)")
                .HasColumnName("h12");
            entity.Property(e => e.H13)
                .HasColumnType("bigint(20)")
                .HasColumnName("h13");
            entity.Property(e => e.H14)
                .HasColumnType("bigint(20)")
                .HasColumnName("h14");
            entity.Property(e => e.H15)
                .HasColumnType("bigint(20)")
                .HasColumnName("h15");
            entity.Property(e => e.H16)
                .HasColumnType("bigint(20)")
                .HasColumnName("h16");
            entity.Property(e => e.H17)
                .HasColumnType("bigint(20)")
                .HasColumnName("h17");
            entity.Property(e => e.H18)
                .HasColumnType("bigint(20)")
                .HasColumnName("h18");
            entity.Property(e => e.H19)
                .HasColumnType("bigint(20)")
                .HasColumnName("h19");
            entity.Property(e => e.H2)
                .HasColumnType("bigint(20)")
                .HasColumnName("h2");
            entity.Property(e => e.H20)
                .HasColumnType("bigint(20)")
                .HasColumnName("h20");
            entity.Property(e => e.H21)
                .HasColumnType("bigint(20)")
                .HasColumnName("h21");
            entity.Property(e => e.H22)
                .HasColumnType("bigint(20)")
                .HasColumnName("h22");
            entity.Property(e => e.H23)
                .HasColumnType("bigint(20)")
                .HasColumnName("h23");
            entity.Property(e => e.H3)
                .HasColumnType("bigint(20)")
                .HasColumnName("h3");
            entity.Property(e => e.H4)
                .HasColumnType("bigint(20)")
                .HasColumnName("h4");
            entity.Property(e => e.H5)
                .HasColumnType("bigint(20)")
                .HasColumnName("h5");
            entity.Property(e => e.H6)
                .HasColumnType("bigint(20)")
                .HasColumnName("h6");
            entity.Property(e => e.H7)
                .HasColumnType("bigint(20)")
                .HasColumnName("h7");
            entity.Property(e => e.H8)
                .HasColumnType("bigint(20)")
                .HasColumnName("h8");
            entity.Property(e => e.H9)
                .HasColumnType("bigint(20)")
                .HasColumnName("h9");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Survivaltest1CmiPlaytimereward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_cmi_playtimereward");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Onetime).HasColumnName("onetime");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
            entity.Property(e => e.Repeatable)
                .HasColumnType("text")
                .HasColumnName("repeatable");
        });

        modelBuilder.Entity<Survivaltest1CmiUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_cmi_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Afrecharge)
                .HasColumnType("text")
                .HasColumnName("AFRecharge");
            entity.Property(e => e.AlertReason).HasColumnType("text");
            entity.Property(e => e.AlertUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.BanReason).HasColumnType("text");
            entity.Property(e => e.BannedAt).HasColumnType("bigint(20)");
            entity.Property(e => e.BannedBy).HasColumnType("text");
            entity.Property(e => e.BannedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Charges).HasColumnType("text");
            entity.Property(e => e.ChatColor).HasColumnType("text");
            entity.Property(e => e.Cooldowns).HasColumnType("mediumtext");
            entity.Property(e => e.DeathLocation).HasColumnType("text");
            entity.Property(e => e.DisplayName).HasColumnType("text");
            entity.Property(e => e.Economy).HasColumnType("text");
            entity.Property(e => e.FlightChargeEnabled).HasColumnName("flightChargeEnabled");
            entity.Property(e => e.Glow).HasColumnType("text");
            entity.Property(e => e.Homes).HasColumnType("text");
            entity.Property(e => e.Ignores).HasColumnType("text");
            entity.Property(e => e.Ips).HasColumnType("text");
            entity.Property(e => e.Jail).HasColumnType("text");
            entity.Property(e => e.JailReason).HasColumnType("text");
            entity.Property(e => e.JailedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Kits).HasColumnType("text");
            entity.Property(e => e.LastLoginTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LastLogoffTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LockedIps).HasColumnType("text");
            entity.Property(e => e.LogOutLocation).HasColumnType("text");
            entity.Property(e => e.Mail).HasColumnType("mediumtext");
            entity.Property(e => e.Muted).HasColumnType("text");
            entity.Property(e => e.NameColor).HasColumnType("text");
            entity.Property(e => e.NamePrefix).HasColumnType("text");
            entity.Property(e => e.NameSuffix).HasColumnType("text");
            entity.Property(e => e.Nickname)
                .HasColumnType("text")
                .HasColumnName("nickname");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Options).HasColumnType("text");
            entity.Property(e => e.PTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("pTime");
            entity.Property(e => e.PlayerUuid)
                .HasColumnType("text")
                .HasColumnName("player_uuid");
            entity.Property(e => e.PlaytimeOptimized).HasColumnType("bigint(20)");
            entity.Property(e => e.Rank).HasColumnType("text");
            entity.Property(e => e.Skin).HasColumnType("text");
            entity.Property(e => e.TFly)
                .HasColumnType("bigint(20)")
                .HasColumnName("tFly");
            entity.Property(e => e.TGod)
                .HasColumnType("bigint(20)")
                .HasColumnName("tGod");
            entity.Property(e => e.TeleportLocation).HasColumnType("text");
            entity.Property(e => e.TotalPlayTime).HasColumnType("bigint(20)");
            entity.Property(e => e.UserMeta).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasColumnType("text")
                .HasColumnName("username");
            entity.Property(e => e.Vanish).HasColumnType("text");
            entity.Property(e => e.Votifier).HasColumnType("int(11)");
            entity.Property(e => e.Warnings).HasColumnType("text");
        });

        modelBuilder.Entity<Survivaltest1MpdbEnderchest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_mpdb_enderchest");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Enderchest).HasColumnName("enderchest");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Survivaltest1MpdbExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_mpdb_experience");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Exp)
                .HasColumnType("float(60,30)")
                .HasColumnName("exp");
            entity.Property(e => e.ExpLvl)
                .HasColumnType("int(10)")
                .HasColumnName("exp_lvl");
            entity.Property(e => e.ExpToLevel)
                .HasColumnType("int(10)")
                .HasColumnName("exp_to_level");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
            entity.Property(e => e.TotalExp)
                .HasColumnType("int(10)")
                .HasColumnName("total_exp");
        });

        modelBuilder.Entity<Survivaltest1MpdbHealthFoodAir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_mpdb_health_food_air");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Air)
                .HasColumnType("int(10)")
                .HasColumnName("air");
            entity.Property(e => e.Food)
                .HasColumnType("int(10)")
                .HasColumnName("food");
            entity.Property(e => e.Health)
                .HasColumnType("double(10,2)")
                .HasColumnName("health");
            entity.Property(e => e.HealthScale)
                .HasColumnType("double(10,2)")
                .HasColumnName("health_scale");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.MaxAir)
                .HasColumnType("int(10)")
                .HasColumnName("max_air");
            entity.Property(e => e.MaxHealth)
                .HasColumnType("double(10,2)")
                .HasColumnName("max_health");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.Saturation)
                .HasMaxLength(20)
                .HasColumnName("saturation");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Survivaltest1MpdbInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_mpdb_inventory");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Armor)
                .HasColumnType("text")
                .HasColumnName("armor");
            entity.Property(e => e.Gamemode)
                .HasColumnType("int(1)")
                .HasColumnName("gamemode");
            entity.Property(e => e.HotbarSlot)
                .HasColumnType("int(2)")
                .HasColumnName("hotbar_slot");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Survivaltest1MpdbPotionEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("survivaltest1_mpdb_potionEffects");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.PotionEffects)
                .HasColumnType("text")
                .HasColumnName("potion_effects");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Test1CmiInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_cmi_inventories");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Inventories).HasColumnName("inventories");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Test1CmiPlaytime>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_cmi_playtime");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("int(11)")
                .HasColumnName("date");
            entity.Property(e => e.H0)
                .HasColumnType("bigint(20)")
                .HasColumnName("h0");
            entity.Property(e => e.H1)
                .HasColumnType("bigint(20)")
                .HasColumnName("h1");
            entity.Property(e => e.H10)
                .HasColumnType("bigint(20)")
                .HasColumnName("h10");
            entity.Property(e => e.H11)
                .HasColumnType("bigint(20)")
                .HasColumnName("h11");
            entity.Property(e => e.H12)
                .HasColumnType("bigint(20)")
                .HasColumnName("h12");
            entity.Property(e => e.H13)
                .HasColumnType("bigint(20)")
                .HasColumnName("h13");
            entity.Property(e => e.H14)
                .HasColumnType("bigint(20)")
                .HasColumnName("h14");
            entity.Property(e => e.H15)
                .HasColumnType("bigint(20)")
                .HasColumnName("h15");
            entity.Property(e => e.H16)
                .HasColumnType("bigint(20)")
                .HasColumnName("h16");
            entity.Property(e => e.H17)
                .HasColumnType("bigint(20)")
                .HasColumnName("h17");
            entity.Property(e => e.H18)
                .HasColumnType("bigint(20)")
                .HasColumnName("h18");
            entity.Property(e => e.H19)
                .HasColumnType("bigint(20)")
                .HasColumnName("h19");
            entity.Property(e => e.H2)
                .HasColumnType("bigint(20)")
                .HasColumnName("h2");
            entity.Property(e => e.H20)
                .HasColumnType("bigint(20)")
                .HasColumnName("h20");
            entity.Property(e => e.H21)
                .HasColumnType("bigint(20)")
                .HasColumnName("h21");
            entity.Property(e => e.H22)
                .HasColumnType("bigint(20)")
                .HasColumnName("h22");
            entity.Property(e => e.H23)
                .HasColumnType("bigint(20)")
                .HasColumnName("h23");
            entity.Property(e => e.H3)
                .HasColumnType("bigint(20)")
                .HasColumnName("h3");
            entity.Property(e => e.H4)
                .HasColumnType("bigint(20)")
                .HasColumnName("h4");
            entity.Property(e => e.H5)
                .HasColumnType("bigint(20)")
                .HasColumnName("h5");
            entity.Property(e => e.H6)
                .HasColumnType("bigint(20)")
                .HasColumnName("h6");
            entity.Property(e => e.H7)
                .HasColumnType("bigint(20)")
                .HasColumnName("h7");
            entity.Property(e => e.H8)
                .HasColumnType("bigint(20)")
                .HasColumnName("h8");
            entity.Property(e => e.H9)
                .HasColumnType("bigint(20)")
                .HasColumnName("h9");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
        });

        modelBuilder.Entity<Test1CmiPlaytimereward>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_cmi_playtimereward");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Onetime).HasColumnName("onetime");
            entity.Property(e => e.PlayerId)
                .HasColumnType("int(11)")
                .HasColumnName("player_id");
            entity.Property(e => e.Repeatable)
                .HasColumnType("text")
                .HasColumnName("repeatable");
        });

        modelBuilder.Entity<Test1CmiUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_cmi_users");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Afrecharge)
                .HasColumnType("text")
                .HasColumnName("AFRecharge");
            entity.Property(e => e.AlertReason).HasColumnType("text");
            entity.Property(e => e.AlertUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.BanReason).HasColumnType("text");
            entity.Property(e => e.BannedAt).HasColumnType("bigint(20)");
            entity.Property(e => e.BannedBy).HasColumnType("text");
            entity.Property(e => e.BannedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Charges).HasColumnType("text");
            entity.Property(e => e.ChatColor).HasColumnType("text");
            entity.Property(e => e.Cooldowns).HasColumnType("mediumtext");
            entity.Property(e => e.DeathLocation).HasColumnType("text");
            entity.Property(e => e.DisplayName).HasColumnType("text");
            entity.Property(e => e.Economy).HasColumnType("text");
            entity.Property(e => e.FlightChargeEnabled).HasColumnName("flightChargeEnabled");
            entity.Property(e => e.Glow).HasColumnType("text");
            entity.Property(e => e.Homes).HasColumnType("text");
            entity.Property(e => e.Ignores).HasColumnType("text");
            entity.Property(e => e.Ips).HasColumnType("text");
            entity.Property(e => e.Jail).HasColumnType("text");
            entity.Property(e => e.JailReason).HasColumnType("text");
            entity.Property(e => e.JailedUntil).HasColumnType("bigint(20)");
            entity.Property(e => e.Kits).HasColumnType("text");
            entity.Property(e => e.LastLoginTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LastLogoffTime).HasColumnType("bigint(20)");
            entity.Property(e => e.LockedIps).HasColumnType("text");
            entity.Property(e => e.LogOutLocation).HasColumnType("text");
            entity.Property(e => e.Mail).HasColumnType("mediumtext");
            entity.Property(e => e.Muted).HasColumnType("text");
            entity.Property(e => e.NameColor).HasColumnType("text");
            entity.Property(e => e.NamePrefix).HasColumnType("text");
            entity.Property(e => e.NameSuffix).HasColumnType("text");
            entity.Property(e => e.Nickname)
                .HasColumnType("text")
                .HasColumnName("nickname");
            entity.Property(e => e.Notes).HasColumnType("text");
            entity.Property(e => e.Options).HasColumnType("text");
            entity.Property(e => e.PTime)
                .HasColumnType("bigint(20)")
                .HasColumnName("pTime");
            entity.Property(e => e.PlayerUuid)
                .HasColumnType("text")
                .HasColumnName("player_uuid");
            entity.Property(e => e.PlaytimeOptimized).HasColumnType("bigint(20)");
            entity.Property(e => e.Rank).HasColumnType("text");
            entity.Property(e => e.Skin).HasColumnType("text");
            entity.Property(e => e.TFly)
                .HasColumnType("bigint(20)")
                .HasColumnName("tFly");
            entity.Property(e => e.TGod)
                .HasColumnType("bigint(20)")
                .HasColumnName("tGod");
            entity.Property(e => e.TeleportLocation).HasColumnType("text");
            entity.Property(e => e.TotalPlayTime).HasColumnType("bigint(20)");
            entity.Property(e => e.UserMeta).HasColumnType("text");
            entity.Property(e => e.Username)
                .HasColumnType("text")
                .HasColumnName("username");
            entity.Property(e => e.Vanish).HasColumnType("text");
            entity.Property(e => e.Votifier).HasColumnType("int(11)");
            entity.Property(e => e.Warnings).HasColumnType("text");
        });

        modelBuilder.Entity<Test1MpdbEnderchest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_mpdb_enderchest");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Enderchest).HasColumnName("enderchest");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Test1MpdbExperience>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_mpdb_experience");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Exp)
                .HasColumnType("float(60,30)")
                .HasColumnName("exp");
            entity.Property(e => e.ExpLvl)
                .HasColumnType("int(10)")
                .HasColumnName("exp_lvl");
            entity.Property(e => e.ExpToLevel)
                .HasColumnType("int(10)")
                .HasColumnName("exp_to_level");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
            entity.Property(e => e.TotalExp)
                .HasColumnType("int(10)")
                .HasColumnName("total_exp");
        });

        modelBuilder.Entity<Test1MpdbHealthFoodAir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_mpdb_health_food_air");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Air)
                .HasColumnType("int(10)")
                .HasColumnName("air");
            entity.Property(e => e.Food)
                .HasColumnType("int(10)")
                .HasColumnName("food");
            entity.Property(e => e.Health)
                .HasColumnType("double(10,2)")
                .HasColumnName("health");
            entity.Property(e => e.HealthScale)
                .HasColumnType("double(10,2)")
                .HasColumnName("health_scale");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.MaxAir)
                .HasColumnType("int(10)")
                .HasColumnName("max_air");
            entity.Property(e => e.MaxHealth)
                .HasColumnType("double(10,2)")
                .HasColumnName("max_health");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.Saturation)
                .HasMaxLength(20)
                .HasColumnName("saturation");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Test1MpdbInventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_mpdb_inventory");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Armor)
                .HasColumnType("text")
                .HasColumnName("armor");
            entity.Property(e => e.Gamemode)
                .HasColumnType("int(1)")
                .HasColumnName("gamemode");
            entity.Property(e => e.HotbarSlot)
                .HasColumnType("int(2)")
                .HasColumnName("hotbar_slot");
            entity.Property(e => e.Inventory).HasColumnName("inventory");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Test1MpdbPotionEffect>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("test1_mpdb_potionEffects");

            entity.HasIndex(e => e.PlayerUuid, "player_uuid").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.LastSeen)
                .HasMaxLength(13)
                .IsFixedLength()
                .HasColumnName("last_seen");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name")
                .UseCollation("utf8mb4_unicode_ci");
            entity.Property(e => e.PlayerUuid).HasColumnName("player_uuid");
            entity.Property(e => e.PotionEffects)
                .HasColumnType("text")
                .HasColumnName("potion_effects");
            entity.Property(e => e.SyncComplete)
                .HasMaxLength(5)
                .HasColumnName("sync_complete");
        });

        modelBuilder.Entity<Playerdatum>(entity =>
        {
            entity.HasKey(e => e.PlayerUuid).HasName("PRIMARY");

            entity.ToTable("playerdata");

            entity.Property(e => e.PlayerUuid)
                .HasMaxLength(100)
                .HasColumnName("player_uuid");
            entity.Property(e => e.Advancements).HasColumnName("advancements");
            entity.Property(e => e.Effects).HasColumnName("effects");
            entity.Property(e => e.Enderchest)
                .HasColumnType("text")
                .HasColumnName("enderchest");
            entity.Property(e => e.Exp)
                .HasColumnType("int(255)")
                .HasColumnName("exp");
            entity.Property(e => e.Food)
                .HasColumnType("int(10)")
                .HasColumnName("food");
            entity.Property(e => e.Gamemode)
                .HasMaxLength(18)
                .HasColumnName("gamemode");
            entity.Property(e => e.Health)
                .HasColumnType("int(10)")
                .HasColumnName("health");
            entity.Property(e => e.Inventory)
                .HasColumnType("text")
                .HasColumnName("inventory");
            entity.Property(e => e.LastJoined)
                .HasMaxLength(255)
                .HasColumnName("last_joined");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name");
            entity.Property(e => e.Statistics).HasColumnName("statistics");
        });

        modelBuilder.Entity<Syncredisdatum>(entity =>
        {
            entity.HasKey(e => e.InventoryUuid).HasName("PRIMARY");

            entity.ToTable("syncredisdata");

            entity.Property(e => e.InventoryUuid)
                .HasMaxLength(100)
                .HasColumnName("inventory_uuid");
            entity.Property(e => e.InventoryName)
                .HasMaxLength(100)
                .HasColumnName("inventory_name");
            entity.Property(e => e.Jsondata).HasColumnName("jsondata");
            entity.Property(e => e.LastUpdated)
                .HasMaxLength(255)
                .HasColumnName("last_updated");
            entity.Property(e => e.PlayerName)
                .HasMaxLength(16)
                .HasColumnName("player_name");
            entity.Property(e => e.PlayerUuid)
                .HasMaxLength(100)
                .HasColumnName("player_uuid"); 
            entity.Property(e => e.Size)
                .HasColumnType("int(11)")
                .HasColumnName("size");
            entity.Property(e => e.Web).HasColumnName("web");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
