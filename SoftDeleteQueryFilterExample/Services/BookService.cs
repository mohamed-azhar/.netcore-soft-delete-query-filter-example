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

        public async Task<int> GetTotalCount() => await DbContext.Books.CountAsync();

        public async Task<Book[]> GetAllBooks() => await DbContext.Books.ToArrayAsync();

        public async Task<Tuple<bool, string>> DeleteBook(int id)
        {
            var book = await DbContext.Books.SingleOrDefaultAsync(x => x.Id == id);
            if (book is null) return Tuple.Create(false, "Book to be deleted cannot be found");

            try
            {
                DbContext.Books.Remove(book);
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
