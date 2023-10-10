using AutoMapper;
using SimpleWebApp.Domain.Models;
using SimpleWebApp.DTOs.AuthorizedPerson;

namespace SimpleWebApp.Mapper
{
    public class AuthorizedPersonProfile : Profile
    {
        public AuthorizedPersonProfile()
        {
            CreateMap<AuthorizedPerson, AuthorizedPersonDto>().ReverseMap();
        }
    }
}
