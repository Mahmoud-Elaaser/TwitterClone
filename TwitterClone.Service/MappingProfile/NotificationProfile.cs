using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs;

namespace TwitterClone.Service.MappingProfile
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.NotificationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.SenderId, opt => opt.MapFrom(src => src.SenderId))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Message))
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead))
                .ReverseMap();
        }
    }

}
