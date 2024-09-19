namespace Application.DTO.Post.Request;

public sealed class CreatePostRequest : IPostRequest
{
    public string Name { get; set; } = string.Empty;
    public double? Area { get; set; }
}