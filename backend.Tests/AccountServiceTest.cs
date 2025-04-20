using Microsoft.AspNetCore.Identity;
using Moq;

namespace backend.Tests;

public class AccountServiceTest
{

    public static Mock<UserManager<IdentityUser>> MockUserManager()
    {
        var dict = new Dictionary<string, Tuple<IdentityUser, string>>();
        var userStoreMock = new Mock<IUserStore<IdentityUser>>();
        var userManagerMock = new Mock<UserManager<IdentityUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        
        userManagerMock.Object.UserValidators.Add(new UserValidator<IdentityUser>());
        userManagerMock.Object.PasswordValidators.Add(new PasswordValidator<IdentityUser>());
        
        userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<IdentityUser>()))
            .ReturnsAsync(IdentityResult.Success)
            .Callback<IdentityUser, string>((x, y) => dict.Add(x.UserName!, new(x, y)));

        userManagerMock.Setup(userManager => userManager.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync((string name) => dict[name].Item1);
        
        return userManagerMock;
    }

    [Fact]
    public async Task Register_ValidUserData_ReturnsValidUser()
    {
    }
}