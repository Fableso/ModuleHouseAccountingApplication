using Domain.StronglyTypedIds;

namespace Application.DTO.Post.Response;

public sealed class PostResponse 
{ 
    public PostId Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public double? Area { get; set; }
}