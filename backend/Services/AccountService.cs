using AutoMapper;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public class AccountService(IMapper mapper, UserManager<IdentityUser> userManager) : IAccountService
{
    
    public IdentityUser? Create(UserRegistrationModel model, out IdentityResult result)
    {
        var user = mapper.Map<IdentityUser>(model);
        result = userManager.CreateAsync(user, model.Password).GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            return user;
        }

        return null;
    }
}