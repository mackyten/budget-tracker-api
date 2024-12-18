using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BT.DOMAIN.Entities;
using Microsoft.EntityFrameworkCore;

namespace BT.PERSISTENCE.Context
{
    public partial class BTDbContext
    {
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userEmail = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)?.Value;

            var newEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity).ToList();

            var newEnumerator = newEntities.GetEnumerator();
            while (newEnumerator.MoveNext())
            {
                var ent = newEnumerator.Current;
                bool isBaseEntityInt = IsTypeof<BaseEntity<int>>(ent);

                if (isBaseEntityInt)
                {
                    var entity = ent as BaseEntity<int>;
                    if (entity != null)
                    {
                        entity.CreatedById = !string.IsNullOrEmpty(entity.CreatedById) ? entity.CreatedById : !string.IsNullOrEmpty(userId) ? userId : "n/a";
                        entity.CreatedBy = !string.IsNullOrEmpty(entity.CreatedBy) ? entity.CreatedBy : !string.IsNullOrEmpty(userEmail) ? userEmail : "n/a";
                    }
                }
                else
                {
                    var entity = ent as BaseEntity<long>;
                    if (entity != null)
                    {
                        entity.CreatedById = !string.IsNullOrEmpty(entity.CreatedById) ? entity.CreatedById : !string.IsNullOrEmpty(userId) ? userId : "n/a";
                        entity.CreatedBy = !string.IsNullOrEmpty(entity.CreatedBy) ? entity.CreatedBy : !string.IsNullOrEmpty(userEmail) ? userEmail : "n/a";
                    }
                }
            }

            var modifiedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity).ToList();

            var modifiedEnumerator = modifiedEntities.GetEnumerator();
            while (modifiedEnumerator.MoveNext())
            {
                var ent = modifiedEnumerator.Current;
                bool isBaseEntityInt = IsTypeof<BaseEntity<int>>(ent);

                if (isBaseEntityInt)
                {
                    var entity = ent as BaseEntity<int>;
                    if (entity != null)
                    {
                        entity.LastUpdatedAt = DateTime.UtcNow;
                        entity.LastUpdatedById = string.IsNullOrEmpty(userId) ? userId : "n/a";
                        entity.LastUpdatedBy = !string.IsNullOrEmpty(userEmail) ? userEmail : "n/a";
                        entity.Version++;
                    }
                }
                else
                {
                    var entity = ent as BaseEntity<long>;
                    if (entity != null)
                    {
                        entity.LastUpdatedAt = DateTime.UtcNow;
                        entity.LastUpdatedById = string.IsNullOrEmpty(userId) ? userId : "n/a";
                        entity.LastUpdatedBy = !string.IsNullOrEmpty(userEmail) ? userEmail : "n/a";
                        entity.Version++;
                    }
                }
            }

            return await base.SaveChangesAsync(true, cancellationToken);
        }

        static bool IsTypeof<T>(object t)
        {
            return t is T;
        }
    }
}