using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.LikeDto;

namespace TwitterClone.Service.MappingProfile
{
    public class LikeProfile : Profile
    {
        public LikeProfile()
        {
            CreateMap<CreateLikeDto, Like>()
                .ForMember(dest => dest.LikedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<GetLikeDto, Like>()
                .ForMember(dest => dest.LikedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
