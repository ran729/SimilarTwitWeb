using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;
using SimilarTwitWeb.Core.Interfaces;
using System.Linq;

namespace SimilarTwitWeb.Core.DAL
{
    public class FollowerEFRepository : EfRepository<Follower>, IFollowerRepository
    {
        public FollowerEFRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteFollower(Follower follower)
        {
            _dbContext.Followers.RemoveRange(
                _dbContext.Followers.Where(f => f.FollowedUserId == follower.FollowedUserId &&
                                                f.FollowingUserId == follower.FollowingUserId)
            );

            await _dbContext.SaveChangesAsync();
        }
    }
}
