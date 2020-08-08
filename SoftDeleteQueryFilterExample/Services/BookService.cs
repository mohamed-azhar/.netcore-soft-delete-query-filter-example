using Microsoft.EntityFrameworkCore;
using SoftDeleteQueryFilterExample.Database;
using SoftDeleteQueryFilterExample.Models;
using System;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample.Services
{
    public class BookService : BaseService
    {
        public BookService(AppDbContext context) : base(context)
        {
        }

        public async Task<int> GetTotalCount() => await DbContext.Book.CountAsync();

        public async Task<Book[]> GetAllBooks() => await DbContext.Book.ToArrayAsync();

        public async Task<Tuple<bool, string>> CreateBook(string title, string description)
        {
            if (string.IsNullOrWhiteSpace(title)) return Tuple.Create(false, "Name cannot be empty.");
            if (string.IsNullOrWhiteSpace(description)) return Tuple.Create(false, "Description cannot be empty.");

            try
            {
                await DbContext.Book.AddAsync(new Book(title, description));

                await Commit();
                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                return Tuple.Create(false, ex.Message);
            }
        }

        public async Task<Tuple<bool, string>> DeleteBook(int id) => await DeleteEntity<Book>(id);
    }
}
