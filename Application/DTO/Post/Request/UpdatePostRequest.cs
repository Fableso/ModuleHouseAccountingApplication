using Domain.StronglyTypedIds;

namespace Application.DTO.Post.Request;

public sealed class UpdatePostRequest : IPostRequest
{
    public PostId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? Area { get; set; }
}