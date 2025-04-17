using backend.Dtos.Request;
using backend.Dtos.Response;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IAccountService
{
    Task<Tuple<IdentityUser?, IdentityResult>> Create(UserRegistrationRequest request);
    Task<TokenResponse> Login(UserAuthenticationRequest request);
}