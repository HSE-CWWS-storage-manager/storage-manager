using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using backend.Auth;
using backend.Dtos.Request;
using backend.Dtos.Response;
using backend.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services.Impl;

public class AccountService(IMapper mapper, UserManager<IdentityUser> userManager) : IAccountService
{
    public async Task<Tuple<IdentityUser?, IdentityResult>> Create(UserRegistrationRequest request)
    {
        var user = mapper.Map<IdentityUser>(request);
        var result = await userManager.CreateAsync(user, request.Password);

        return new(user, result);
    }

    public async Task<TokenResponse> Login(UserAuthenticationRequest request)
    {
        var user = await GetUser(request.Email, request.Password);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwt = new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenResponse(encodedJwt);
    }

    private async Task<IdentityUser> GetUser(string email, string password)
    {
        var user = await userManager.FindByNameAsync(email);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            throw new HttpResponseException((int)HttpStatusCode.Forbidden, new
            {
                Error = "Email or password incorrect!",
                Code = HttpStatusCode.Forbidden
            });

        return user;
    }
}