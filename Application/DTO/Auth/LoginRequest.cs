namespace Application.DTO.Auth;

/// <summary>
/// Represents the request data required for user login.
/// </summary>
/// <remarks>
/// The <see cref="LoginRequest"/> class is used to encapsulate the user's credentials, including email and password,
/// required for authentication during login.
/// </remarks>
public class LoginRequest
{
    /// <summary>
    /// Gets or sets the user's email address.
    /// This is used as the identifier for authentication purposes.
    /// </summary>
    /// <example>user@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user's password.
    /// This is the secret credential used in combination with the email to authenticate the user.
    /// </summary>
    /// <example>P@ssw0rd!</example>
    public string Password { get; set; } = string.Empty;
}
