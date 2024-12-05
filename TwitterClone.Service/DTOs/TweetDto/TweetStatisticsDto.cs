namespace TwitterClone.Service.DTOs.TweetDto
{
    public class TweetStatisticsDto
    {
        public int TweetId { get; set; }
        public int Likes { get; set; }
        public int Comments { get; set; }
        public int Bookmarks { get; set; }
        public int Retweets { get; set; }
    }

}
