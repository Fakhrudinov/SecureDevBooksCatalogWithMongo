using BookObjectsGenerator;
using DataAbstraction.Interfaces;
using DataAbstraction.Models;
using DataValidationService;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace BooksCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private IBooksRepository _repository;
        private IElasticSearchService _elasticService;
        private ILogger<BooksController> _logger;

        public BooksController(IBooksRepository repository, IElasticSearchService elasticService, ILogger<BooksController> logger)
        {
            _repository = repository;
            _elasticService = elasticService;
            _logger = logger;
        }

        [HttpPost("Create/NewBooks/{count}")]
        public async Task<ActionResult<IEnumerable<Book>>> GetNewBooks(int count)
        {
            _logger.LogInformation($"BooksController GetNewBooks executed at {DateTime.UtcNow} with count={count}");
            Book[] books = BookGenerator.GetNewBooksArray(count);

            foreach (Book book in books)
            {
                await _repository.CreateAsync(book);
                Book findId = (Book)CreatedAtAction(nameof(Get), new { id = book.Id }, book).Value;
                book.Id = findId.Id;
            }

            await _elasticService.IndexManyBooksAsync(books);

            return Ok(books);
        }


        [HttpGet]
        public async Task<List<Book>> Get()
        {
            _logger.LogInformation($"BooksController Get executed at {DateTime.UtcNow}");
            return await _repository.GetAsync();
        } 


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            _logger.LogInformation($"BooksController Get by id executed at {DateTime.UtcNow} with id={id}");
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                _logger.LogWarning($"BooksController Get by id={id} response 404");
                var response = new ValidationResponseModel();
                response.IsValid = false;
                response.ValidationMessages.Add($"F_200.1 Указанный Id не найден");
                return NotFound(response);
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookRequest newBook)
        {
            _logger.LogInformation($"BooksController Post executed at {DateTime.UtcNow}");
            BookRequestValidation validator = new BookRequestValidation();
            var response = new ValidationResponseModel();

            var validationResult = validator.Validate(newBook);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"BooksController Post validation fail.");
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            Book book = MapClassBookRequest(newBook);

            await _repository.CreateAsync(book);
            Book findId = (Book)CreatedAtAction(nameof(Get), new { id = book.Id }, book).Value;
            book.Id = findId.Id;

            await _elasticService.IndexBookAsync(book);

            return Ok(book);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookRequest newData)
        {
            _logger.LogInformation($"BooksController Update executed at {DateTime.UtcNow} with id={id}");
            BookRequestValidation validator = new BookRequestValidation();
            var response = new ValidationResponseModel();

            var validationResult = validator.Validate(newData);
            if (!validationResult.IsValid)
            {
                _logger.LogWarning($"BooksController Update validation fail.");
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                _logger.LogWarning($"BooksController Update by id={id} response 404");
                response.IsValid = false;
                response.ValidationMessages.Add($"F_200.2 Указанный Id не найден");
                return NotFound(response);
            }

            Book updateBook = MapClassBookRequest(newData);
            updateBook.Id = book.Id;
            
            await _repository.UpdateAsync(id, updateBook);

            await _elasticService.UpdateBookAsync(id, updateBook);

            return Ok(updateBook);
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation($"BooksController Delete executed at {DateTime.UtcNow} with id={id}");
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                _logger.LogWarning($"BooksController Delete by id={id} response 404");
                var response = new ValidationResponseModel();
                response.IsValid = false;
                response.ValidationMessages.Add($"F_200.3 Указанный Id не найден");
                return NotFound(response);
            }

            await _repository.RemoveAsync(id);

            await _elasticService.DeleteBookByIndexAsync(id);

            return Ok();
        }


        private Book MapClassBookRequest(BookRequest book)
        {
            return new Book
            {
                Author = book.Author,
                Category = book.Category,
                Description = book.Description,
                Name = book.Name,
                Price = book.Price
            };
        }
        private ValidationResponseModel SetResponseFromValidationResult(ValidationResult validationResultAsync, ValidationResponseModel response)
        {
            List<string> ValidationMessages = new List<string>();

            response.IsValid = false;
            foreach (ValidationFailure failure in validationResultAsync.Errors)
            {
                ValidationMessages.Add(failure.ErrorCode + " " + failure.ErrorMessage);
            }
            response.ValidationMessages = ValidationMessages;

            return response;
        }
    }
}
