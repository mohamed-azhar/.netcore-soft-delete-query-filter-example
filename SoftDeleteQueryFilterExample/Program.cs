using SoftDeleteQueryFilterExample.Database;
using SoftDeleteQueryFilterExample.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SoftDeleteQueryFilterExample
{
    class Program
    {
        static bool ValidGuidingOptionSelected = false;
        static bool ValidBookDeleteOptionSelected = false;

        public static async Task Main(string[] args)
        {
            //print main intro 
            PrintIntro();

            //print guiding screen
            PrintGuidingScreen();

            // Create and get db context
            var context = GetDbContext();
            await context.EnsureDatabaseCreatedAsync();

            //initialize book service
            var bookService = new BookService(context);

            int guidingScreenSeletion = 0;

            SelectFromGuidingScreen(ref guidingScreenSeletion, bookService);
        }

        #region Information Display
        static void PrintIntro()
        {
            Console.Title = "Soft Delete Query Filter Example by mohamed-azhar";
            Console.WriteLine("=============================================");
            Console.WriteLine("=     Soft Delete Query Filter Example      =");
            Console.WriteLine("=               mohamed-azhar               =");
            Console.WriteLine("=      https://github.com/mohamed-azhar     =");
            Console.WriteLine("=============================================");
        }

        static void PrintHelp()
        {
            var width = 100;
            PrintDivider(tableWidth: 100);
            Console.ForegroundColor = ConsoleColor.Green;
            PrintRow(width, "ABOUT");
            Console.ForegroundColor = ConsoleColor.White;
            PrintRow(width, "A console app to demonstrate soft delete query filter with .NET Core and EntityFramework Core");
            PrintDivider(tableWidth: 100);
            Console.ForegroundColor = ConsoleColor.Green;
            PrintRow(width, "HOW IT WORKS");
            Console.ForegroundColor = ConsoleColor.White;
            PrintRow(width, "A list of books is prepopulated on database create");
            PrintRow(width, "View the list of books from database");
            PrintRow(width, "Delete a book by providing its Id");
            PrintRow(width, "Observe from database that the record is not removed instead the IsDeleted switch is turned on");
            PrintDivider(tableWidth: 100);
            Console.ForegroundColor = ConsoleColor.Green;
            PrintRow(width, "REQUIREMENTS");
            Console.ForegroundColor = ConsoleColor.White;
            PrintRow(width, ".NET Core SDK 3.1+");
            PrintRow(width, "SQL Server");
            PrintDivider(tableWidth: 100);
        }

        static void PrintGuidingScreen()
        {
            Console.WriteLine("\n\tSelect an option to proceed");
            Console.WriteLine("\t\t1 - Help");
            Console.WriteLine("\t\t2 - View all Books");
            Console.WriteLine("\t\t3 - Delete a Book");
        }

        static void SelectFromGuidingScreen(ref int selectedMainScreenOption, BookService bookService)
        {
            int bookTableWidth = 100;
            do
            {
                Console.Write("option: ");
                int.TryParse(Console.ReadLine(), out var selectedOption);
                selectedMainScreenOption = selectedOption;

                if (selectedMainScreenOption != 1 && selectedMainScreenOption != 2 && selectedMainScreenOption != 3)
                {
                    ValidGuidingOptionSelected = false;
                    Console.WriteLine("Invalid option selected.\n");
                    SelectFromGuidingScreen(ref selectedMainScreenOption, bookService);
                }
                else
                {
                    ValidGuidingOptionSelected = true;
                    
                    if (selectedMainScreenOption == 1)
                    {
                        PrintHelp();
                        PrintGuidingScreen();
                        SelectFromGuidingScreen(ref selectedMainScreenOption, bookService);
                    }
                    else if (selectedMainScreenOption == 2)
                    {
                        DisplayBooks(bookService, bookTableWidth).GetAwaiter().GetResult();
                        PrintGuidingScreen();
                        SelectFromGuidingScreen(ref selectedMainScreenOption, bookService);
                    }
                    else if (selectedMainScreenOption == 3)
                    {
                        int selectedBookDeleteOption = 0;
                        SelectFromDeleteBook(ref selectedBookDeleteOption, bookService.GetTotalCount().GetAwaiter().GetResult());
                        var deleteResult = bookService.DeleteBook(selectedBookDeleteOption).GetAwaiter().GetResult();
                        if (!deleteResult.Item1) Console.WriteLine(deleteResult.Item2);
                        else Console.WriteLine("Deleted the book with Id " + selectedBookDeleteOption);

                        PrintGuidingScreen();
                        SelectFromGuidingScreen(ref selectedMainScreenOption, bookService);
                    }
                }

            } while (!ValidGuidingOptionSelected);
        }

        static async Task DisplayBooks(BookService bookService, int bookTableWidth)
        {
            var books = await bookService.GetAllBooks();
            PrintRow(bookTableWidth, "Id", "Title", "Author", "Created Date");
            PrintDivider(tableWidth: bookTableWidth);
            if (books?.Any() ?? false)
            {
                foreach (var book in books)
                {
                    PrintRow(bookTableWidth, book.Id.ToString(), book.Title, book.Author, book.CreatedDate.ToShortDateString());
                }
            }
            else
            {
                PrintRow(bookTableWidth, "No books to show");
            }
        }

        static void SelectFromDeleteBook(ref int selectedBookDeleteOption, int booksCount)
        {
            do
            {
                Console.Write("Select the Id of the book to delete: ");

                int.TryParse(Console.ReadLine(), out int selectedOption);
                selectedBookDeleteOption = selectedOption;

                if (selectedOption > booksCount || selectedOption < 1)
                {
                    ValidBookDeleteOptionSelected = false;
                    Console.WriteLine("Invalid Id selected. Make sure you select a valid Id from the Id column of the books table\n");
                }
                else
                {
                    ValidBookDeleteOptionSelected = true;
                }

            } while (!ValidBookDeleteOptionSelected);
        }

        #endregion

        #region DbContext Helpers
        /// <summary>
        /// Creates a new db context from the dbcontextfactory
        /// </summary>
        /// <returns></returns>
        static AppDbContext GetDbContext() => new DbContextFactory().CreateDbContext(null);
        #endregion

        #region Table UI
        /// <summary>
        /// Draws a single table row 
        /// </summary>
        /// <param name="columns">columns to be drawn</param>
        static void PrintRow(int tableWidth = 60, params string[] columns)
        {
            int width = (tableWidth - columns.Length) / columns.Length;
            string row = "|";

            foreach (string column in columns)
            {
                row += AlignCentre(column, width) + "|";
            }

            Console.WriteLine(row);
        }

        /// <summary>
        /// Aligning text in a column
        /// </summary>
        /// <param name="text">text of the colum</param>
        /// <param name="width">width of the column</param>
        /// <returns></returns>
        static string AlignCentre(string text, int width)
        {
            text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

            if (string.IsNullOrEmpty(text)) return new string(' ', width);
            else return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }

        /// <summary>
        /// Prints a divider in the table
        /// </summary>
        /// <param name="drawCharacter">character representing the divider</param>
        static void PrintDivider(char drawCharacter = '-', int tableWidth = 60) => Console.WriteLine(new string(drawCharacter, tableWidth));
        #endregion
    }
}
