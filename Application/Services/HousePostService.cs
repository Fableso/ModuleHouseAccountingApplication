using Application.Abstractions;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class HousePostService : IHousePostService
{
    private readonly IApplicationDbContext _context;
    public HousePostService(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task AddHousePostRelationsAsync(HouseId houseId, IEnumerable<PostId> postIdsToAdd, CancellationToken token = default)
    {
        var idsToAdd = postIdsToAdd.ToList();
        if (idsToAdd.Count == 0) return;

        var newHousePosts = idsToAdd.Select(postId => new HousePost(houseId, postId));
        await _context.HousePosts.AddRangeAsync(newHousePosts, token);
    }
    
    public async Task UpdatePostsForHouseAsync(HouseId houseId, IEnumerable<PostId> newPostIds, CancellationToken token = default)
    {
        await ValidateHouseExistsAsync(houseId, token);
        var newPostIdsList = newPostIds.ToList();
        await ValidatePostsExistAsync(newPostIdsList, token);

        var currentPostIds = await GetCurrentPostIdsAsync(houseId, token);

        var postIdsToAdd = ComputePostIdsToAdd(currentPostIds, newPostIdsList);
        var postIdsToRemove = ComputePostIdsToRemove(currentPostIds, newPostIdsList);

        await AddHousePostRelationsAsync(houseId, postIdsToAdd, token);
        await RemoveHousePostRelationsAsync(houseId, postIdsToRemove, token);
    }
    
    private async Task ValidateHouseExistsAsync(HouseId houseId, CancellationToken token)
    {
        var houseExists = await _context.Houses.AnyAsync(h => h.Id == houseId, token);
        if (!houseExists)
        {
            throw new ArgumentException($"House with ID {houseId.Value} does not exist.");
        }
    }

    private async Task ValidatePostsExistAsync(IEnumerable<PostId> postIds, CancellationToken token)
    {
        var postIdsList = postIds.ToList();
        var existingPostIds = await _context.Posts
            .Where(p => postIdsList.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync(token);

        var invalidPostIds = postIdsList.Except(existingPostIds).ToList();
        if (invalidPostIds.Count != 0)
        {
            throw new ArgumentException($"The following Post IDs do not exist: {string.Join(", ", invalidPostIds.Select(x => x.Value))}");
        }
    }

    private async Task<List<PostId>> GetCurrentPostIdsAsync(HouseId houseId, CancellationToken token)
    {
        return await _context.HousePosts
            .Where(hp => hp.HouseId == houseId)
            .Select(hp => hp.PostId)
            .ToListAsync(token);
    }

    private static List<PostId> ComputePostIdsToAdd(IEnumerable<PostId> currentPostIds, IEnumerable<PostId> newPostIds)
    {
        return newPostIds.Except(currentPostIds).ToList();
    }

    private static List<PostId> ComputePostIdsToRemove(IEnumerable<PostId> currentPostIds, IEnumerable<PostId> newPostIds)
    {
        return currentPostIds.Except(newPostIds).ToList();
    }
    
    private async Task RemoveHousePostRelationsAsync(HouseId houseId, IEnumerable<PostId> postIdsToRemove, CancellationToken token)
    {
        var idsToRemove = postIdsToRemove.ToList();
        if (idsToRemove.Count == 0) return;

        var housePostsToRemove = await _context.HousePosts
            .Where(hp => hp.HouseId == houseId && idsToRemove.Contains(hp.PostId))
            .ToListAsync(token);

        _context.HousePosts.RemoveRange(housePostsToRemove);
    }
}