namespace backend.Models;

public class TokenResponseModel(string accessToken)
{
    public string AccessToken { get; set; } = accessToken;
}