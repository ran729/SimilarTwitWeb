using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Api.ApiObjects;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private IMessageRepository _messageRepository;
        private IUserRepository _userRepository;

        public FeedController(IMessageRepository messageRepository, IUserRepository userRepository)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
        }

        [HttpGet("global")]
        public IEnumerable<ApiFeedItem> GetGlobalFeed([FromQuery]int? size = null, [FromQuery]int? offset = null)
        {
            var filter = new MessageFilter { Size = size, Offset = offset };
            return _messageRepository.GetFeed(filter).Select(msg => new ApiFeedItem(msg));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetPersonalFeed(int userId, [FromQuery]int? size=null, [FromQuery]int? offset=null)
        {
            var userExists = await _userRepository.DoesUserExist(userId);

            if (!userExists)
            {
                return BadRequest($"Can't show feed of user#{userId}, user does not exist.");
            }

            var filter = new MessageFilter { UserId = userId, Size = size, Offset = offset };
            var feed = _messageRepository.GetFeed(filter).Select(msg => new ApiFeedItem(msg));
            return Ok(feed);
        }
    }
}
