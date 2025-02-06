using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GPTLearning.Services;
using GPTLearning.Services.Interfaces;

namespace GPTLearning.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IGPTService _gptService;
        public ChatController(IGPTService gptService)
        {
            _gptService = gptService;
        }
        //test
        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return BadRequest("Input cannot be empty.");

            var response = await _gptService.GetResponseFromOpenAI(userInput);
            return Ok(new { Response = response });
        }
    }
}
