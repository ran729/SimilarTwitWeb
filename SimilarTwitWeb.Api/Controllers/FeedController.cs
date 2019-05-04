using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Api.ApiObjects;
using SimilarTwitWeb.Core.BL;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedController : ControllerBase
    {
        private IFeedManager _feedManager;
        private IUserRepository _userRepository;

        public FeedController(IFeedManager feedManager, IUserRepository userRepository)
        {
            _feedManager = feedManager;
            _userRepository = userRepository;
        }

        [HttpGet("global")]
        public IEnumerable<ApiFeedItem> GetGlobalFeed()
        {
            return _feedManager.GetFeed().Select(msg => new ApiFeedItem(msg));
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetPersonalFeed(int userId, [FromQuery]int? size=null, [FromQuery]int? offset=null)
        {
            var userExists = await _userRepository.DoesUserExist(userId);

            if (!userExists)
            {
                return BadRequest($"Can't show feed of user#{userId}, user does not exist.");
            }

            var feed = _feedManager.GetFeed(userId).Select(msg => new ApiFeedItem(msg));
            return Ok(feed);
        }
    }
}
