using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.ORM.EntityFramework
{
    public abstract class BaseDbContext : DbContext
    {

        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = GetCurrentAssembly();
            var entityTypes = assembly.GetTypes()
                .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                .Where(type => type.IsClass)
                .Where(type => type.BaseType != null)
                .Where(type => typeof(ITrack).IsAssignableFrom(type));

            foreach (var entityType in entityTypes)
            {
                if (modelBuilder.Model.FindEntityType(entityType) != null) continue;
                modelBuilder.Model.AddEntityType(entityType);
            }
            base.OnModelCreating(modelBuilder);
        }
        protected abstract Assembly GetCurrentAssembly();

        public override int SaveChanges()
        {
            SetTrackInfo();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTrackInfo();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetTrackInfo()
        {
            ChangeTracker.DetectChanges();

            var entries = this.ChangeTracker.Entries()
                .Where(x => x.Entity is ITrack)
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var entry in entries)
            {
                var entity = entry.Entity;
                var entityBase = entity as ITrack;
                entityBase.Updatetime = DateTime.Now;
                if (entry.State == EntityState.Added)
                {
                    entityBase.Createtime = DateTime.Now;
                }
            }
        }
    }
}
