using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Highgeek.McWebApp.Common.Migrations
{
    /// <inheritdoc />
    public partial class Cmsstart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "carousel_content",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    header = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    imageurl = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<int>(type: "int", nullable: false),
                    Visible = table.Column<bool>(type: "boolean", nullable: true)
                });

            migrationBuilder.CreateTable(
                name: "image_cache",
                columns: table => new
                {
                    uuid = table.Column<string>(type: "text", maxLength: 37, nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    imageurl = table.Column<string>(type: "text", nullable: true),
                    image = table.Column<byte[]>(type: "bytea", nullable: false),
                    format = table.Column<string>(type: "text", maxLength: 255, nullable: true),
                    date = table.Column<string>(type: "text", maxLength: 255, nullable: false)
                });

            migrationBuilder.CreateTable(
                name: "serverstatus",
                columns: table => new
                {
                    name = table.Column<string>(type: "text", nullable: false),
                    players = table.Column<string>(type: "text", nullable: true),
                    maxplayers = table.Column<string>(type: "text", nullable: true),
                    playerslist = table.Column<string>(type: "text", nullable: true),
                    online = table.Column<string>(type: "text", nullable: true),
                    order = table.Column<string>(type: "text", nullable: false),
                    visible = table.Column<bool>(type: "boolean", nullable: true),
                    maintenance = table.Column<bool>(type: "boolean", nullable: true),
                    ip = table.Column<string>(type: "text", nullable: true),
                    port = table.Column<string>(type: "text", nullable: true),
                    rconport = table.Column<string>(type: "text", nullable: true),
                    rconpass = table.Column<string>(type: "text", nullable: true)
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "carousel_content");

            migrationBuilder.DropTable(
                name: "image_cache");

            migrationBuilder.DropTable(
                name: "serverstatus");
        }
    }
}
