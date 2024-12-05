using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.QuoteDto;

namespace TwitterClone.Service.MappingProfile
{
    public class QuoteProfile : Profile
    {
        public QuoteProfile()
        {
            CreateMap<GetQuoteDto, Quote>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuoteId))
                .ForMember(dest => dest.MediaURL, opt => opt.MapFrom(src => src.MediaURL))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ReverseMap();

            CreateMap<Quote, CreateOrUpdateQuoteDto>()
                .ForMember(dest => dest.QuoteId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MediaURL, opt => opt.MapFrom(src => src.MediaURL))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.TweetId, opt => opt.MapFrom(src => src.TweetId))
                .ReverseMap();
        }


    }
}
