using common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping.Impl;

public class UserMapper : IUserMapper
{
    public UserDto ToUserDto(IdentityUser user, IList<string> roles)
    {
        return new(
            Guid.Parse(user.Id),
            user.Email!,
            roles
        );
    }
}