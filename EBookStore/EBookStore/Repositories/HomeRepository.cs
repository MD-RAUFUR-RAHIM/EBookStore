using Microsoft.EntityFrameworkCore;

namespace EBookStore.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        //ctor
        public HomeRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<Book>> GetBooks(string str = "", int genreId=0)
        {
            str = str.ToLower();
            IEnumerable<Book> books = await (from book in _applicationDbContext.Books
                         join genre in _applicationDbContext.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrEmpty(str) || (book != null && book.BookName.ToLower().StartsWith(str))
                         select new Book
                         {
                             Id = book.Id,
                             BookImage = book.BookImage,
                             AuthorName = book.AuthorName,
                             BookName = book.BookName,
                             GenreId = book.GenreId,
                             Price = book.Price,
                             GenreName = genre.GenreName
                         }
                         ).ToListAsync();
            if (genreId > 0) { 
            books = books.Where( a => a.GenreId == genreId ).ToList();
            }
            return books;
        }
    }
    
}
