namespace Application.DTO.Post.Request;

/// <summary>
/// Defines the structure required for requests related to creating or updating posts in the system.
/// </summary>
/// <remarks>
/// The <see cref="IPostRequest"/> interface provides the common properties that need to be implemented by any 
/// request involving posts, such as creating a new post or updating an existing one. 
/// The properties include essential information like the name and area of the post.
/// </remarks>
public interface IPostRequest
{
    /// <summary>
    /// Gets or sets the name of the post.
    /// </summary>
    /// <remarks>
    /// The name should be descriptive and should clearly identify the purpose or function of the post.
    /// Examples include "Main", "Backyard", or "Garage Area".
    /// </remarks>
    /// <example>Main</example>
    string Name { get; set; }

    /// <summary>
    /// Gets or sets the area of the post in square meters.
    /// </summary>
    /// <remarks>
    /// The area represents the size of the post. This can be left as null if the area is not applicable or unknown.
    /// </remarks>
    /// <example>50.0</example>
    double? Area { get; set; }
}