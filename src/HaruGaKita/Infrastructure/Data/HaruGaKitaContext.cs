using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HaruGaKita.Entities;
using HaruGaKita.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace HaruGaKita.Infrastructure.Data
{
    public class HaruGaKitaContext : DbContext
    {
        public HaruGaKitaContext(DbContextOptions<HaruGaKitaContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IDatedEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.Created = now;
                            entity.Updated = now;
                            break;
                        case EntityState.Modified:
                            entity.Updated = now;
                            break;
                    }
                }
            }
        }
    }
}