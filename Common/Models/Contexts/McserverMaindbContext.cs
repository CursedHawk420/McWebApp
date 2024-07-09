using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Models.mcserver_maindb;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McserverMaindbContext : DbContext
{
    private Helpers.ConfigProvider manager = Helpers.ConfigProvider.Instance;

    private protected string ConnectionString;

    public McserverMaindbContext()
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection");
    }

    public McserverMaindbContext(DbContextOptions<McserverMaindbContext> options)
        : base(options)
    {
        ConnectionString = manager.GetConnectionString("MysqlMCServerConnection");
        Database.EnsureCreated();
    }


    public virtual DbSet<Authme> Authmes { get; set; }

    public virtual DbSet<DiscordsrvAccount> DiscordsrvAccounts { get; set; }

    public virtual DbSet<DiscordsrvCode> DiscordsrvCodes { get; set; }

    public virtual DbSet<EfmigrationsHistory> EfmigrationsHistories { get; set; }

    public virtual DbSet<LuckpermsAction> LuckpermsActions { get; set; }

    public virtual DbSet<LuckpermsGroup> LuckpermsGroups { get; set; }

    public virtual DbSet<LuckpermsGroupPermission> LuckpermsGroupPermissions { get; set; }

    public virtual DbSet<LuckpermsMessenger> LuckpermsMessengers { get; set; }

    public virtual DbSet<LuckpermsPlayer> LuckpermsPlayers { get; set; }

    public virtual DbSet<LuckpermsTrack> LuckpermsTracks { get; set; }

    public virtual DbSet<LuckpermsUserPermission> LuckpermsUserPermissions { get; set; }

    public virtual DbSet<Premium> Premia { get; set; }

    public virtual DbSet<SrCache> SrCaches { get; set; }

    public virtual DbSet<SrCustomSkin> SrCustomSkins { get; set; }

    public virtual DbSet<SrPlayer> SrPlayers { get; set; }

    public virtual DbSet<SrPlayerSkin> SrPlayerSkins { get; set; }

    public virtual DbSet<SrUrlIndex> SrUrlIndices { get; set; }

    public virtual DbSet<SrUrlSkin> SrUrlSkins { get; set; }

    public virtual DbSet<VentureChat> VentureChats { get; set; }

    public virtual DbSet<MinecraftUser> WebMinecraftusers { get; set; }

    public virtual DbSet<WebServerstatus> WebServerstatuses { get; set; }

    public virtual DbSet<WebPlayerDatum> WebPlayerData { get; set; }

    public virtual DbSet<Xconomy> Xconomies { get; set; }

    public virtual DbSet<Xconomyrecord> Xconomyrecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Authme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("authme")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("mediumint(8) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.HasSession)
                .HasColumnType("smallint(6)")
                .HasColumnName("hasSession");
            entity.Property(e => e.Ip)
                .HasMaxLength(40)
                .HasColumnName("ip")
                .UseCollation("ascii_bin")
                .HasCharSet("ascii");
            entity.Property(e => e.IsLogged)
                .HasColumnType("smallint(6)")
                .HasColumnName("isLogged");
            entity.Property(e => e.Lastlogin)
                .HasColumnType("bigint(20)")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password")
                .UseCollation("ascii_bin")
                .HasCharSet("ascii");
            entity.Property(e => e.Pitch).HasColumnName("pitch");
            entity.Property(e => e.Realname)
                .HasMaxLength(255)
                .HasColumnName("realname");
            entity.Property(e => e.Regdate)
                .HasColumnType("bigint(20)")
                .HasColumnName("regdate");
            entity.Property(e => e.Regip)
                .HasMaxLength(40)
                .HasColumnName("regip")
                .UseCollation("ascii_bin")
                .HasCharSet("ascii");
            entity.Property(e => e.Totp)
                .HasMaxLength(32)
                .HasColumnName("totp");
            entity.Property(e => e.Username).HasColumnName("username");
            entity.Property(e => e.World)
                .HasMaxLength(255)
                .HasDefaultValueSql("'world'")
                .HasColumnName("world");
            entity.Property(e => e.X).HasColumnName("x");
            entity.Property(e => e.Y).HasColumnName("y");
            entity.Property(e => e.Yaw).HasColumnName("yaw");
            entity.Property(e => e.Z).HasColumnName("z");
        });

        modelBuilder.Entity<DiscordsrvAccount>(entity =>
        {
            entity.HasKey(e => e.Link).HasName("PRIMARY");

            entity.ToTable("discordsrv__accounts");

            entity.HasIndex(e => e.Discord, "accounts_discord_uindex").IsUnique();

            entity.HasIndex(e => e.Uuid, "accounts_uuid_uindex").IsUnique();

            entity.Property(e => e.Link)
                .HasColumnType("int(11)")
                .HasColumnName("link");
            entity.Property(e => e.Discord)
                .HasMaxLength(32)
                .HasColumnName("discord");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<DiscordsrvCode>(entity =>
        {
            entity.HasKey(e => e.Code).HasName("PRIMARY");

            entity.ToTable("discordsrv__codes");

            entity.HasIndex(e => e.Uuid, "codes_uuid_uindex").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(4)
                .IsFixedLength()
                .HasColumnName("code");
            entity.Property(e => e.Expiration)
                .HasColumnType("bigint(20)")
                .HasColumnName("expiration");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<EfmigrationsHistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__EFMigrationsHistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion).HasMaxLength(32);
        });

        modelBuilder.Entity<LuckpermsAction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("luckperms_actions");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.ActedName)
                .HasMaxLength(36)
                .HasColumnName("acted_name");
            entity.Property(e => e.ActedUuid)
                .HasMaxLength(36)
                .HasColumnName("acted_uuid");
            entity.Property(e => e.Action)
                .HasMaxLength(300)
                .HasColumnName("action");
            entity.Property(e => e.ActorName)
                .HasMaxLength(100)
                .HasColumnName("actor_name");
            entity.Property(e => e.ActorUuid)
                .HasMaxLength(36)
                .HasColumnName("actor_uuid");
            entity.Property(e => e.Time)
                .HasColumnType("bigint(20)")
                .HasColumnName("time");
            entity.Property(e => e.Type)
                .HasMaxLength(1)
                .IsFixedLength()
                .HasColumnName("type");
        });

        modelBuilder.Entity<LuckpermsGroup>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("luckperms_groups");

            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
        });

        modelBuilder.Entity<LuckpermsGroupPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("luckperms_group_permissions");

            entity.HasIndex(e => e.Name, "luckperms_group_permissions_name");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Contexts)
                .HasMaxLength(200)
                .HasColumnName("contexts");
            entity.Property(e => e.Expiry)
                .HasColumnType("bigint(20)")
                .HasColumnName("expiry");
            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
            entity.Property(e => e.Permission)
                .HasMaxLength(200)
                .HasColumnName("permission");
            entity.Property(e => e.Server)
                .HasMaxLength(36)
                .HasColumnName("server");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.World)
                .HasMaxLength(64)
                .HasColumnName("world");
        });

        modelBuilder.Entity<LuckpermsMessenger>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("luckperms_messenger");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Msg)
                .HasColumnType("text")
                .HasColumnName("msg");
            entity.Property(e => e.Time)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp")
                .HasColumnName("time");
        });

        modelBuilder.Entity<LuckpermsPlayer>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("luckperms_players");

            entity.HasIndex(e => e.Username, "luckperms_players_username");

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.PrimaryGroup)
                .HasMaxLength(36)
                .HasColumnName("primary_group");
            entity.Property(e => e.Username)
                .HasMaxLength(16)
                .HasColumnName("username");
        });

        modelBuilder.Entity<LuckpermsTrack>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("luckperms_tracks");

            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
            entity.Property(e => e.Groups)
                .HasColumnType("text")
                .HasColumnName("groups");
        });

        modelBuilder.Entity<LuckpermsUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("luckperms_user_permissions");

            entity.HasIndex(e => e.Uuid, "luckperms_user_permissions_uuid");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Contexts)
                .HasMaxLength(200)
                .HasColumnName("contexts");
            entity.Property(e => e.Expiry)
                .HasColumnType("bigint(20)")
                .HasColumnName("expiry");
            entity.Property(e => e.Permission)
                .HasMaxLength(200)
                .HasColumnName("permission");
            entity.Property(e => e.Server)
                .HasMaxLength(36)
                .HasColumnName("server");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.Value).HasColumnName("value");
            entity.Property(e => e.World)
                .HasMaxLength(64)
                .HasColumnName("world");
        });

        modelBuilder.Entity<Premium>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("premium");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("UserID");
            entity.Property(e => e.LastIp).HasMaxLength(255);
            entity.Property(e => e.LastLogin)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("timestamp");
            entity.Property(e => e.Name).HasMaxLength(16);
            entity.Property(e => e.Premium1).HasColumnName("Premium");
            entity.Property(e => e.Uuid)
                .HasMaxLength(37)
                .IsFixedLength()
                .HasColumnName("UUID");
        });

        modelBuilder.Entity<SrCache>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity
                .ToTable("sr_cache")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Name)
                .HasMaxLength(16)
                .HasColumnName("name");
            entity.Property(e => e.Timestamp)
                .HasColumnType("bigint(20)")
                .HasColumnName("timestamp");
            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<SrCustomSkin>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity
                .ToTable("sr_custom_skins")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Name)
                .HasMaxLength(36)
                .HasColumnName("name");
            entity.Property(e => e.Signature)
                .HasColumnType("text")
                .HasColumnName("signature");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");
        });

        modelBuilder.Entity<SrPlayer>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity
                .ToTable("sr_players")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.SkinIdentifier)
                .HasMaxLength(2083)
                .HasColumnName("skin_identifier");
            entity.Property(e => e.SkinType)
                .HasMaxLength(20)
                .HasColumnName("skin_type");
            entity.Property(e => e.SkinVariant)
                .HasMaxLength(20)
                .HasColumnName("skin_variant");
        });

        modelBuilder.Entity<SrPlayerSkin>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity
                .ToTable("sr_player_skins")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Uuid)
                .HasMaxLength(36)
                .HasColumnName("uuid");
            entity.Property(e => e.LastKnownName)
                .HasMaxLength(16)
                .HasColumnName("last_known_name");
            entity.Property(e => e.Signature)
                .HasColumnType("text")
                .HasColumnName("signature");
            entity.Property(e => e.Timestamp)
                .HasColumnType("bigint(20)")
                .HasColumnName("timestamp");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");
        });

        modelBuilder.Entity<SrUrlIndex>(entity =>
        {
            entity.HasKey(e => e.Url).HasName("PRIMARY");

            entity
                .ToTable("sr_url_index")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Url)
                .HasMaxLength(266)
                .HasColumnName("url");
            entity.Property(e => e.SkinVariant)
                .HasMaxLength(20)
                .HasColumnName("skin_variant");
        });

        modelBuilder.Entity<SrUrlSkin>(entity =>
        {
            entity.HasKey(e => e.Url).HasName("PRIMARY");

            entity
                .ToTable("sr_url_skins")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Url)
                .HasMaxLength(266)
                .HasColumnName("url");
            entity.Property(e => e.MineSkinId)
                .HasMaxLength(36)
                .HasColumnName("mine_skin_id");
            entity.Property(e => e.Signature)
                .HasColumnType("text")
                .HasColumnName("signature");
            entity.Property(e => e.SkinVariant)
                .HasMaxLength(20)
                .HasColumnName("skin_variant");
            entity.Property(e => e.Value)
                .HasColumnType("text")
                .HasColumnName("value");
        });

        modelBuilder.Entity<VentureChat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("VentureChat");

            entity.Property(e => e.Id)
                .HasColumnType("bigint(20) unsigned")
                .HasColumnName("ID");
            entity.Property(e => e.Channel).HasColumnType("text");
            entity.Property(e => e.ChatTime).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(36);
            entity.Property(e => e.Server).HasColumnType("text");
            entity.Property(e => e.Text).HasColumnType("text");
            entity.Property(e => e.Type).HasColumnType("text");
            entity.Property(e => e.Uuid)
                .HasColumnType("text")
                .HasColumnName("UUID");
        });

        modelBuilder.Entity<MinecraftUser>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("web_minecraftuser");

            entity.Property(e => e.Uuid).HasMaxLength(36);
            entity.Property(e => e.ApplicationUserEmail).HasColumnType("text");
            entity.Property(e => e.ApplicationUserId).HasColumnType("text");
            entity.Property(e => e.ApplicationUserName).HasColumnType("text");
            entity.Property(e => e.EcoId)
                .HasMaxLength(16)
                .IsFixedLength();
            entity.Property(e => e.IsPremium).HasColumnType("text");
            entity.Property(e => e.LpUserGroup).HasColumnType("text");
            entity.Property(e => e.NickName).HasColumnType("text");
            entity.Property(e => e.PremiumUuid).HasColumnType("text");
            entity.Property(e => e.SkinHeadTexture).HasColumnType("text");
            entity.Property(e => e.SkinTexture).HasColumnType("text");
        });

        modelBuilder.Entity<WebServerstatus>(entity =>
        {
            entity.HasKey(e => e.Name)
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 128 });

            entity.ToTable("web_serverstatus");

            entity.Property(e => e.Name)
                .HasColumnType("longtext")
                .HasColumnName("name");
            entity.Property(e => e.Maxplayers).HasColumnName("maxplayers");
            entity.Property(e => e.Online).HasColumnName("online");
            entity.Property(e => e.Players).HasColumnName("players");
            entity.Property(e => e.PlayersList).HasColumnName("playerslist");
            entity.Property(e => e.PlayersList).HasConversion(
                                v => JsonConvert.SerializeObject(v),
                                v => JsonConvert.DeserializeObject<List<string>>(v));
        });

        modelBuilder.Entity<WebPlayerDatum>(entity =>
        {
            entity.HasKey(e => e.Name)
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 128 });

            entity.ToTable("web_player_data");

            entity.Property(e => e.Name)
                .HasColumnType("text")
                .HasColumnName("name");
            entity.Property(e => e.Plandata).HasColumnName("plandata");
            entity.Property(e => e.Uuid)
                .HasColumnType("text")
                .HasColumnName("uuid");
        });

        modelBuilder.Entity<Xconomy>(entity =>
        {
            entity.HasKey(e => e.Uid).HasName("PRIMARY");

            entity
                .ToTable("xconomy")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("UID");
            entity.Property(e => e.Balance)
                .HasColumnType("double(20,2)")
                .HasColumnName("balance");
            entity.Property(e => e.Hidden)
                .HasColumnType("int(5)")
                .HasColumnName("hidden");
            entity.Property(e => e.Player)
                .HasMaxLength(50)
                .HasColumnName("player");
        });

        modelBuilder.Entity<Xconomyrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("xconomyrecord")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .HasColumnType("int(20)")
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("double(20,2)")
                .HasColumnName("amount");
            entity.Property(e => e.Balance)
                .HasColumnType("double(20,2)")
                .HasColumnName("balance");
            entity.Property(e => e.Command)
                .HasMaxLength(255)
                .HasColumnName("command");
            entity.Property(e => e.Comment)
                .HasMaxLength(255)
                .HasColumnName("comment");
            entity.Property(e => e.Datetime)
                .HasColumnType("datetime")
                .HasColumnName("datetime");
            entity.Property(e => e.Operation)
                .HasMaxLength(50)
                .HasColumnName("operation");
            entity.Property(e => e.Player)
                .HasMaxLength(50)
                .HasColumnName("player");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
            entity.Property(e => e.Uid)
                .HasMaxLength(50)
                .HasColumnName("uid");
        });

        OnModelCreatingPartial(modelBuilder);
    }


    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
