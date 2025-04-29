using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IAccountService
{
    Task<Tuple<UserDto?, IdentityResult>> Create(UserRegistrationRequest request);
    Task<TokenResponse> Login(UserAuthenticationRequest request);
}