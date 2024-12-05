namespace TwitterClone.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public DateTime Createdat { get; set; } = DateTime.UtcNow;

        public Tweet? Tweet { get; set; }
        public User User { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();
    }
}