namespace TwitterClone.Data.Entities
{
    public class Tweet
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? MediaURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();
    }

}
