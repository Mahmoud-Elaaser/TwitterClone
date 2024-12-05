using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs.UserDto;

namespace TwitterClone.Service.MappingProfile
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateOrUpdateUserDto, User>().ReverseMap();

            CreateMap<User, GetUserDto>().ReverseMap();

            CreateMap<GetUserDto, User>().ReverseMap();

        }
    }
}
