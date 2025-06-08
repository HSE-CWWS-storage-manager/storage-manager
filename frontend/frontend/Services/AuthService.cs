// Services/AuthService.cs

using common.Dtos.Request;
using common.Dtos.Response;
using frontend.Components.Helpers;
using frontend.Providers;
using Microsoft.AspNetCore.Components.Authorization;

namespace frontend.Services;

public class AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
{
    public async Task<Result<TokenResponse>> LoginAsync(UserAuthenticationRequest request)
    {
        var response = await http.PostAsJsonAsync("https://storage-manager-backend.fiwka.xyz/Account/Login", request);

        if (!response.IsSuccessStatusCode) return Result<TokenResponse>.Failure("Failed to login");
        
        response.EnsureSuccessStatusCode();
        var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
            
        return token is null ? Result<TokenResponse>.Failure("Invalid JSON") : Result<TokenResponse>.Success(token);

    }

    public async Task LogoutAsync()
    {
        // await _http.PostAsync("api/auth/logout", null); // если есть эндпоинт logout
        var customAuthStateProvider = (AuthProvider)authStateProvider;
        customAuthStateProvider.MarkUserAsLoggedOut();
    }

    public async Task<AuthenticationState> RefreshLoginStateAsync(TokenResponse token)
    {
        var customAuthStateProvider = (AuthProvider)authStateProvider;
        return await customAuthStateProvider.LoginAsync(token);
    }
}