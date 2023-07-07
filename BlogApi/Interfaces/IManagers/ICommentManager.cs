using BlogApi.Entities;
using BlogApi.HelperEntities.Pagination;
using BlogApi.Models.CommentModels;

namespace BlogApi.Interfaces.IManagers;

public interface ICommentManager
{
    Task<CommentModel> AddNewCommentAsync(CreateCommentModel model);
    Task<IEnumerable<CommentModel>> GetAllCommentsAsync(CommentGetFilter commentGetFilter);
    Task<CommentModel> GetCommentByIdAsync(Guid commentId);
    Task<CommentModel> GetCommentByIdWithLikesAsync(Guid commentId);
    Task<CommentModel> UpdateCommentAsync(Guid commentId, UpdateCommentModel model);
    Task DeleteCommentAsync(Guid commentId);

    CommentModel MapToCommentModel(Comment comment);
}