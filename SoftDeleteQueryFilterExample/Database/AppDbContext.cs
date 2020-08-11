using Microsoft.EntityFrameworkCore;
using SoftDeleteQueryFilterExample.Extensions;
using SoftDeleteQueryFilterExample.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder bob)
        {
            var foreignKeys = bob.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys());
            foreach (var relationship in foreignKeys) relationship.DeleteBehavior = DeleteBehavior.Restrict;

            bob.ApplySoftDeleteQueryFilter();
        }

        public async Task EnsureDatabaseCreatedAsync()
        {
            await Database.EnsureCreatedAsync();

            if (Books != null && (!Books?.Any() ?? false))
            {
                Books.AddRange(new[]
                {
                    new Book("Lolita", "Vladimir Nabokov"),
                    new Book("The Great Gatsby", "F. Scott Fitzgerald"),
                    new Book("In Search of Lost Time", "Marcel Proust"),
                    new Book("Ulysses", "James Joyce"),
                    new Book("Dubliners", "James Joyce"),
                    new Book("One Hundred Years of Solitude ", "Gabriel Garcia Marquez"),
                    new Book("The Sound and the Fury", "William Faulkner"),
                });
                await SaveChangesAsync();
            }

        }
    }
}
