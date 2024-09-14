using Application.DTO.House.Response;
using Application.DTO.Post.Request;
using Application.DTO.Post.Response;
using Domain.StronglyTypedIds;

namespace Application.Abstractions;

public interface IPostService
{
    Task<IEnumerable<PostResponse>> GetAllAsync(CancellationToken token = default);
    Task<PostResponse?> GetByIdAsync(PostId id, CancellationToken token = default);
    Task<PostResponse> AddAsync(CreatePostRequest postRequest, CancellationToken token = default);
    Task RemoveByIdAsync(PostId id, CancellationToken token = default);
    Task<PostResponse> UpdateAsync(UpdatePostRequest post, CancellationToken token = default);
}