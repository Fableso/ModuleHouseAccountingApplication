namespace Application.DTO.Auth;

/// <summary>
/// Represents the response data returned upon successful user login.
/// </summary>
/// <remarks>
/// The <see cref="LoginResponse"/> class is typically used to encapsulate
/// authentication information, such as tokens that are required for secure API access.
/// </remarks>
public class LoginResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LoginResponse"/> class.
    /// </summary>
    /// <param name="token">The JWT (JSON Web Token) used for authenticating subsequent API requests.</param>
    /// <param name="expiration">The date and time when the token expires.</param>
    /// <example>
    /// <code>
    /// var loginResponse = new LoginResponse("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9", DateTime.UtcNow.AddHours(1));
    /// </code>
    /// </example>
    public LoginResponse(string token, DateTime expiration)
    {
        Token = token;
        Expiration = expiration;
    }

    /// <summary>
    /// Gets the JWT (JSON Web Token) issued after a successful login.
    /// This token is used for authenticating subsequent API requests.
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
    public string Token { get; init; }

    /// <summary>
    /// Gets the expiration date and time of the token.
    /// The client should ensure to renew the token before this time to maintain access.
    /// </summary>
    /// <example>2024-10-05T12:45:00Z</example>
    public DateTime Expiration { get; init; }
}
