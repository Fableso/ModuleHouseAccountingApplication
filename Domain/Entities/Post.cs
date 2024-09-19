using Domain.Common;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;

namespace Domain.Entities;

public class Post : BaseEntity<PostId>
{
    private Post()
    {
        _houses = new List<HousePost>();
    }
    public Post(PostName name) : this()
    {
        Name = name.Value;
    }

    public string Name { get; private set; } = string.Empty;

    private double _area;
    public double Area
    {
        get => _area;
        set => _area = value < 0 ? 0 : value;
    }
    
    public IReadOnlyList<HousePost> Houses => _houses;
    private List<HousePost> _houses;

    public Post UpdatePost(Post newPost)
    {
        Name = newPost.Name;
        _area = newPost._area;
        return this;
    }
}