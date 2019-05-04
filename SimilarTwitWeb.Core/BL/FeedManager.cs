using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.DAL;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.BL
{
    // ALL CALCULATIONS ON FEEDS IDEALLY WILL HAPPEN OFFLINE AND NOT REALTIME
    public class FeedManager : IFeedManager
    {
        const string GLOBAL_FEED_KEY = "global";
        private readonly InMemoryStorage _memoryStorage;
        private readonly IMessageRepository _messageRepository;

        public FeedManager(InMemoryStorage memoryStorage, IMessageRepository messageRepository)
        {
            _memoryStorage = memoryStorage;
            _messageRepository = messageRepository;
        }

        public List<Message> GetFeed(int? userId = null)
        {
            List<Message> resultFeed = null;

            if (userId.HasValue)
            {
                var key = userId.Value.ToString();

                if (!_memoryStorage.Feeds.ContainsKey(key))
                {
                    var constructedfeed = ConstructFeed(userId.Value);
                    _memoryStorage.Feeds[key] = constructedfeed;
                }

                var feed = _memoryStorage.Feeds[key];
                resultFeed = AddCelebrityMessagesToFeed(feed, userId.Value);
            }
            else
            {
                if(!_memoryStorage.Feeds.ContainsKey(GLOBAL_FEED_KEY))
                {
                    var constructedFeed = ConstructFeed();
                    _memoryStorage.Feeds[GLOBAL_FEED_KEY] = constructedFeed;
                }

                resultFeed = _memoryStorage.Feeds[GLOBAL_FEED_KEY].ToList();
            }

            return resultFeed
                .OrderByDescending(o => o.CreatedAt)
                .Take(Constants.MAX_FEED_SIZE)
                .ToList();
        }
       
        public async Task<Message> IndexMessage(Message message)
        {
            message = await _messageRepository.AddAsync(message);
            IndexToFeed(message); // IDEALLY SHOULD HAPPEN OFFLINE - NOT ON REALTIME
            return message;
        }

        public void AddFollowerToFeed(int followingUserId, int followedUserId)
        {
            var messages = _messageRepository.GetLatestMessages(followedUserId, Constants.MAX_FEED_SIZE);
            var key = followingUserId.ToString();
            var feed = _memoryStorage.Feeds.GetOrAdd(key, new ConcurrentQueue<Message>()).ToList();
            feed.AddRange(messages);
            var newFeed = feed.OrderBy(o => o.CreatedAt).Take(Constants.MAX_FEED_SIZE).ToList();
            _memoryStorage.Feeds[key] = new ConcurrentQueue<Message>(newFeed);
        }

        public void RemoveFromFeed(int followingUserId, int followedUserId)
        {
            var key = followingUserId.ToString();
            var feed = _memoryStorage.Feeds.GetOrAdd(key, new ConcurrentQueue<Message>()).ToList();
            var newFeed = feed.Where(o => o.UserId != followedUserId);
            _memoryStorage.Feeds[key] = new ConcurrentQueue<Message>(newFeed);
        }

        private void IndexToFeed(Message message)
        {
            var followers = GetFollowers(message.UserId);

            AddInFeed(_memoryStorage.Feeds, message, GLOBAL_FEED_KEY); // anyway add to global feeds

            if (IsCelebrity(message.UserId))
            {
                var key = message.UserId.ToString();
                AddInFeed(_memoryStorage.CelebrityFeeds, message, key);
            }
            else
            {
                foreach (var follower in followers)
                {
                    var key = follower.ToString();
                    AddInFeed(_memoryStorage.Feeds, message, key);
                }
            }
        }

        private List<Message> AddCelebrityMessagesToFeed(ConcurrentQueue<Message> feed, int userId)
        {
            var celebrityIds = GetCelebrtiesFollowed(userId);
            var newFeed = feed.ToList();

            foreach(var celebrityId in celebrityIds)
            {
                var key = celebrityId.ToString();
                var celebrityFeed = _memoryStorage.CelebrityFeeds.GetOrAdd(key, new ConcurrentQueue<Message>());
                newFeed.AddRange(celebrityFeed);
            }

            return newFeed;
        }

        private IEnumerable<int> GetCelebrtiesFollowed(int userId)
        {
            var exists = _memoryStorage.UserFollowers.TryGetValue(userId, out List<int> followers);

            if (!exists)
                return new List<int>();

            return followers.Where(_memoryStorage.Celebrities.Contains);
        }

        private ConcurrentQueue<Message> ConstructFeed(int? userId = null)
        {
            var feed = _messageRepository.GetFeed(new MessageFilter { UserId = userId, Size = Constants.MAX_FEED_SIZE });
            return new ConcurrentQueue<Message>(feed);
        }

        private static void AddInFeed(ConcurrentDictionary<string, ConcurrentQueue<Message>> feeds, Message message, string key)
        {
            if (!feeds.ContainsKey(key))
            {
                feeds[key] = new ConcurrentQueue<Message>();
            }

            var currentFeed = feeds[key];
            currentFeed.Enqueue(message);

            if (currentFeed.Count >= Constants.MAX_FEED_SIZE)
                currentFeed.TryDequeue(out Message oldMessage);

        }

        private bool IsCelebrity(int userId)
        {
            return _memoryStorage.Celebrities.Contains(userId);
        }

        private List<int> GetFollowers(int userId)
        {
            var exists = _memoryStorage.UserFollowers.TryGetValue(userId, out List<int> followers);
            return exists ? followers : new List<int>();
        }
    }
}
