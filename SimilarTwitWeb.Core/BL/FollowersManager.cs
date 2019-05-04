using System.Collections.Generic;
using System.Threading.Tasks;
using SimilarTwitWeb.Core.DAL;
using SimilarTwitWeb.Core.Interfaces;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Core.BL
{
    public class FollowersManager : IFollowersManager
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly InMemoryStorage _inMemoryStorage;
        private readonly IFeedManager _feedManager;

        public FollowersManager(IFollowerRepository followerRepository, InMemoryStorage inMemoryStorage, IFeedManager feedManager)
        {
            _followerRepository = followerRepository;
            _inMemoryStorage = inMemoryStorage;
            _feedManager = feedManager;
        }

        public async Task Follow(Follower follower)
        {
            await _followerRepository.AddAsync(follower);

            if(!_inMemoryStorage.UserFollowers.TryGetValue(follower.FollowedUserId, out List<int> followers))
            {
                followers = new List<int>();
                _inMemoryStorage.UserFollowers[follower.FollowedUserId] = followers;
            }

            followers.Add(follower.FollowingUserId);

            if (followers.Count >= Constants.CELEBRITY_FOLLOWERS_MIN_AMOUNT)
            {
                _inMemoryStorage.Celebrities.Add(follower.FollowedUserId);
            }

            _feedManager.AddFollowerToFeed(follower.FollowingUserId, follower.FollowedUserId);
        }

        public async Task Unfollow(Follower follower)
        {
            await _followerRepository.DeleteFollower(follower);

            if (!_inMemoryStorage.UserFollowers.TryGetValue(follower.FollowedUserId, out List<int> followers))
            {
                followers = new List<int>();
                _inMemoryStorage.UserFollowers[follower.FollowedUserId] = followers;
            }

            followers.Remove(follower.FollowingUserId);

            if (followers.Count < Constants.CELEBRITY_FOLLOWERS_MIN_AMOUNT)
            {
                _inMemoryStorage.Celebrities.Remove(follower.FollowedUserId);
            }

            _feedManager.RemoveFromFeed(follower.FollowingUserId, follower.FollowedUserId);
        }
    }
}
