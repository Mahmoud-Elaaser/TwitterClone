using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.RetweetDto;

namespace TwitterClone.Service.MappingProfile
{
    public class RetweetProfile : Profile
    {
        public RetweetProfile()
        {
            CreateMap<Retweet, GetRetweetDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ReverseMap();

            //CreateMap<IEnumerable<Retweet>, IEnumerable<GetRetweetDto>>()
            //.ConvertUsing((src, dest, context) => src.Select(context.Mapper.Map<GetRetweetDto>));
        }
    }
}
