using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IFollowerRepository
    {
        Task<Follower> AddAsync(Follower entity);
        Task DeleteFollower(Follower entity);
    }
}
