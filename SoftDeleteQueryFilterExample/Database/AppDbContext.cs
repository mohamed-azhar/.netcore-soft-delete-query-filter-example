using Microsoft.EntityFrameworkCore;
using SoftDeleteQueryFilterExample.Extensions;
using SoftDeleteQueryFilterExample.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample.Database
{
    public class AppDbContext : DbContext
    {
        private static string _connectionString = "Server=.;Database=book_store;Trusted_Connection=True;MultipleActiveResultSets=true";

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder bob)
        {
            //set delete behavior for foreign keys
            var foreignKeys = bob.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys());
            foreach (var relationship in foreignKeys) relationship.DeleteBehavior = DeleteBehavior.Restrict;

            bob.ConfigureSoftDeleteQueryFilter();
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            await Database.EnsureCreatedAsync();
        }

        #region Db Sets
        public DbSet<Book> Book { get; set; }
        #endregion
    }
}
