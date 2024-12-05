namespace TwitterClone.Service.DTOs
{
    public class NotificationDto
    {
        public int NotificationId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
