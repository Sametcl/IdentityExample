using AutoMapper;
using IdentitiyExample.DTOs;
using IdentitiyExample.Models;
using IdentityExample.DTOs;

namespace IdentitiyExample.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, AppUser>().ReverseMap();
            CreateMap<GetAllRoleDtos, AppRole>().ReverseMap();
        }
    }
}
