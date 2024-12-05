using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.BookmarkDto;

namespace TwitterClone.Service.MappingProfile
{
    public class BookmarkProfile : Profile
    {
        public BookmarkProfile()
        {
            CreateMap<GetBookmarkDto, Bookmark>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookmarkId))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BookmarkedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<Bookmark, CreateBookmarkDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.BookmarkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ReverseMap();
        }
    }
}
