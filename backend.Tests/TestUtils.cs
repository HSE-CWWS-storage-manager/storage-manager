using AutoMapper;
using backend.Mapping;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace backend.Tests;

public static class TestUtils
{
    
    public static Mock<UserManager<IdentityUser>> MockUserManager()
    {
        var dict = new Dictionary<string, Tuple<IdentityUser, string>>();
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        var userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        userManagerMock.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());
        
        userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync((IdentityUser user, string _) => dict.ContainsKey(user.Email!) ? IdentityResult.Failed() : IdentityResult.Success)
            .Callback<IdentityUser, string>((x, y) =>
            {
                if (!dict.ContainsKey(x.Email!))
                    dict.Add(x.UserName!, new(x, y));
            });

        userManagerMock.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => dict[name].Item1);
        
        return userManagerMock;
    }

    public static IMapper CreateAutoMapper()
    {
        var configuration = new MapperConfiguration(configuration => configuration.AddProfile<MappingProfile>());

        return configuration.CreateMapper();
    }
}