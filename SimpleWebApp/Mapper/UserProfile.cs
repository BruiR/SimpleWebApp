using AutoMapper;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.User;

namespace SimpleWebApp.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UserDto>()
                .ForMember(dist => dist.Roles, opts => opts.MapFrom(src => src.Roles));
        }
    }
}
