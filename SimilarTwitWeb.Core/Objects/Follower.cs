namespace SimilarTwitWeb.Core.Objects
{
    public class Follower : BaseEntity
    {
        public int FollowingUserId { get; set; }
        public int FollowedUserId { get; set; }
    }
}
