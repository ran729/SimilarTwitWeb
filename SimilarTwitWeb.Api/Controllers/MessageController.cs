using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IFeedManager _feedManager;

        public MessageController(IFeedManager feedManager)
        {
            _feedManager = feedManager;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Message message)
        {
            // assuming user exists.

            if (!IsMessageValid(message, out string reason))
            {
                return BadRequest("Cannot post this message, reason: " + reason);
            }

            var createdMessage = await _feedManager.IndexMessage(message);
            return Ok(createdMessage);
        }

        private bool IsMessageValid(Message message, out string reason)
        {
            reason = string.Empty;

            if (message.MessageText.Length > 150)
            {
                reason = "message text cant be longer then 150 characters.";
                return false;
            }

            return true;
        }
    }
}
