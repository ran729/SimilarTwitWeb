namespace SimilarTwitWeb.Core.Objects
{
    public class Message : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string MessageText { get; set; }
    }
}
