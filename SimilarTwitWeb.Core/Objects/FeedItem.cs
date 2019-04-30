using System;
namespace SimilarTwitWeb.Core.Objects
{
    public class FeedItem
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public int MessageId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
