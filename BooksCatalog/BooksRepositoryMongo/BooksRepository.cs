using BooksCatalog;
using DataAbstraction.Models;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using DataAbstraction.Interfaces;

namespace BooksRepositoryMongo
{
    public class BooksRepository : IBooksRepository
    {
        private readonly IMongoCollection<Book> _booksRepository;

        public BooksRepository(IOptions<BooksDataBaseConnection> dbSettings)
        {
            var mongoClient = new MongoClient(
                dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dbSettings.Value.DatabaseName);

            _booksRepository = mongoDatabase.GetCollection<Book>(
                dbSettings.Value.CollectionName);
        }


        public async Task<List<Book>> GetAsync()
        {
            return await _booksRepository.Find(_ => true).ToListAsync();
        }

        public async Task<Book?> GetAsync(string id)
        {
            return await _booksRepository.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Book newBook)
        {
            await _booksRepository.InsertOneAsync(newBook);
        }

        public async Task UpdateAsync(string id, Book updatedBook)
        {
            await _booksRepository.ReplaceOneAsync(x => x.Id == id, updatedBook);
        }

        public async Task RemoveAsync(string id)
        {
            await _booksRepository.DeleteOneAsync(x => x.Id == id);
        }            
    }
}