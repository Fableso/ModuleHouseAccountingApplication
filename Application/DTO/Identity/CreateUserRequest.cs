namespace Application.DTO.Identity;

/// <summary>
/// Represents the request data to create a new user in the system.
/// </summary>
/// <remarks>
/// The <see cref="CreateUserRequest"/> class is used for user registration or for administrators to create new user accounts.
/// It includes essential information such as the user's email, password, and role within the system.
/// </remarks>
public class CreateUserRequest
{
    /// <summary>
    /// Gets or sets the email address of the user to be created.
    /// </summary>
    /// <remarks>
    /// The email address must be unique within the system and is used for user identification and login.
    /// </remarks>
    /// <example>user@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the password for the user.
    /// </summary>
    /// <remarks>
    /// The password should be strong and contain a mix of letters, numbers, and special characters for better security.
    /// </remarks>
    /// <example>P@ssw0rd123</example>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the role assigned to the user.
    /// </summary>
    /// <remarks>
    /// The role defines the level of access or permissions the user will have within the system.
    /// Typical roles could include "Admin", "User", or "Moderator".
    /// </remarks>
    /// <example>Spectator</example>
    public string Role { get; set; } = string.Empty;
}
