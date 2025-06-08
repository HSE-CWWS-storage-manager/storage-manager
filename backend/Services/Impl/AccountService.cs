using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using backend.Auth;
using backend.Exceptions;
using backend.Mapping;
using common.Dtos;
using common.Dtos.Request;
using common.Dtos.Response;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using static backend.Utils.StringConstants;

namespace backend.Services.Impl;

public class AccountService(IMapper mapper, IUserMapper userMapper, UserManager<IdentityUser> userManager) : IAccountService
{
    
    private const int PageSize = 10;
    
    public async Task<Tuple<UserDto?, IdentityResult>> Create(UserRegistrationRequest request)
    {
        var user = mapper.Map<IdentityUser>(request);
        var result = await userManager.CreateAsync(user, request.Password);
        
        if (result.Succeeded) 
            await userManager.AddToRoleAsync(user, UserRole);

        return new(userMapper.ToUserDto(user, [UserRole]), result);
    }

    public async Task<TokenResponse> Login(UserAuthenticationRequest request)
    {
        var user = await GetUser(request.Email, request.Password);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        var roles = await userManager.GetRolesAsync(user);
        
        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

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

    public async Task<IdentityUser> GetUserFromPrincipal(ClaimsPrincipal principal)
    {
        return await userManager.FindByIdAsync(userManager.GetUserId(principal) ?? throw new HttpResponseException(
            (int) HttpStatusCode.NotFound,
            "Principal don't have user id"
        )) ?? throw new HttpResponseException(
            (int) HttpStatusCode.NotFound,
            "User not found"
        );
    }

    public async Task<UserDto> GetUserDtoFromPrincipal(ClaimsPrincipal principal)
    {
        var user = await GetUserFromPrincipal(principal);
        var roles = await userManager.GetRolesAsync(user);

        return userMapper.ToUserDto(user, roles);
    }

    public async Task<UserListResponse> GetUserList(UserListRequest request)
    {
        var users = userManager.Users
            .Skip((request.Page - 1) * PageSize)
            .Take(PageSize)
            .ToList();
        var dtos = new List<UserDto>();

        foreach (var user in users)
        {
            dtos.Add(userMapper.ToUserDto(user, await userManager.GetRolesAsync(user)));
        }
        
        return new UserListResponse(dtos);
    }

    public async Task<UserDto> ModifyRole(UserRoleModifyRequest request)
    {
        var user = await userManager.FindByIdAsync(request.UserId.ToString());

        if (user == null)
            throw new HttpResponseException(
                (int)HttpStatusCode.NotFound,
                new HttpErrorMessageResponse($"User with id {request.UserId} not found!")
            );

        if (request.Operation == UserRoleModifyOperation.Add)
            await userManager.AddToRoleAsync(user, request.Role);
        else
            await userManager.RemoveFromRoleAsync(user, request.Role);

        return userMapper.ToUserDto(user, await userManager.GetRolesAsync(user));
    }

    private async Task<IdentityUser> GetUser(string email, string password)
    {
        var user = await userManager.FindByNameAsync(email);

        if (user == null || !await userManager.CheckPasswordAsync(user, password))
            throw new HttpResponseException((int)HttpStatusCode.Forbidden, new HttpErrorMessageResponse("Email or password incorrect!"));

        return user;
    }
}