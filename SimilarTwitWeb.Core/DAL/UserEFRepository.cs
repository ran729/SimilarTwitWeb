using System.Threading.Tasks;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.DAL
{
    public class UserEFRepository : EfRepository<User>, IUserRepository
    {
        public UserEFRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> DoesUserExist(int userId)
        {
            var result = await _dbContext.Users.FindAsync(userId);
            return result != null;
        }
    }
}
