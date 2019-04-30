using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User entity);
        Task<bool> DoesUserExist(int userId);
    }
}
