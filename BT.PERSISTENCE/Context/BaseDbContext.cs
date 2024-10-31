using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BT.DOMAIN.Entities;
using BT.PERSISTENCE.Security;
using Microsoft.EntityFrameworkCore;

namespace BT.PERSISTENCE.Context
{
    public class BaseDbContext : DbContext
    {
        private readonly IdentityHelper _identityHelper;

        // Use a generic parameter to allow derived types
        public BaseDbContext(DbContextOptions options, IdentityHelper identityHelper)
            : base(options)
        {
            _identityHelper = identityHelper;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var newEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity).ToList();

            foreach (var entity in newEntities.OfType<BaseEntity<int>>())
            {
                entity.CreatedById = !string.IsNullOrEmpty(entity.CreatedById) ? entity.CreatedById : _identityHelper.UserId;
                entity.CreatedBy = !string.IsNullOrEmpty(entity.CreatedBy) ? entity.CreatedBy : _identityHelper.Email;
            }

            var modifiedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity).ToList();

            foreach (var entity in modifiedEntities.OfType<BaseEntity<int>>())
            {
                entity.LastUpdatedAt = DateTime.UtcNow;
                entity.LastUpdatedById = _identityHelper.UserId;
                entity.LastUpdatedBy = _identityHelper.Email;
                entity.Version++;
            }

            return await base.SaveChangesAsync(true, cancellationToken);
        }
    }

}