using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FollowersController : ControllerBase
    {
        private readonly IFollowerRepository _followersRepository;
        private readonly IUserRepository _userRepository;

        public FollowersController(IFollowerRepository followersRepository, IUserRepository userRepository)
        {
            _followersRepository = followersRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Follow([FromBody] Follower follower)
        {
            // assuming followingUserId users exists.
            var userExists = await _userRepository.DoesUserExist(follower.FollowedUserId);
           
            if (!userExists)
            {
                return BadRequest($"Can't follow {follower.FollowedUserId}, user does not exist.");
            }

            await _followersRepository.AddAsync(follower);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Unfollow([FromBody] Follower follower)
        {
            await _followersRepository.DeleteFollower(follower);
            return Ok(); 
        } 
    }
}
