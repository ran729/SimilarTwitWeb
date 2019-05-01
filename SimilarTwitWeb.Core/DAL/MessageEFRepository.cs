using System.Collections.Generic;
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

        public List<Message> GetFeed(MessageFilter filter = default(MessageFilter))
        {
            var query = _dbContext.Messages.Include("User");

            if (filter.UserId.HasValue)
            {
                query = query.Where(o => _dbContext.Followers
                            .Where(f => f.FollowingUserId == filter.UserId)
                            .Any(f => f.FollowedUserId == o.UserId));

                /* THIS GENERATES THE FOLLOWING QUERY - 
                   
                   SELECT "o"."Id", "o"."CreatedAt", "o"."MessageText", "o"."UserId", "o.User"."Id", "o.User"."CreatedAt", "o.User"."UserName"
                    FROM "Messages" AS "o"
                    INNER JOIN "Users" AS "o.User" ON "o"."UserId" = "o.User"."Id"
                    WHERE EXISTS (
                    SELECT 1
                    FROM "Followers" AS "f"
                    WHERE ("f"."FollowingUserId" = @followingUserId) AND ("f"."FollowedUserId" = "o"."UserId"))
                */
            }

            query = query.OrderByDescending(o => o.CreatedAt);

            if (filter.Offset.HasValue)
            {
                query = query.Skip(filter.Offset.Value);
            }

            if (filter.Size.HasValue)
            {
                query = query.Take(filter.Size.Value);
            }

            return query.ToList(); ;
        }
    }
}
