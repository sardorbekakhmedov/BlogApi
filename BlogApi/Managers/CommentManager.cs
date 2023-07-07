using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.CommentModels;
using BlogApi.CustomExceptions.CommentException;
using BlogApi.Extensions;
using BlogApi.HelperEntities.Pagination;
using BlogApi.Interfaces.IManagers;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers;

public class CommentManager : ICommentManager
{
    private readonly UserProvider _userProvider;
    private readonly HttpContextHelper _httpContextHelper;
    private readonly ICommentRepository _commentRepository;

    public CommentManager(ICommentRepository commentRepository, UserProvider userProvider, HttpContextHelper httpContextHelper)
    {
        _commentRepository = commentRepository;
        _userProvider = userProvider;
        _httpContextHelper = httpContextHelper;
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

    public async Task<IEnumerable<CommentModel>> GetAllCommentsAsync(CommentGetFilter commentGetFilter)
    {
        var commentsIQueryable = _commentRepository.GetAllComments();
        var comments = await commentsIQueryable.ToPagedListAsync(commentGetFilter, _httpContextHelper);

        return comments.Select(MapToCommentModel).ToList();
    }


    public async Task<CommentModel> GetCommentByIdAsync(Guid commentId)
    {
        var commentIQueryable = _commentRepository.GetCommentById(commentId);
        var comment = await commentIQueryable.FirstOrDefaultAsync();

        if (comment is null)
            throw new CommentNotFoundException();
        return MapToCommentModel(comment);
    }

    public async Task<CommentModel> GetCommentByIdWithLikesAsync(Guid commentId)
    {
        var commentIQueryable = _commentRepository.GetCommentByIdWithLikes(commentId);
        var comment = await commentIQueryable.FirstOrDefaultAsync();

        if (comment is null)
            throw new CommentNotFoundException();

        return MapToCommentModel(comment);
    }

    public async Task<CommentModel> UpdateCommentAsync(Guid commentId, UpdateCommentModel model)
    {
        var commentIQueryable = _commentRepository.GetCommentById(commentId);
        var comment = await commentIQueryable.FirstOrDefaultAsync();

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
        var commentIQueryable = _commentRepository.GetCommentById(commentId);
        var comment = await commentIQueryable.FirstOrDefaultAsync();

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