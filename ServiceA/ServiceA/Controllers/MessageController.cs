using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceA.Service;

namespace ServiceA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
         private readonly RabbitMQClientService _rabbitMQClientService;

        public MessageController(RabbitMQClientService rabbitMQClientService)
        {
            _rabbitMQClientService = rabbitMQClientService;
        }

        [HttpPost]
        public IActionResult SendMessage([FromBody] string message)
        {
            _rabbitMQClientService.SendMessage(message);
            return Ok("Message sent");
        }
    }
}
