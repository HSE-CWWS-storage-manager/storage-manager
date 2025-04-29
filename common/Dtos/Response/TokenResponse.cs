namespace common.Dtos.Response;

public class TokenResponse(string accessToken)
{
    public string AccessToken { get; set; } = accessToken;
}