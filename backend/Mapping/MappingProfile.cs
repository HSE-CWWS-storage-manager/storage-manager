using AutoMapper;
using backend.Models;
using common.Dtos;
using common.Dtos.Request;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AddEquipmentRequest, Equipment>();
        CreateMap<Equipment, EquipmentDto>();
        CreateMap<Warehouse, WarehouseDto>();
        CreateMap<UserRegistrationRequest, IdentityUser>()
            .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));
    }
}