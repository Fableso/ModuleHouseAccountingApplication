namespace Application.DTO.Post.Request;

/// <summary>
/// Represents the request data to create a new post in the system.
/// </summary>
/// <remarks>
/// The <see cref="CreatePostRequest"/> is used to define the essential details required to create a new post.
/// Posts can represent different components or areas of a project, and they typically include details like a name and area.
/// </remarks>
public sealed class CreatePostRequest : IPostRequest
{
    /// <summary>
    /// Gets or sets the name of the post.
    /// </summary>
    /// <remarks>
    /// The name is used to identify the post and should be descriptive of the purpose or location of the post.
    /// Examples could include "Main Section", "Garage Area", or "Backyard".
    /// </remarks>
    /// <example>"Main"</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the area of the post in square meters.
    /// </summary>
    /// <remarks>
    /// The area represents the size of the post, which can be null if the area is not applicable or not yet determined.
    /// </remarks>
    /// <example>45.0</example>
    public double? Area { get; set; }
}
