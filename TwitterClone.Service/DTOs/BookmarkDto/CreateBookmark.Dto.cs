namespace TwitterClone.Service.DTOs.BookmarkDto
{
    public class CreateBookmarkDto
    {
        public int BookmarkId { get; set; }
        public int UserId { get; set; }
        public int TweetId { get; set; }

    }
}
