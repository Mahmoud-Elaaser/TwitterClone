
using Microsoft.AspNetCore.Identity;

namespace TwitterClone.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverPhoto { get; set; }
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;


        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
        public ICollection<Retweet> Retweets { get; set; } = new List<Retweet>();
        public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Bookmark> Bookmarks { get; set; } = new List<Bookmark>();
        public ICollection<User> Followings { get; set; } = new List<User>();
        public ICollection<User> Followers { get; set; } = new List<User>();


        public ICollection<MuteUser> MutedUsers { get; set; } /// Users muted by this user
        public ICollection<MuteUser> MutedByUsers { get; set; } /// Users who muted this user
        public ICollection<BlockUser> BlockedUsers { get; set; } /// Users blocked by this user
        public ICollection<BlockUser> BlockedByUsers { get; set; } /// Users who blocked this user
        public ICollection<Notification> SentNotifications { get; set; } // Notifications sent by this user
        public ICollection<Notification> ReceivedNotifications { get; set; } // Notifications received by this user

    }
}
