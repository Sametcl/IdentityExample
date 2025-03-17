using AutoMapper;
using IdentitiyExample.DTOs;
using IdentitiyExample.Models;

namespace IdentitiyExample.Mapping
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<RegisterDto, AppUser>().ReverseMap();
        }
    }
}
