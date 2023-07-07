using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface ICommentRepository
{
    Task CreateCommentAsync(Comment comment);
    IQueryable<Comment> GetAllComments();
    IQueryable<Comment> GetCommentById(Guid commentId);
    IQueryable<Comment> GetCommentByIdWithLikes(Guid commentId);
    Task UpdateCommentAsync(Comment comment);
    Task DeleteCommentAsync(Comment comment);
}