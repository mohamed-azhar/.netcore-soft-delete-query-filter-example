using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SoftDeleteQueryFilterExample.Database;
using SoftDeleteQueryFilterExample.Interfaces;
using SoftDeleteQueryFilterExample.Models;
using SoftDeleteQueryFilterExample.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Create and get db context
            var context = GetDbContext();
            await context.EnsureDatabaseCreatedAsync();

            //prepare book service
            var bookService = new BookService(context);
        }

      




        #region DbContext Helpers
        static AppDbContext GetDbContext()
        {
            return new DbContextFactory().CreateDbContext(null);
        }
        #endregion
    }
}
