using System.Collections.Generic;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SimilarTwitWeb.Core.DAL
{

    public class MessageEFRepository : EfRepository<Message>, IMessageRepository
    {
        public MessageEFRepository(DatabaseContext dbContext) : base(dbContext)
        {
        }

        public List<Message> GetGlobalFeed(MessageFilter filter = new MessageFilter())
        {
            var messages = _dbContext.Messages
              .Include("User").ToList();

            return messages;
        }

        public List<Message> GetPersonalFeed(MessageFilter filter = new MessageFilter())
        {
            var messages = _dbContext.Messages
                .Include("User")
                .Where(o => _dbContext.Followers
                            .Where(f => f.FollowingUserId == filter.UserId)
                            .Any(f => f.FollowedUserId == o.UserId))
                            .ToList();

            /* THIS GENERATES THE FOLLOWING QUERY - 
                
                SELECT "o"."Id", "o"."CreatedAt", "o"."MessageText", "o"."UserId", "o.User"."Id", "o.User"."CreatedAt", "o.User"."UserName"
                FROM "Messages" AS "o"
                INNER JOIN "Users" AS "o.User" ON "o"."UserId" = "o.User"."Id"
                WHERE EXISTS (
                SELECT 1
                FROM "Followers" AS "f"
                WHERE ("f"."FollowingUserId" = @followingUserId) AND ("f"."FollowedUserId" = "o"."UserId"))
            */

            return messages;
        }
    }
}
