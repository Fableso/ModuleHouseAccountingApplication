namespace Application.DTO.Post.Request;

public interface IPostRequest
{
    public string Name { get; set; }
    public double? Area { get; set; }
}