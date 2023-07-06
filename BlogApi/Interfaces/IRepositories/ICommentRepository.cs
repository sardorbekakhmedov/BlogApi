using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface ICommentRepository
{
    Task CreateCommentAsync(Comment comment);
    Task<List<Comment>> GetAllCommentsAsync();
    Task<Comment?> GetCommentByIdAsync(Guid commentId);
    Task<Comment?> GetCommentByIdWithLikesAsync(Guid commentId);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Comment comment);
}