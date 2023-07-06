using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class PostRepository : IPostRepository
{
    private readonly BlogApiDbContext _dbContext;

    public PostRepository(BlogApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreatePostAsync(Post post)
    {
        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Post>> GetAllPostsAsync()
    {
        return await _dbContext.Posts.ToListAsync();
    }

    public async Task<Post?> GetPostByIdAsync(Guid postId)
    {
        return await _dbContext.Posts.FirstOrDefaultAsync(post => post.Id == postId);
    }

    public async Task<Post?> GetPostByIdWithLikesAndCommentsAsync(Guid postId)
    {
        return await _dbContext.Posts
            .Include(post => post.Likes)
            .Include(post => post.Comments)
            .FirstOrDefaultAsync(post => post.Id == postId);
    }

    public async Task UpdatePostAsync(Post post)
    {
        _dbContext.Posts.Update(post);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Post post)
    {
        _dbContext.Posts.Remove(post);
        await _dbContext.SaveChangesAsync();
    }
}