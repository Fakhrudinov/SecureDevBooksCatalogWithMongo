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
        public ElasticSearchController(IElasticSearchService elasticService)
        {
            _elasticService = elasticService;
        }

        [HttpGet("Search/{term}")]
        public async Task<IActionResult> SearchAsync(string term)
        {
            ISearchResponse<Book> res = await _elasticService.SearchAsync(term);

            if (!res.IsValid)
            {
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Author/{term}")]
        public async Task<IActionResult> SearchByAuthorAsync(string term)
        {
            ISearchResponse<Book> res = await _elasticService.SearchByAuthorAsync(term);

            if (!res.IsValid)
            {
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Book/Name/{term}")]
        public async Task<IActionResult> SearchByBookNameAsync(string term)
        {
            ISearchResponse<Book> res = await _elasticService.SearchByBookNameAsync(term);

            if (!res.IsValid)
            {
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                return NotFound();
            }

            return Ok(res.Documents);
        }

        [HttpGet("Search/By/Book/Description/{term}")]
        public async Task<IActionResult> SearchByBookDescriptionAsync(string term)
        {
            ISearchResponse<Book> res = await _elasticService.SearchByBookDescriptionAsync(term);

            if (!res.IsValid)
            {
                //throw new InvalidOperationException(res.DebugInformation);
                return BadRequest();
            }

            if (res.Documents.Count == 0)
            {
                return NotFound();
            }

            return Ok(res.Documents);
        }
    }
}
