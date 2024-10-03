using Domain.StronglyTypedIds;

namespace Application.DTO.Post.Request;

/// <summary>
/// Represents the request data to update an existing post in the system.
/// </summary>
/// <remarks>
/// The <see cref="UpdatePostRequest"/> is used to modify the details of an existing post within the system.
/// It includes properties like the identifier of the post to be updated, the updated name, and optionally, the updated area.
/// </remarks>
public sealed class UpdatePostRequest : IPostRequest
{
    /// <summary>
    /// Gets or sets the identifier of the post that needs to be updated.
    /// </summary>
    /// <remarks>
    /// The identifier is used to uniquely locate the post that is to be modified.
    /// </remarks>
    /// <example>2</example>
    public PostId Id { get; set; }

    /// <summary>
    /// Gets or sets the updated name of the post.
    /// </summary>
    /// <remarks>
    /// The name should provide a descriptive label for the post, reflecting the updated purpose or function of the post.
    /// Examples might include "Renovated Main Section" or "Updated Backyard".
    /// </remarks>
    /// <example>"Updated Backyard"</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the updated area of the post in square meters.
    /// </summary>
    /// <remarks>
    /// The area represents the size of the post after modification. This can be null if no changes are made to the area.
    /// </remarks>
    /// <example>55.0</example>
    public double? Area { get; set; }
}
