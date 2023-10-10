using AutoMapper;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.Role;

namespace SimpleWebApp.Mapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
