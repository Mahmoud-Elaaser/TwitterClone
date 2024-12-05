namespace TwitterClone.Data.Entities
{
    public class Bookmark
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public DateTime BookmarkedAt { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        public Tweet? Tweet { get; set; }

    }
}
