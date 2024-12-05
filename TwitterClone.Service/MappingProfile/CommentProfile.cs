using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.CommentDto;

namespace TwitterClone.Service.MappingProfile
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<GetCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Createdat, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ReverseMap();

            CreateMap<CreateOrUpdateCommentDto, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();

            //CreateMap<IEnumerable<Comment>, IEnumerable<GetCommentDto>>()
            //.ConvertUsing((src, dest, context) => src.Select(context.Mapper.Map<GetCommentDto>));
        }
    }
}
