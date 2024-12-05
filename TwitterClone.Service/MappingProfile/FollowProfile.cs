using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.FollowDto;

namespace TwitterClone.Service.MappingProfile
{
    public class FollowProfile : Profile
    {
        public FollowProfile()
        {
            CreateMap<CreateFollowDto, Follow>().ReverseMap();

            CreateMap<Follow, FollowerDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Follower.Id))
            .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Follower.Bio))
            .ReverseMap();
        }
    }
}
