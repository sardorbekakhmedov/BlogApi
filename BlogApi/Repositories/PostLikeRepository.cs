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

    public IQueryable<PostLike> GetAllPostLikes()
    {
        return _dbContext.PostLikes.AsQueryable();
    }

    public IQueryable<PostLike> GetPostLikeById(Guid postLikeId)
    {
        return _dbContext.PostLikes.Where(post => post.Id == postLikeId);
    }

    public IQueryable<PostLike> GetPostLikeByUserId(Guid userId)
    {
        return _dbContext.PostLikes.Where(post => post.UserId == userId);
    }

    public async Task DeletePostLikeAsync(PostLike postLike)
    {
        _dbContext.PostLikes.Remove(postLike);
        await _dbContext.SaveChangesAsync();
    }
}