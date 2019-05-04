using System.Collections.Generic;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IFeedManager
    {
        Task<Message> IndexMessage(Message message);
        List<Message> GetFeed(int? userId = null);
        void AddFollowerToFeed(int followingUserId, int followedUserId);
        void RemoveFromFeed(int followingUserId, int followedUserId);
    }
}