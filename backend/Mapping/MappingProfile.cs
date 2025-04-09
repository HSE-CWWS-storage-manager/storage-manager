using AutoMapper;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserRegistrationModel, IdentityUser>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
    }
}