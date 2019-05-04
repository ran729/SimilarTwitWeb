using System.Collections.Generic;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> AddAsync(Message entity);
        List<Message> GetFeed(MessageFilter filter = new MessageFilter());
        List<Message> GetLatestMessages(int followedUserId, int mAX_FEED_SIZE);
    }
}
