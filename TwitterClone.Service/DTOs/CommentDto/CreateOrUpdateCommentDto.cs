namespace TwitterClone.Service.DTOs.CommentDto
{
    public class CreateOrUpdateCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
    }

}
