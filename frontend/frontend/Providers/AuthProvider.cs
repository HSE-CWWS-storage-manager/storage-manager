using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using common.Dtos.Response;
using Microsoft.AspNetCore.Components.Authorization;

namespace frontend.Providers;

public class AuthProvider(HttpClient http) : AuthenticationStateProvider
{
    private bool _isAuthenticated;
    private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_isAuthenticated)
        {
            return new AuthenticationState(_currentUser);
        }

        return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public async Task<AuthenticationState> LoginAsync(TokenResponse tokenResponse)
    {
        // Парсим токен, например, JWT
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(tokenResponse.AccessToken);

        var identity = new ClaimsIdentity(jwtToken.Claims, "custom");
        _currentUser = new ClaimsPrincipal(identity);
        _isAuthenticated = true;

        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        return new AuthenticationState(_currentUser);
    }

    public void MarkUserAsLoggedOut()
    {
        _isAuthenticated = false;
        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}