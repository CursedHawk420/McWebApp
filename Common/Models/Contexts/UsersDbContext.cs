using Highgeek.McWebApp.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Highgeek.McWebApp.Common.Models.Contexts
{
    public class UsersDbContext : IdentityDbContext<ApplicationUser>
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "user");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("userroles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("userclaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("userlogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("roleclaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("usertokens");
            });
            builder.Entity<MinecraftUser>(entity =>
            {
                entity.HasKey(e => e.Uuid).HasName("web_minecraftuser_pkey");

                entity.ToTable("web_minecraftuser");

                entity.Property(e => e.Uuid)
                    .HasMaxLength(36)
                    .HasColumnName("uuid");
                entity.Property(e => e.ApplicationUserEmail).HasColumnName("applicationuseremail");
                entity.Property(e => e.ApplicationUserId).HasColumnName("applicationuserid");
                entity.Property(e => e.ApplicationUserName).HasColumnName("applicationusername");
                entity.Property(e => e.EcoId).HasColumnName("ecoid");
                entity.Property(e => e.IsPremium).HasColumnName("ispremium");
                entity.Property(e => e.LpUserGroup).HasColumnName("lpusergroup");
                entity.Property(e => e.NickName).HasColumnName("nickname");
                entity.Property(e => e.PremiumUuid).HasColumnName("premiumuuid");
                entity.Property(e => e.SkinHeadTexture).HasColumnName("skinheadtexture");
                entity.Property(e => e.SkinHeadTextureImage).HasColumnName("skinheadtextureimage");
                entity.Property(e => e.SkinTexture).HasColumnName("skintexture");
            });

            builder.Entity<WebPlanData>(entity =>
            {
                entity.HasKey(e => e.Name).HasName("web_player_data_pkey");

                entity.ToTable("web_player_data");

                entity.Property(e => e.Name).HasColumnName("name");
                entity.Property(e => e.Plandata).HasColumnName("plandata");
                entity.Property(e => e.Uuid).HasColumnName("uuid");
            });
        }
    }

}