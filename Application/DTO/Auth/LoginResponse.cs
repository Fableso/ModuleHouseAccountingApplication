namespace Application.DTO.Auth;

public class LoginResponse(string token, DateTime expiration)
{
    public string Token { get; init; } = token;
    public DateTime Expiration { get; init; } = expiration;
};