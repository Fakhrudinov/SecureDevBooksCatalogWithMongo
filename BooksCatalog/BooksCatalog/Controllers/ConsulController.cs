using Consul;
using DataAbstraction.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text;
using System.Text.Json;

namespace BooksCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsulController : ControllerBase
    {
        private readonly IMongoCollection<Book> _booksRepository;
        private Func<IConsulClient> _consulClientFactory;

        public ConsulController(IOptions<BooksDataBaseConnection> dbSettings, Func<IConsulClient> consulClientFactory)
        {
            var mongoClient = new MongoClient(
                dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                dbSettings.Value.DatabaseName);

            _booksRepository = mongoDatabase.GetCollection<Book>(
                dbSettings.Value.CollectionName);

            _consulClientFactory = consulClientFactory;
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

        // PUT api/Consul/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] ConsulExampleKey jsonData)
        {
            using (var client = _consulClientFactory())
            {
                var json = JsonSerializer.Serialize<ConsulExampleKey>(jsonData);
                var value = Encoding.UTF8.GetBytes(json);

                var putPair = new KVPair($"ConsulExampleKey-{id.ToString()}")
                {
                    Value = value
                };

                await client.KV.Put(putPair);
            }
        }

        // GET api/Consul/MyAllConsulExampleKey
        [HttpGet("MyAllConsulExampleKey")]
        public async Task<IEnumerable<ConsulExampleKey>> GetMyAllKeys()
        {
            using (var client = _consulClientFactory())
            {
                var getPairs = await client.KV.List("ConsulExampleKey-");
                if (getPairs.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    List<ConsulExampleKey> results = new List<ConsulExampleKey>();
                    foreach (var matchedPair in getPairs.Response)
                    {
                        var value = Encoding.UTF8.GetString(matchedPair.Value, 0, matchedPair.Value.Length);

                        var newItem = JsonSerializer.Deserialize<ConsulExampleKey>(value);
                        if(newItem != null)
                        {
                            results.Add(newItem);
                        }                        
                    }
                    return results;
                }
                return new ConsulExampleKey[0];
            }
        }

        // get api/Consul/GetSingleConsulExampleKey/2
        [HttpGet("GetSingleConsulExampleKey/{id}")]
        public async Task<ActionResult<ConsulExampleKey>> GetSingleKey(int id)
        {
            using (var client = _consulClientFactory())
            {
                var getPair = await client.KV.Get($"ConsulExampleKey-{id.ToString()}");

                if (getPair?.Response == null)
                {
                    return NotFound();
                }

                var value = Encoding.UTF8.GetString(getPair.Response.Value, 0, getPair.Response.Value.Length);

                var newItem = JsonSerializer.Deserialize<ConsulExampleKey>(value);
                if (newItem != null)
                {
                    return newItem;
                }

                return NotFound();
            }
        }
    }
}
