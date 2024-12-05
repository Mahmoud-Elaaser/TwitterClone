namespace TwitterClone.Service.DTOs.QuoteDto
{
    public class GetQuoteDto
    {
        public int QuoteId { get; set; }
        public int UserId { get; set; }
        public int? TweetId { get; set; }
        public string Content { get; set; }
        public string? MediaURL { get; set; }
    }
}
