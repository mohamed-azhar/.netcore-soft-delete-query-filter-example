using Microsoft.EntityFrameworkCore;
using SoftDeleteQueryFilterExample.Database;
using SoftDeleteQueryFilterExample.Interfaces;
using SoftDeleteQueryFilterExample.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample.Services
{
    public abstract class BaseService
    {
        public AppDbContext DbContext { get; set; }

        public BaseService(AppDbContext context)
        {
            DbContext = context;
        }

        public async Task Commit()
        {
            SetAuditInformation();
            await DbContext.SaveChangesAsync();
        }

        public void SetAuditInformation()
        {
            var deletable = DbContext.ChangeTracker.Entries<IDeletableEntity>().ToList();

            foreach (var record in deletable)
            {
                switch (record.State)
                {
                    case EntityState.Added:
                    case EntityState.Modified:
                        record.Entity.IsDeleted = false;
                        break;
                    case EntityState.Deleted:
                        record.State = EntityState.Modified;
                        record.Entity.IsDeleted = true;
                        break;
                }
            }
        }

        public async Task<Tuple<bool, string>> DeleteEntity<T>(int id) where T : CommonAttributes
        {
            var entity = await DbContext.Set<T>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity is null) return Tuple.Create(false, "Entity to be deleted cannot be found");

            try
            {
                DbContext.Set<T>().Remove(entity);
                await Commit();
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }

        }
    }
}
