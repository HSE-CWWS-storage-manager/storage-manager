using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace backend.Auth;

public class AuthOptions
{
    public const string Issuer = "StorageManagerServer";
    public const string Audience = "StorageManagerClient";
    public const int Lifetime = 1440;

    private static readonly string? Key = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ??
                                          "mysupersecret_secretkey!123secretTTT";

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}