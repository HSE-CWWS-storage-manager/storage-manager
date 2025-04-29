using AutoMapper;
using backend.Mapping;
using backend.Mapping.Impl;
using backend.Services.Impl;
using common.Dtos.Request;
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

    [Fact]
    public async Task Register_ValidUserData_ReturnsValidUser()
    {
        var accountService = new AccountService(CreateAutoMapper(), new UserMapper(), MockUserManager().Object);

        var request = new UserRegistrationRequest
        {
            Email = "test@example.com",
            Password = "12345678Aa$",
            ConfirmPassword = "12345678Aa$"
        };
        
        var actual = await accountService.Create(request);
        
        Assert.True(actual.Item2.Succeeded);
        Assert.NotNull(actual.Item1);
        Assert.Equal("test@example.com", actual.Item1.Email);
    }
    
    [Fact]
    public async Task Register_Conflict_ReturnsUnsuccessfulResult()
    {
        var accountService = new AccountService(CreateAutoMapper(), new UserMapper(), MockUserManager().Object);

        var request = new UserRegistrationRequest
        {
            Email = "test@example.com",
            Password = "12345678Aa$",
            ConfirmPassword = "12345678Aa$"
        };
        
        var request2 = new UserRegistrationRequest
        {
            Email = "test@example.com",
            Password = "12345678Aa$",
            ConfirmPassword = "12345678Aa$"
        };

        await accountService.Create(request);
        var actual = await accountService.Create(request2);
        
        Assert.False(actual.Item2.Succeeded);
    }
}