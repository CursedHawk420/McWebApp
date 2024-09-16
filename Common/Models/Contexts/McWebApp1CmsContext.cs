using System;
using System.Collections.Generic;
using Highgeek.McWebApp.Common.Helpers;
using Highgeek.McWebApp.Common.Models.mcwebapp1_cms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Highgeek.McWebApp.Common.Models.Contexts;

public partial class McWebApp1CmsContext : DbContext
{

    private protected string ConnectionString;

    public McWebApp1CmsContext()
    {
        ConnectionString = ConfigProvider.GetConnectionString("PostgresCmsConnection");
    }

    public McWebApp1CmsContext(DbContextOptions<McWebApp1CmsContext> options)
        : base(options)
    {
        ConnectionString = ConfigProvider.GetConnectionString("PostgresCmsConnection");
    }

    public virtual DbSet<CarouselContent> CarouselContents { get; set; }


    public virtual DbSet<Serverstatus> Serverstatuses { get; set; }

    public virtual DbSet<ImageCache> ImageCache { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        => optionsBuilder.UseNpgsql(ConnectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CarouselContent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carousel_content");

            entity.Property(e => e.Id).HasColumnType("int(11)");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Header).HasColumnName("header");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Order)
                .HasColumnType("int(11)")
                .HasColumnName("order");
        });


        modelBuilder.Entity<Serverstatus>(entity =>
        {
            entity.HasKey(e => e.Name)
                .HasName("PRIMARY");

            entity.ToTable("serverstatus");

            entity.Property(e => e.Name)
                .HasColumnType("longtext")
                .HasColumnName("name");
            entity.Property(e => e.Ip)
                .HasColumnType("text")
                .HasColumnName("ip");
            entity.Property(e => e.Maintenance).HasColumnName("maintenance");
            entity.Property(e => e.Maxplayers).HasColumnName("maxplayers");
            entity.Property(e => e.Online).HasColumnName("online");
            entity.Property(e => e.Order).HasColumnName("order");
            entity.Property(e => e.Players).HasColumnName("players");
            entity.Property(e => e.Playerslist).HasColumnName("playerslist");
            entity.Property(e => e.Playerslist).HasConversion(
                    v => JsonConvert.SerializeObject(v),
                    v => JsonConvert.DeserializeObject<List<string>>(v));

            entity.Property(e => e.Port)
                .HasColumnType("text")
                .HasColumnName("port");
            entity.Property(e => e.Rconpass)
                .HasColumnType("text")
                .HasColumnName("rconpass");
            entity.Property(e => e.Rconport)
                .HasColumnType("text")
                .HasColumnName("rconport");
            entity.Property(e => e.Visible).HasColumnName("visible");
        });
        
        modelBuilder.Entity<ImageCache>(entity =>
        {
            entity.HasKey(e => e.Uuid).HasName("PRIMARY");

            entity.ToTable("image_cache");

            entity.Property(e => e.Uuid)
                .HasMaxLength(37)
                .HasColumnName("uuid");
            entity.Property(e => e.Date)
                .HasMaxLength(255)
                .HasColumnName("date");
            entity.Property(e => e.Format)
                .HasMaxLength(255)
                .HasColumnName("format");
            entity.Property(e => e.Image)
                .HasColumnType("blob")
                .HasColumnName("image");
            entity.Property(e => e.Imageurl).HasColumnName("imageurl");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
