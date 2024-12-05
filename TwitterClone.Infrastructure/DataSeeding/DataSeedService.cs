//using TwitterClone.Data.Entities;
//using TwitterClone.Infrastructure.Context;

//namespace TwitterClone.Data
//{
//    public class DataSeedService
//    {
//        private readonly ApplicationDbContext _context;

//        public DataSeedService(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task SeedDataAsync()
//        {
//            if (!_context.Users.Any())
//            {
//                var users = GetSampleUsers();
//                _context.Users.AddRange(users);
//                await _context.SaveChangesAsync();
//            }

//            if (!_context.Tweets.Any())
//            {
//                var tweets = GetSampleTweets();
//                _context.Tweets.AddRange(tweets);
//                await _context.SaveChangesAsync();
//            }

//            if (!_context.Quotes.Any())
//            {
//                var quotes = GetSampleQuotes();
//                _context.Quotes.AddRange(quotes);
//                await _context.SaveChangesAsync();
//            }

//            if (!_context.Comments.Any())
//            {
//                var comments = GetSampleComments();
//                _context.Comments.AddRange(comments);
//                await _context.SaveChangesAsync();
//            }
//        }


//        private List<User> GetSampleUsers()
//        {
//            return new List<User>
//            {
//                new User { Bio = "CS student", DateJoined = DateTime.Now },
//                new User { Bio = "IT student", DateJoined = DateTime.Now },
//                new User { Bio = "CSE student", DateJoined = DateTime.Now }
//            };
//        }

//        private List<Tweet> GetSampleTweets()
//        {
//            return new List<Tweet>
//            {
//                new Tweet
//                {
//                    UserId = 1,
//                    Content = "Hello world! This is my first tweet.",
//                    Likes = new List<Like>(),
//                    Comments = new List<Comment>()
//                },
//                new Tweet
//                {
//                    UserId = 2,
//                    Content = "Excited to join TwitterClone!",
//                    Likes = new List<Like>(),
//                    Comments = new List<Comment>()
//                }
//            };
//        }

//        private List<Quote> GetSampleQuotes()
//        {
//            return new List<Quote>
//            {
//                new Quote { UserId = 1, Content = "Quote Content 1", Likes = new List<Like>(), Comments = new List<Comment>() },
//                new Quote { UserId = 2, Content = "Quote Content 2", Likes = new List<Like>(), Comments = new List<Comment>() }
//            };
//        }

//        private List<Comment> GetSampleComments()
//        {
//            var quote1 = _context.Quotes.FirstOrDefault(q => q.Content == "Quote Content 1");
//            var tweet1 = _context.Tweets.FirstOrDefault(t => t.Content == "Tweet Content 1");

//            if (quote1 == null || tweet1 == null)
//            {
//                Console.WriteLine("Missing required quotes or tweets. Skipping comment seeding.");
//                return new List<Comment>(); // Return an empty list if dependencies are missing
//            }

//            return new List<Comment>
//            {
//                new Comment { QuoteId = quote1.Id, UserId = quote1.UserId, Content = "Comment on Quote 1" },
//                new Comment { TweetId = tweet1.Id, UserId = tweet1.UserId, Content = "Comment on Tweet 1" }
//            };
//        }



//        private List<Follow> GetSampleFollows()
//        {
//            return new List<Follow>
//            {
//                new Follow { FollowerId = 1, FollowingId = 2 },
//                new Follow { FollowerId = 2, FollowingId = 3 }
//            };
//        }
//    }
//}
