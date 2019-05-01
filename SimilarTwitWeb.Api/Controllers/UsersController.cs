using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] string userName)
        {
            var newUser = new User { UserName = userName };

            if (!IsUserValid(newUser, out string reason))
            {
                return BadRequest("user was not created, reason:" + reason);
            }

            var createdUser = await _userRepository.AddAsync(newUser);
            return Ok(createdUser);
        }

        private bool IsUserValid(User newUser, out string reason)
        {
            reason = string.Empty;

            if (newUser.UserName.Length > 15)
            {
                reason = "user name cant be longer then 15 characters.";
                return false;
            }

            return true; 
        }
    }
}
