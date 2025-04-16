using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace backend.Auth;

public class AuthOptions
{

    private static readonly string? Key = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "mysupersecret_secretkey!123secretTTT";
    
    public const string Issuer = "StorageManagerServer";
    public const string Audience = "StorageManagerClient";
    public const int Lifetime = 5;
    
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}