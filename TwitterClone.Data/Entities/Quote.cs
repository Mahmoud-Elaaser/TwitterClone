namespace TwitterClone.Data.Entities
{
    public class Quote
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string? MediaURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }
        public User? User { get; set; }

        public int? TweetId { get; set; }
        public virtual Tweet? Tweet { get; set; }

        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public int NumberOfBookmarks { get; set; }
        public int NumberOfRetweets { get; set; }


        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();

    }
}
