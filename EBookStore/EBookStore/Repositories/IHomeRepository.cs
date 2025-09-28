namespace EBookStore
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Book>> GetBooks(string str = "", int genreId = 0);
        Task<IEnumerable<Genre>> Genres();
    }
}