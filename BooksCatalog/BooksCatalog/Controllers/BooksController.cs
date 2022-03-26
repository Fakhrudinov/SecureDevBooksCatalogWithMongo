using DataAbstraction.Interfaces;
using DataAbstraction.Models;
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
        public async Task<List<Book>> Get() =>
        await _repository.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookRequest newBook)
        {
            Book book = MapClassBookRequest(newBook);

            await _repository.CreateAsync(book);

            return CreatedAtAction(nameof(Get), new { id = book.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookRequest newData)
        {
            var book = await _repository.GetAsync(id);

            if (book is null)
            {
                return NotFound();
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
                return NotFound();
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
    }
}
