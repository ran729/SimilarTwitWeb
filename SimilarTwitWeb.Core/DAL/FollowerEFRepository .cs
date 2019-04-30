using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;
using Microsoft.EntityFrameworkCore;
using SimilarTwitWeb.Core.Interfaces;

namespace SimilarTwitWeb.Core.DAL
{
    public class FollowerEFRepository : EfRepository<Follower>, IFollowerRepository
    {
        public FollowerEFRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public async Task DeleteFollower(Follower follower)
        {
            await _dbContext.Database.ExecuteSqlCommandAsync(
                $@"delete from Followers 
                where FollowingUserId = {follower.FollowingUserId}
                and FollowedUserId = {follower.FollowedUserId}
            ");
        }
    }
}
