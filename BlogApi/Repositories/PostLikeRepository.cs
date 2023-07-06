using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class PostLikeRepository : IPostLikeRepository
{
    private readonly BlogApiDbContext _dbContext;

    public PostLikeRepository(BlogApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePostLikeAsync(PostLike postLike)
    {
        await _dbContext.PostLikes.AddAsync(postLike);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<PostLike>> GetAllPostLikesAsync()
    {
        return await _dbContext.PostLikes.ToListAsync();
    }

    public async Task<PostLike?> GetPostLikeByIdAsync(Guid postLikeId)
    {
        return await _dbContext.PostLikes.FirstOrDefaultAsync(post => post.Id == postLikeId);
    }

    public async Task DeletePostLikeAsync(PostLike postLike)
    {
        _dbContext.PostLikes.Remove(postLike);
        await _dbContext.SaveChangesAsync();
    }
}