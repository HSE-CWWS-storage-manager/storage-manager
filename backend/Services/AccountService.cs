using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using backend.Auth;
using backend.Exceptions;
using backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

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

    public TokenResponseModel Login(UserAuthenticationModel model)
    {
        var user = GetUser(model.Email, model.Password).GetAwaiter().GetResult();
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.Issuer,
            audience: AuthOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new TokenResponseModel(encodedJwt);
    }

    private async Task<IdentityUser> GetUser(string email, string password)
    {
        var user = await userManager.FindByNameAsync(email);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            throw new HttpResponseException((int) HttpStatusCode.Forbidden, "Email or password incorrect!");
        
        return user;
    }
}