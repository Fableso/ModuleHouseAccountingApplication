using Domain.StronglyTypedIds;

namespace Domain.Entities;

public class HousePost
{
    public HousePostId Id { get; set; }

    public HouseId HouseId { get; set; }
    public PostId PostId { get; set; }

    public House House { get; private set; } = null!;
    public Post Post { get; private set; } = null!;
}