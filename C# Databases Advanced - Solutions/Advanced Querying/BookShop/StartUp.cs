namespace BookShop
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Globalization;

    using Initializer;

    using Data;
    using BookShop.Models.Enums;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            //DbInitializer.ResetDatabase(db);

            //Console.WriteLine(GetBooksByAgeRestriction(db, Console.ReadLine()));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetGoldenBooks(db));
            //Console.WriteLine(GetBooksByPrice(db));
            //Console.WriteLine(GetBooksNotReleasedIn(db,int.Parse(Console.ReadLine())));
            //Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));  
            //Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));
            //Console.WriteLine(GetBooksReleasedBefore(db, Console.ReadLine()));
            //Console.WriteLine(GetAuthorNamesEndingIn(db, Console.ReadLine()));
            //Console.WriteLine(GetBookTitlesContaining(db, Console.ReadLine()));
            //Console.WriteLine(GetBooksByAuthor(db, Console.ReadLine()));
            //Console.WriteLine(CountBooks(db, int.Parse(Console.ReadLine())));
            //Console.WriteLine(CountCopiesByAuthor(db));
            //Console.WriteLine(GetMostRecentBooks(db));
            //IncreasePrices(db);
            Console.WriteLine(RemoveBooks(db));
        }
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            var booksTitles = context.Books.
                Where(t => t.AgeRestriction == ageRestriction).
                Select(x => new
                {
                    Title = x.Title
                }).OrderBy(x => x.Title).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in booksTitles)
            {
                sb.AppendLine(title.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetGoldenBooks(BookShopContext context)
        {
            EditionType goldenType = Enum.Parse<EditionType>("Gold", true);

            var goldenBooks = context.Books.
               Where(x => x.EditionType == goldenType && x.Copies < 5000).
               Select(x => new
               {
                   Id = x.BookId,
                   Title = x.Title
               }).
               OrderBy(x => x.Id).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var title in goldenBooks)
            {
                sb.AppendLine(title.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByPrice(BookShopContext context)
        {
            decimal price = 40;

            var books = context.Books
                .Where(x => x.Price > price)
                .Select(x => new
                {
                    Title = x.Title,
                    Price = x.Price
                }
                ).OrderByDescending(x => x.Price).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Title} - ${book.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(book => book.ReleaseDate.HasValue && book.ReleaseDate.Value.Year != year)
                .Select(x => new
                {
                    x.BookId,
                    x.Title,
                    x.ReleaseDate
                })
                .OrderBy(x => x.BookId)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToArray();

            var books = context.Books
                .Where(x => x.BookCategories
                .Any(y => categories.Contains(y.Category.Name.ToLower()))).Select(x => new { x.Title })
                .OrderBy(x => x.Title)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var item in books)
            {
                sb.AppendLine(item.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            DateTime specifiedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value < specifiedDate)
                .Select(x => new
                {
                    Ttitle = x.Title,
                    Price = x.Price,
                    ReleaseDate = x.ReleaseDate,
                    EditionType = x.EditionType.ToString()
                })
                .OrderByDescending(x => x.ReleaseDate)
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine($"{book.Ttitle} - {book.EditionType} - ${book.Price:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.FirstName.EndsWith(input))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName
                })
                .OrderBy(x => x.FullName).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine(author.FullName);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => new
                {
                    x.Title
                })
                .OrderBy(x => x.Title).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var book in books)
            {
                sb.AppendLine(book.Title);
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => x.LastName.ToLower().StartsWith(input.ToLower()))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    Books = x.Books.Select(book => new
                    {
                        BookId = book.BookId,
                        Title = book.Title
                    }
                    ).OrderBy(x => x.BookId).ToArray()
                })
                .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                foreach (var book in author.Books)
                {
                    sb.AppendLine($"{book.Title} ({author.FullName})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books.Where(x => x.Title.Length > lengthCheck).Count();

            return books;
        }
        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    FullName = string.Concat(x.FirstName, ' ', x.LastName),
                    BooksCount = x.Books.Select(book => book.Copies).Sum()
                }).OrderByDescending(x => x.BooksCount).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var author in authors)
            {
                sb.AppendLine($"{author.FullName} - {author.BooksCount}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categoryProfit = context.Categories
                .Select(x => new
                {
                    Category = x.Name,
                    Profit = x.CategoryBooks.Select(y => y.Book.Price * y.Book.Copies).Sum()
                }
                ).OrderByDescending(x => x.Profit).ThenBy(x => x.Category).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var category in categoryProfit)
            {
                sb.AppendLine($"{category.Category} ${category.Profit:f2}");
            }

            return sb.ToString().TrimEnd();
        }
        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categoryBooks = context.Categories
                              .Select(x => new
                              {
                                  CategoryName = x.Name,
                                  Book = x.CategoryBooks.Select(y => new
                                  {
                                      Title = y.Book.Title,
                                      ReleaseDate = y.Book.ReleaseDate
                                  }).OrderByDescending(x => x.ReleaseDate).Take(3).ToArray()
                              }
                              ).OrderBy(x => x.CategoryName).ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (var categoryInfo in categoryBooks)
            {
                sb.AppendLine($"--{categoryInfo.CategoryName}");
                foreach (var book in categoryInfo.Book)
                {
                    sb.AppendLine($"{book.Title} ({book.ReleaseDate.Value.Year})");
                }
            }

            return sb.ToString().TrimEnd();
        }
        public static void IncreasePrices(BookShopContext context)
        {
            var booksBefore2010 = context.Books.Where(x => x.ReleaseDate.HasValue && x.ReleaseDate.Value.Year < 2010).ToArray();
            StringBuilder sb = new StringBuilder();
            foreach (var book in booksBefore2010)
            {
                sb.AppendLine($"{book.Title} Date: {book.ReleaseDate.Value.Year} Price: {book.Price}");
                book.Price += 5;
            }            

            context.SaveChanges();
        }
        public static int RemoveBooks(BookShopContext context)
        {
            var booksForDeleting = context.Books.Where(x => x.Copies < 4200).ToArray();

            foreach (var book in booksForDeleting)
            {
                context.Remove(book);
            }
            
            context.SaveChanges();

            return booksForDeleting.Length;
        }
    }
}
