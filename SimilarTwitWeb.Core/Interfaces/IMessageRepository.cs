using System.Collections.Generic;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> AddAsync(Message entity);
        List<Message> GetGlobalFeed(MessageFilter filter = new MessageFilter());
        List<Message> GetPersonalFeed(MessageFilter filter = new MessageFilter());
    }
}
