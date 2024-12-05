using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.TweetDto;

namespace TwitterClone.Service.MappingProfile
{
    public class TweetProfile : Profile
    {
        public TweetProfile()
        {
            CreateMap<GetTweetDto, Tweet>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();

            CreateMap<Tweet, CreateOrUpdateTweetDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.MediaURL, opt => opt.MapFrom(src => src.MediaURL))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();
        }
    }
}
