using ChatApp.Model;
using ChatApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        private readonly MessageServices _messageService;

        public MessageController(MessageServices messageService) =>
            _messageService = messageService;

        [HttpGet]
        public async Task<List<MessageModel>> Get() =>
           await _messageService.GetAsync();

        [HttpPost]
        public async Task<IActionResult> Post(MessageModel newMess)
        {
            try
            {
                DateTimeOffset now = (DateTimeOffset)DateTime.UtcNow;
                newMess.TimeStamp = now.ToUnixTimeSeconds(); 
                await _messageService.CreateAsync(newMess);

                return CreatedAtAction(nameof(Get), new { id = newMess.Id }, newMess);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
