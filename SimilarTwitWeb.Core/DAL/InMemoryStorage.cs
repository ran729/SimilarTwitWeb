using System.Collections.Concurrent;
using System.Collections.Generic;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.DAL
{
    public class InMemoryStorage
    {
        // ASSUMING THERE IS A POLICY OF OLD MESSAGES DISAPPEARING (10 days... or so)
        public readonly ConcurrentDictionary<string, ConcurrentQueue<Message>> Feeds = new ConcurrentDictionary<string, ConcurrentQueue<Message>>();
        public readonly ConcurrentDictionary<string, ConcurrentQueue<Message>> CelebrityFeeds = new ConcurrentDictionary<string, ConcurrentQueue<Message>>();

        public readonly ConcurrentDictionary<int, List<int>> UserFollowers = new ConcurrentDictionary<int, List<int>>();
        public readonly HashSet<int> Celebrities = new HashSet<int>();
    }
}
