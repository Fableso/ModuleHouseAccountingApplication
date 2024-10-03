using Domain.StronglyTypedIds;

namespace Application.DTO.Post.Response;

/// <summary>
/// Represents the response data for a post in the system.
/// </summary>
/// <remarks>
/// The <see cref="PostResponse"/> class contains information about an individual post, including its identifier, name, and area.
/// This DTO is used to provide details about posts when they are queried from the system.
/// </remarks>
public sealed class PostResponse
{
    /// <summary>
    /// Gets or sets the unique identifier of the post.
    /// </summary>
    /// <example>2</example>
    public PostId Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the post.
    /// </summary>
    /// <remarks>
    /// The name provides a descriptive label for the post, indicating its purpose or position within the project.
    /// Examples could include "Main Section" or "Backyard".
    /// </remarks>
    /// <example>"Main"</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the area of the post in square meters.
    /// </summary>
    /// <remarks>
    /// The area represents the size of the post. This value can be null if the area is not applicable or has not been specified.
    /// </remarks>
    /// <example>75.0</example>
    public double? Area { get; set; }
}
