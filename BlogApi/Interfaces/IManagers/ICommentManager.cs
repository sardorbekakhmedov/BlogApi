using BlogApi.Entities;
using BlogApi.Models.CommentModels;

namespace BlogApi.Interfaces.IManagers;

public interface ICommentManager
{
    Task<CommentModel> AddNewCommentAsync(CreateCommentModel model);
    Task<List<CommentModel>> GetAllCommentsAsync();
    Task<CommentModel> GetCommentByIdAsync(Guid commentId);
    Task<CommentModel> GetCommentByIdWithLikesAsync(Guid commentId);
    Task<CommentModel> UpdateCommentAsync(Guid commentId, UpdateCommentModel model);
    Task DeleteCommentAsync(Guid commentId);

    CommentModel MapToCommentModel(Comment comment);
}