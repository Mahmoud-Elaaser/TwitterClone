namespace TwitterClone.Service.DTOs.TweetDto
{
    public class GetTweetDto
    {
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
