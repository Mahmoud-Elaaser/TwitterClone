using TwitterClone.Data.Enums;

namespace TwitterClone.Data.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public NotificationType Type { get; set; }
        public EntityType? RelatedEntityType { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }

}
