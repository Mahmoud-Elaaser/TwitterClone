namespace TwitterClone.Service.DTOs.TweetDto
{
    public class CreateOrUpdateTweetDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public string? MediaURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
