namespace BookShop
{
    using System.Globalization;
    using System.Text;
    using BookShop.Models.Enums;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            using var db = new BookShopContext();
            DbInitializer.ResetDatabase(db);

            string input = Console.ReadLine();

            string result = GetBooksByAuthor(db, input);

            Console.WriteLine(result);

            Console.ReadKey();
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction ageRestriction = Enum.Parse<AgeRestriction>(command, true);

            string[] titles = context.Books
                .Where(a => a.AgeRestriction == ageRestriction)
                .OrderBy(t => t.Title)
                .Select(t => t.Title)
                .ToArray();

            return string.Join(Environment.NewLine, titles);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            string[] result = context.Books
                .Where(b => b.EditionType == EditionType.Gold && b.Copies < 5000)
                .OrderBy(i => i.BookId)
                .Select(c => c.Title)
                .ToArray();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            StringBuilder sb = new StringBuilder();

            var result = context.Books
                .Where(p => p.Price > 40)
                .OrderByDescending(p => p.Price)
                .Select(p => new
                {
                    p.Title,
                    p.Price
                })
                .ToList();

            foreach (var res in result)
            {
                sb.AppendLine($"{res.Title} - ${res.Price:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            string[] results = context.Books
                .Where(r => r.ReleaseDate.Value.Year != year)
                .Select(d => d.Title)
                .ToArray();

            return string.Join(Environment.NewLine, results);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            string[] categories = input
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.ToLower())
                .ToArray();

            string[] bookTitles = context.Books
                .Where(b => b.BookCategories
                    .Any(bc => categories.Contains(bc.Category.Name.ToLower())))
                .OrderBy(b => b.Title)
                .Select(b => b.Title)
                .ToArray();

            return string.Join(Environment.NewLine, bookTitles);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            string pattern = "dd-MM-yyyy";

            DateTime releaseDate = DateTime.ParseExact(
                date,
                pattern,
                CultureInfo.InvariantCulture);

            var result = context.Books
                .Where(d => d.ReleaseDate.HasValue && d.ReleaseDate.Value < releaseDate)
                .OrderByDescending(b => b.ReleaseDate)
                .Select(s => new
                {
                    s.Title,
                    s.EditionType,
                    s.Price
                })
                .ToList()
                .Select(b => $"{b.Title} - {b.EditionType} - ${b.Price:F2}");

            return string.Join(Environment.NewLine, result);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            input = input.ToLower();

            string[] authorNames = context.Authors
                .Where(a => a.FirstName != null &&
                            a.FirstName.ToLower().EndsWith(input))
                .OrderBy(a => a.FirstName)
                .ThenBy(a => a.LastName)
                .Select(a => $"{a.FirstName} {a.LastName}")
                .ToArray();

            return string.Join(Environment.NewLine, authorNames);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            input = input.ToLower();

            var result = context.Books
                .Where(t => t.Title.ToLower().Contains(input))
                .OrderBy(o => o.Title)
                .Select(t => t.Title)
                .ToList();

            return string.Join(Environment.NewLine, result);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            input = input.ToLower();

            var authors = context.Books
                .Where(l => l.Author.LastName.ToLower().StartsWith(input))
                .OrderBy(i => i.BookId)
                .Select(a => new
                {
                    a.Title,
                    AuthorName = a.Author.FirstName + ' ' + a.Author.LastName
                })
                .Select(s => $"{s.Title} ({s.AuthorName})")
                .ToArray();

            return string.Join(Environment.NewLine, authors);

        }
    }
}


