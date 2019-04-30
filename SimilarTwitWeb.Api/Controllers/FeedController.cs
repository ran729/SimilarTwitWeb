using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public IEnumerable<FeedItem> GetGlobalFeed()
        {
            return _messageRepository.GetGlobalFeed().Select(ToFeedItem);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetPersonalFeed(int userId)
        {
            var userExists = await _userRepository.DoesUserExist(userId);

            if (!userExists)
            {
                return BadRequest($"Can't show feed of userid - {userId}, user does not exist.");
            }

            var filter = new MessageFilter { UserId = userId };
            var feed = _messageRepository.GetPersonalFeed(filter).Select(ToFeedItem);
            return Ok(feed);
        }

        private FeedItem ToFeedItem(Message msg)
        {
            return new FeedItem
            {
                MessageId = msg.Id,
                UserId = msg.UserId,
                UserName = msg.User.UserName,
                Message = msg.MessageText,
                CreatedAt = DateTime.Now,
            };
        }
    }
}
