using backend.Dtos.Request;
using backend.Dtos.Response;
using common.Dtos;
using Microsoft.AspNetCore.Identity;

namespace backend.Services;

public interface IAccountService
{
    Task<Tuple<UserDto?, IdentityResult>> Create(UserRegistrationRequest request);
    Task<TokenResponse> Login(UserAuthenticationRequest request);
}