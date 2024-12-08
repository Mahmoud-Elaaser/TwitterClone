using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TwitterClone.Service.Services.Implementations;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service
{
    public static class ModuleServiceDependencies
    {
        public static void AddServiceDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITweetService, TweetService>();
            services.AddScoped<IMuteUserService, MuteUserService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IQuoteService, QuoteService>();
            services.AddScoped<IRetweetService, RetweetService>();
            services.AddScoped<IBookmarkService, BookmarkService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFollowService, FollowService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<IEmailService, EmailService>();

            /// Register automapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }
    }
}
