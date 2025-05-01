using backend.Mapping.Impl;
using backend.Services.Impl;
using common.Dtos.Request;

using static backend.Tests.TestUtils;

namespace backend.Tests;

public class AccountServiceTest
{

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