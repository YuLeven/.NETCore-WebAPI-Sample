using System;
using System.Threading;
using System.Threading.Tasks;
using HaruGaKita.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

#pragma warning disable 1591
namespace HaruGaKita.Persistence
{
    public class HaruGaKitaDbContext : DbContext
    {
        public HaruGaKitaDbContext(DbContextOptions<HaruGaKitaDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            PrepareForSaving();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            PrepareForSaving();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void PrepareForSaving()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.Created = now;
                            entity.Updated = now;
                            entity.Uid = Guid.NewGuid();
                            break;
                        case EntityState.Modified:
                            entity.Updated = now;
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => new { u.Email })
                .IsUnique(true);
        }
    }
}