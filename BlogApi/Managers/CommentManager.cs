using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.CommentModels;
using BlogApi.CustomExceptions.CommentException;
using BlogApi.Interfaces.IManagers;

namespace BlogApi.Managers;

public class CommentManager : ICommentManager
{
    private readonly UserProvider _userProvider;
    private readonly ICommentRepository _commentRepository;

    public CommentManager(ICommentRepository commentRepository, UserProvider userProvider)
    {
        _commentRepository = commentRepository;
        _userProvider = userProvider;
    }

    public async Task<CommentModel> AddNewCommentAsync(CreateCommentModel model)
    {
        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();

        var comment = new Comment()
        {
            TextComment = model.TextComment,
            UserId = (Guid)userId,
            PostId = model.PostId
        };

        await _commentRepository.CreateCommentAsync(comment);

        return MapToCommentModel(comment);
    }

    public async Task<List<CommentModel>> GetAllCommentsAsync()
    {
        var posts = await _commentRepository.GetAllCommentsAsync();

        return posts.Select(MapToCommentModel).ToList();
    }


    public async Task<CommentModel> GetCommentByIdAsync(Guid commentId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);

        if (comment is null)
            throw new CommentNotFoundException();
        return MapToCommentModel(comment);
    }

    public async Task<CommentModel> GetCommentByIdWithLikesAsync(Guid commentId)
    {
        var comment = await _commentRepository.GetCommentByIdWithLikesAsync(commentId);

        if (comment is null)
            throw new CommentNotFoundException();

        return MapToCommentModel(comment);
    }

    public async Task<CommentModel> UpdateCommentAsync(Guid commentId, UpdateCommentModel model)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);

        if (comment == null)
            throw new CommentNotFoundException();

        comment.TextComment = model.TextComment ?? comment.TextComment;
        comment.PostId = model.PostId ?? comment.PostId;
        comment.UpdatedDate = DateTime.UtcNow;

        await _commentRepository.UpdateCommentAsync(comment);

        return MapToCommentModel(comment);
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);

        if (comment is not null)
        {
            await _commentRepository.DeleteCommentAsync(comment);
        }
    }


    public CommentModel MapToCommentModel(Comment comment)
    {
        var commentModel = new CommentModel
        {
            Id = comment.Id,
            TextComment = comment.TextComment,
            UserId = comment.UserId,
            PostId = comment.PostId,
            CreatedDate = comment.CreatedDate,
            UpdatedDate = comment.UpdatedDate
        };

        if (comment.CommentLikes is not null)
            commentModel.CommentLikesCount = comment.CommentLikes.Count;

        return commentModel;
    }
}