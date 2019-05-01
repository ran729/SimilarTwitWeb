using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Core.Exceptions;
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
            // assuming followingUserId user exists.
            var userExists = await _userRepository.DoesUserExist(follower.FollowedUserId);
           
            if (!userExists)
            {
                return BadRequest($"Can't follow user#{follower.FollowedUserId}, user does not exist.");
            }

            try
            {
                await _followersRepository.AddAsync(follower);
            }
            catch(UniqueRowAlreadyExistsException)
            {
                return BadRequest($"user#{follower.FollowingUserId} already follows user#{follower.FollowedUserId}");
            }

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
