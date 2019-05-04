using System;
using SimilarTwitWeb.Core.Objects;

namespace SimilarTwitWeb.Api.ApiObjects
{
    public class ApiFeedItem
    {
        public ApiFeedItem()
        {
        }

        public ApiFeedItem(Message msg)
        {
            MessageId = msg.Id;
            UserId = msg.UserId;
            Message = msg.MessageText;
            CreatedAt = DateTime.Now;
        }

        public int UserId { get; set; }
        public int MessageId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
