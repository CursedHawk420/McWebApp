﻿// <auto-generated />
using System;
using Highgeek.McWebApp.Common.Models.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Highgeek.McWebApp.Common.Migrations
{
    [DbContext(typeof(McWebApp1CmsContext))]
    [Migration("20240607134736_Cms start")]
    partial class Cmsstart
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_general_ci")
                .HasAnnotation("MySql:CharSet", "utf8mb4")
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Highgeek.McWebApp.Common.Models.mcwebapp1_cms.CarouselContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int(11)");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .HasColumnType("text")
                        .HasColumnName("content");

                    b.Property<string>("Header")
                        .HasColumnType("text")
                        .HasColumnName("header");

                    b.Property<string>("Imageurl")
                        .HasColumnType("text")
                        .HasColumnName("imageurl");

                    b.Property<int>("Order")
                        .HasColumnType("int(11)")
                        .HasColumnName("order");

                    b.Property<bool?>("Visible")
                        .HasColumnType("boolean");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("carousel_content", (string)null);
                });

            modelBuilder.Entity("Highgeek.McWebApp.Common.Models.mcwebapp1_cms.ImageCache", b =>
                {
                    b.Property<string>("Uuid")
                        .HasMaxLength(37)
                        .HasColumnType("character varying(37)")
                        .HasColumnName("uuid");

                    b.Property<string>("Date")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("date");

                    b.Property<string>("Format")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("format");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnType("blob")
                        .HasColumnName("image");

                    b.Property<string>("Imageurl")
                        .HasColumnType("text")
                        .HasColumnName("imageurl");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Uuid")
                        .HasName("PRIMARY");

                    b.ToTable("image_cache", (string)null);
                });

            modelBuilder.Entity("Highgeek.McWebApp.Common.Models.mcwebapp1_cms.Serverstatus", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("longtext")
                        .HasColumnName("name");

                    b.Property<string>("Ip")
                        .HasColumnType("text")
                        .HasColumnName("ip");

                    b.Property<bool?>("Maintenance")
                        .HasColumnType("boolean")
                        .HasColumnName("maintenance");

                    b.Property<string>("Maxplayers")
                        .HasColumnType("text")
                        .HasColumnName("maxplayers");

                    b.Property<string>("Online")
                        .HasColumnType("text")
                        .HasColumnName("online");

                    b.Property<string>("Order")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("order");

                    b.Property<string>("Players")
                        .HasColumnType("text")
                        .HasColumnName("players");

                    b.Property<string>("Playerslist")
                        .HasColumnType("text")
                        .HasColumnName("playerslist");

                    b.Property<string>("Port")
                        .HasColumnType("text")
                        .HasColumnName("port");

                    b.Property<string>("Rconpass")
                        .HasColumnType("text")
                        .HasColumnName("rconpass");

                    b.Property<string>("Rconport")
                        .HasColumnType("text")
                        .HasColumnName("rconport");

                    b.Property<bool?>("Visible")
                        .HasColumnType("boolean")
                        .HasColumnName("visible");

                    b.HasKey("Name")
                        .HasName("PRIMARY");

                    b.ToTable("serverstatus", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
