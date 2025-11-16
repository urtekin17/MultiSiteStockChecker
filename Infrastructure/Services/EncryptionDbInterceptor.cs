using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EncryptionDbInterceptor: SaveChangesInterceptor, IMaterializationInterceptor
    {
        private readonly IEncryptionService _encryption;
        public EncryptionDbInterceptor(IEncryptionService encryption)
        {
            _encryption = encryption;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            EncryptEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            EncryptEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void EncryptEntities(DbContext? context)
        {
            if (context == null) return;
            var entries = context.ChangeTracker.Entries<Site>()
                 .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var item in entries)
            {
                var entity = item.Entity;
                if (!string.IsNullOrEmpty(entity.Password))
                {
                    entity.PasswordEncrypted = _encryption.Encrypt(entity.Password);
                    entity.Password = string.Empty;
                }
            }
        }
        // After EF materializes an entity, decrypt PasswordEncrypted -> set Password property
        public object CreatedInstance(MaterializationInterceptionData materializationData, object entity)
        {
            if (entity is Site s && !string.IsNullOrEmpty(s.PasswordEncrypted))
            {
                try
                {
                    s.Password = _encryption.Decrypt(s.PasswordEncrypted);
                }
                catch
                {
                    s.Password = null;
                }
            }
            return entity;
        }

        // Other IMaterializationInterceptor methods defaults
        public object InitializingInstance(MaterializationInterceptionData materializationData, object entity)
        {
            return entity;
        }
    }
}
