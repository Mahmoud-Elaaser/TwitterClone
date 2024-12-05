namespace TwitterClone.Service.DTOs.LikeDto
{
    public class GetLikeDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }
        public DateTime LikedAt { get; set; }
    }
}

