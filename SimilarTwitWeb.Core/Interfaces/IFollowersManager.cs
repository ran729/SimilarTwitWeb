using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IFollowersManager
    {
        Task Follow(Follower follower);
        Task Unfollow(Follower follower);
    }
}