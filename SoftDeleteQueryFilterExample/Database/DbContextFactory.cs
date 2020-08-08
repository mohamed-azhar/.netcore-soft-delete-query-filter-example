using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SoftDeleteQueryFilterExample.Database
{
    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        private static string _connectionString = "Server=.;Database=book-store;Trusted_Connection=True;MultipleActiveResultSets=true";

        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(_connectionString);

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
