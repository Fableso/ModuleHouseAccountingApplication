using Application.Abstractions;
using Application.DTO.House.Response;
using Application.DTO.Post.Request;
using Application.DTO.Post.Response;
using Application.Exceptions;
using AutoMapper;
using Domain.Entities;
using Domain.StronglyTypedIds;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class PostService : IPostService
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PostService(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<PostResponse>> GetAllAsync(CancellationToken token = default)
    {
        var posts = await _context.Posts.AsNoTracking().ToListAsync(cancellationToken: token);
        return _mapper.Map<IEnumerable<PostResponse>>(posts);
    }

    public async Task<PostResponse?> GetByIdAsync(PostId id, CancellationToken token = default)
    {
        var post = await _context.Posts.FindAsync(id, token);
        return _mapper.Map<PostResponse?>(post);
    }

    public async Task<PostResponse> AddAsync(CreatePostRequest postRequest, CancellationToken token = default)
    {
        if (await PostNameExists(postRequest.Name, token))
        {
            throw new EntityAlreadyExistsException("Post with the same name already exists. Post names must be unique");
        }
        var post = _mapper.Map<Post>(postRequest);
        await _context.Posts.AddAsync(post, token);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<PostResponse>(post);
    }

    public async Task RemoveByIdAsync(PostId id, CancellationToken token = default)
    {
        var postToDelete = await _context.Posts.FindAsync(id, token);
        if (postToDelete is null)
        {
            throw new EntityNotFoundException($"Post with ID {id.Value} not found");
        }
        _context.Posts.Remove(postToDelete);
        await _context.SaveChangesAsync(token);
    }

    public async Task<PostResponse> UpdateAsync(UpdatePostRequest post, CancellationToken token = default)
    {
        var existingPost = await _context.Posts.FindAsync(post.Id, token);
        if (existingPost is null)
        {
            throw new EntityNotFoundException($"Post with ID {post.Id.Value} not found");
        }

        if (await PostNameExists(post.Name, token) && existingPost.Name != post.Name)
        {
            throw new EntityAlreadyExistsException("Post with the same name already exists. Post names must be unique");
        }
        
        var updatedPost = _mapper.Map(post, existingPost);
        _context.Posts.Update(updatedPost);
        await _context.SaveChangesAsync(token);
        return _mapper.Map<PostResponse>(updatedPost);
    }
    
    private async Task<bool> PostNameExists(string name, CancellationToken token = default)
    {
        return await _context.Posts.AnyAsync(e => e.Name == name, token);
    }
}