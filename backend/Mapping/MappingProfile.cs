using AutoMapper;
using common.Dtos.Request;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegistrationRequest, IdentityUser>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
    }
}