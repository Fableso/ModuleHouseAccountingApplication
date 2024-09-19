using Domain.StronglyTypedIds;

namespace Application.Abstractions;

public interface IHousePostService
{
    Task AddHousePostRelationsForNewHouseAsync(HouseId houseId, IEnumerable<PostId> postIdsToAdd, CancellationToken token = default);
    Task UpdatePostsForHouseAsync(HouseId houseId, IEnumerable<PostId> newPostIds, CancellationToken token = default);
}