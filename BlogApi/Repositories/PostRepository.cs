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

    public IQueryable<Post> GetAllPosts()
    {
        return _dbContext.Posts.AsQueryable();
    }

    public IQueryable<Post> GetPostById(Guid postId)
    {
        return _dbContext.Posts.Where(post => post.Id == postId);
    }

    public IQueryable<Post> GetPostByIdWithLikesAndComments(Guid postId)
    {
        return _dbContext.Posts
            .Include(post => post.Likes)
            .Include(post => post.Comments)
            .Where(post => post.Id == postId);
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