using Hangfire.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Highgeek.McWebApp.Common.Models.Contexts
{
    public class HangfireDbContext : DbContext
    {
        public HangfireDbContext() { }

        public HangfireDbContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.OnHangfireModelCreating();
        }
    }
}
