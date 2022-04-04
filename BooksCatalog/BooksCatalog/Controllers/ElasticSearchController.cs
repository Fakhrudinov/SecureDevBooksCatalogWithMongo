using DataAbstraction.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace BooksCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticSearchController : ControllerBase
    {
        private IElasticSearchService _elasticService;
        private ILogger<ElasticSearchController> _logger;
        public ElasticSearchController(IElasticSearchService elasticService, ILogger<ElasticSearchController> logger)
        {
            _elasticService = elasticService;
            _logger = logger;
        }

        [HttpGet("Search/{term}")]
        public async Task<IActionResult> SearchAsync(string term)
        {
            _logger.LogInformation($"ElasticSearchController SearchAsync executed at {DateTime.UtcNow} with term={term}");

            ISearchResponse<Book> res = await _elasticService.SearchAsync(term);

            if (!res.IsValid)
            {
                _logger.LogWarning($"ElasticSearchController SearchAsync response invalid " + res.DebugInformation);
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                _logger.LogWarning($"ElasticSearchController SearchAsync response 404");
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Author/{term}")]
        public async Task<IActionResult> SearchByAuthorAsync(string term)
        {
            _logger.LogInformation($"ElasticSearchController SearchByAuthorAsync executed at {DateTime.UtcNow} with term={term}");
            ISearchResponse<Book> res = await _elasticService.SearchByAuthorAsync(term);

            if (!res.IsValid)
            {
                _logger.LogWarning($"ElasticSearchController SearchByAuthorAsync response invalid " + res.DebugInformation);
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                _logger.LogWarning($"ElasticSearchController SearchByAuthorAsync response 404");
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Book/Name/{term}")]
        public async Task<IActionResult> SearchByBookNameAsync(string term)
        {
            _logger.LogInformation($"ElasticSearchController SearchByBookNameAsync executed at {DateTime.UtcNow} with term={term}");
            ISearchResponse<Book> res = await _elasticService.SearchByBookNameAsync(term);

            if (!res.IsValid)
            {
                _logger.LogWarning($"ElasticSearchController SearchByBookNameAsync response invalid " + res.DebugInformation);
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                _logger.LogWarning($"ElasticSearchController SearchByBookNameAsync response 404");
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Book/Description/{term}")]
        public async Task<IActionResult> SearchByBookDescriptionAsync(string term)
        {
            _logger.LogInformation($"ElasticSearchController SearchByBookDescriptionAsync executed at {DateTime.UtcNow} with term={term}");
            ISearchResponse<Book> res = await _elasticService.SearchByBookDescriptionAsync(term);

            if (!res.IsValid)
            {
                _logger.LogWarning($"ElasticSearchController SearchByBookDescriptionAsync response invalid " + res.DebugInformation);
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                _logger.LogWarning($"ElasticSearchController SearchByBookDescriptionAsync response 404");
                return NotFound();
            }

            return Ok(res.Documents);
        }
    }
}
