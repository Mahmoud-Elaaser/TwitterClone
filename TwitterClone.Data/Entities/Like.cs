namespace TwitterClone.Data.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        public virtual User User { get; set; }
        public virtual Tweet Tweet { get; set; }
    }
}
