using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IAccountService
{

    IdentityUser? Create(UserRegistrationModel model, out IdentityResult result);
    TokenResponseModel Login(UserAuthenticationModel model);
}