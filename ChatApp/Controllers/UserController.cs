using Microsoft.AspNetCore.Mvc;
using ChatApp.Services;
using ChatApp.Model;
using MongoDB.Bson;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController :ControllerBase
    {
        private readonly UserServices _booksService;

        public UserController(UserServices booksService) =>
            _booksService = booksService;

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(User newBook)
        {
            try
            {
                await _booksService.CreateAsync(newBook);

                return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
     
        }

        [HttpPost("Login")]
        public async  Task<IActionResult> Login(UserLogin User1)
        {
            try
            {
                Console.WriteLine(User1.Username+" "+User1.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedBook)
        {
            try
            {
                var book = await _booksService.GetAsync(id);

                if (book is null)
                {
                    return NotFound();
                }

                updatedBook.Id = book.Id;

                await _booksService.UpdateAsync(id, updatedBook);

                return NoContent();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var book = await _booksService.GetAsync(id);

                if (book is null)
                {
                    return NotFound();
                }

                await _booksService.RemoveAsync(id);

                return NoContent();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }
    }
}
