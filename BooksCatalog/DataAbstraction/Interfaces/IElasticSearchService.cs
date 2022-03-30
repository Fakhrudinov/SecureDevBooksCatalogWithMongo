using BooksCatalog;
using Nest;

namespace DataAbstraction.Interfaces
{
    public interface IElasticSearchService
    {
        Task<ISearchResponse<Book>> SearchByAuthorAsync(string term);
        Task<ISearchResponse<Book>> SearchByBookNameAsync(string term);
        Task<ISearchResponse<Book>> SearchByBookDescriptionAsync(string term);
        Task<ISearchResponse<Book>> SearchAsync(string term);
        Task IndexBookAsync(Book book);
        Task IndexManyBooksAsync(Book[] books);
        Task DeleteBookByIndexAsync(string id);
        Task UpdateBookAsync(string id, Book updateBook);
    }
}
