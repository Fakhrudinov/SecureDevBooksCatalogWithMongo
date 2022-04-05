using DataAbstraction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace BooksCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulController : ControllerBase
    {
        private readonly IMongoCollection<Book> _booksRepository;

        public ConsulController(IOptions<BooksDataBaseConnection> dbSettings)
        {
            var mongoClient = new MongoClient(
                dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dbSettings.Value.DatabaseName);

            _booksRepository = mongoDatabase.GetCollection<Book>(
                dbSettings.Value.CollectionName);
        }


        [HttpGet("status")]
        public IActionResult Status()
        {
            var databaseNames = _booksRepository.Database.DatabaseNamespace.DatabaseName;
            if (databaseNames.Equals("BooksCatalog"))
            {
                return Ok(new { status = "Ok", mongodb = databaseNames });
            }

            return Problem("mongodb problem");
        }
    }
}
