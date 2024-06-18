using Autoflow.Portal.Domain;
using Autoflow.Portal.Domain.RunnerBots;
using Microsoft.EntityFrameworkCore;

namespace Autoflow.Portal.EntityFrameworkCore.EntityFrameworkCore
{
    public class PortalDbContext(DbContextOptions<PortalDbContext> options) : DbContext(options)
    {
        public DbSet<RunnerBot> RunnerBots { get; set; }
        public DbSet<RunnerEvent> RunnerEvents { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<RunnerBot>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "runner_bot", PortalConst.DbSchema);
            });

            builder.Entity<RunnerEvent>(b =>
            {
                b.ToTable(PortalConst.DbTablePrefix + "runner_event", PortalConst.DbSchema);
            });
        }
    }
}
