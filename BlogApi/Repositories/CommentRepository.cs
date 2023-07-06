using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly BlogApiDbContext _dbContext;

    public CommentRepository(BlogApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCommentAsync(Comment comment)
    {
        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Comment>> GetAllCommentsAsync()
    {
        return await _dbContext.Comments.ToListAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(Guid commentId)
    {
        return await _dbContext.Comments.FirstOrDefaultAsync(post => post.Id == commentId);
    }

    public async Task<Comment?> GetCommentByIdWithLikesAsync(Guid commentId)
    {
        return await _dbContext.Comments.Include(post => post.CommentLikes)
            .FirstOrDefaultAsync(post => post.Id == commentId);
    }

    public async Task UpdateCommentAsync(Comment comment)
    {
        _dbContext.Comments.Update(comment);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Comment comment)
    {
        _dbContext.Comments.Remove(comment);
        await _dbContext.SaveChangesAsync();
    }
}