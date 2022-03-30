using BooksCatalog;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using Microsoft.Extensions.Options;
using Nest;

namespace ElasticSearchService
{
    public class ElasticSearchService : IElasticSearchService
    {
        private ElasticClient _elasticClient;

        public ElasticSearchService(IOptions<ElasticSearchConnection> elasticSettings)
        {
            ConnectionSettings connSettings = new ConnectionSettings(new Uri(elasticSettings.Value.Uri))
                .DefaultIndex(elasticSettings.Value.DefaultIndex);

            _elasticClient = new ElasticClient(connSettings);
        }

        public async Task<ISearchResponse<Book>> SearchAsync(string term)
        {
            return await _elasticClient
                .SearchAsync<Book>(x => x
                    .Query(q => q
                        .QueryString(qs => qs
                            .Query(term))));
        }

        public async Task<ISearchResponse<Book>> SearchByAuthorAsync(string term)
        {            
            return await _elasticClient
                .SearchAsync<Book>(s => s
                    .Query (q => q
                        .Match (m => m
                            .Field(f => f.Author)
                            .Query(term))));
        }

        public async Task<ISearchResponse<Book>> SearchByBookNameAsync(string term)
        {
            return await _elasticClient
                .SearchAsync<Book>(s => s
                    .Query(q => q
                       .Match(m => m
                          .Field(f => f.Name)
                          .Query(term))));
        }

        public async Task<ISearchResponse<Book>> SearchByBookDescriptionAsync(string term)
        {
            return await _elasticClient
                .SearchAsync<Book>(s => s
                    .Query(q => q
                       .Match(m => m
                          .Field(f => f.Description)
                          .Query(term))));
        }

        public async Task IndexBookAsync(Book book)
        {
            await _elasticClient.IndexDocumentAsync(book);
        }

        public async Task IndexManyBooksAsync(Book[] books)
        {
            await _elasticClient.IndexManyAsync(books);
        }

        public async Task DeleteBookByIndexAsync(string id)
        {
            await _elasticClient
                .DeleteByQueryAsync<Book>(d => d
                    .Query(q => q
                        .Match(m => m
                          .Field(f => f.Id)
                            .Query(id))));
        }

        public async Task UpdateBookAsync(string id, Book updateBook)
        {
            var indexedBook = await _elasticClient.GetAsync<Book>(id);
            var newBook = indexedBook.Source;

            newBook.Name = updateBook.Name;
            newBook.Description = updateBook.Description;
            newBook.Author = updateBook.Author;
            newBook.Price = updateBook.Price;
            newBook.Category = updateBook.Category;

            var updateResponse = _elasticClient
                .Update<Book>(newBook, u => u.Doc(newBook)
                .RetryOnConflict(1));
        }
    }
}