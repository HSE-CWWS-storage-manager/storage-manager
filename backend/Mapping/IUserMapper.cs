using common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping;

public interface IUserMapper
{

    UserDto ToUserDto(IdentityUser user, IList<string> roles);
}