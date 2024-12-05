namespace TwitterClone.Data.Entities
{
    public class MuteUser
    {
        public int Id { get; set; }
        public int MutedById { get; set; } /// The user who muted another user
        public int MutedUserId { get; set; } /// The user being muted

        public User MutedBy { get; set; } /// Navigation to the muting user
        public User MutedUser { get; set; } /// Navigation to the muted user
    }

}
