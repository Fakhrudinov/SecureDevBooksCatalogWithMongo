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
        public BooksController(IBooksRepository repository)
        {
            _repository = repository;
        }


        [HttpGet]
        public async Task<List<Book>> Get() => await _repository.GetAsync();


        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
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
            BookRequestValidation validator = new BookRequestValidation();
            var response = new ValidationResponseModel();

            var validationResult = validator.Validate(newBook);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            Book book = MapClassBookRequest(newBook);

            await _repository.CreateAsync(book);
            return CreatedAtAction(nameof(Get), new { id = book.Id }, book);
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookRequest newData)
        {
            BookRequestValidation validator = new BookRequestValidation();
            var response = new ValidationResponseModel();

            var validationResult = validator.Validate(newData);
            if (!validationResult.IsValid)
            {
                response = SetResponseFromValidationResult(validationResult, response);

                return BadRequest(response);
            }

            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                response.IsValid = false;
                response.ValidationMessages.Add($"F_200.2 Указанный Id не найден");
                return NotFound(response);
            }

            Book updateBook = MapClassBookRequest(newData);
            updateBook.Id = book.Id;

            await _repository.UpdateAsync(id, updateBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                var response = new ValidationResponseModel();
                response.IsValid = false;
                response.ValidationMessages.Add($"F_200.3 Указанный Id не найден");
                return NotFound(response);
            }

            await _repository.RemoveAsync(id);

            return NoContent();
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
