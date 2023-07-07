using BlogApi.CustomExceptions.PostLikeException;
using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.PostLikeModels;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers;

public class PostLikeManager : IPostLikeManager
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly UserProvider _userProvider;

    public PostLikeManager(IPostLikeRepository postLikeRepository, UserProvider userProvider)
    {
        _postLikeRepository = postLikeRepository;
        _userProvider = userProvider;
    }

    public async Task<PostLikeModel> AddPostLikeAsync(Guid postId)
    {
        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();

        var postLikeIQueryable = _postLikeRepository.GetPostLikeByUserId((Guid)userId);
        var postLike = await postLikeIQueryable.FirstOrDefaultAsync();

        if  (postLike is null)
        {
            postLike = new PostLike
            {
                Id = Guid.NewGuid(),
                UserId = (Guid)userId,
                PostId = postId
            };

            await _postLikeRepository.CreatePostLikeAsync(postLike);

            return MapToPostLikeModel(postLike);
        }

        await DeletePostLikeAsync(postLike.Id);

        return new PostLikeModel();
    }

    public async Task<IEnumerable<PostLikeModel>> GetAllPostLikesAsync()
    {
        var postLikeIQueryable = _postLikeRepository.GetAllPostLikes();
        var postLikes = await postLikeIQueryable.ToListAsync();

        return postLikes.Select(MapToPostLikeModel).ToList();
    }

    public async Task<PostLikeModel> GetPostLikeByIdAsync(Guid postLikeId)
    {
        var postLikeIQueryable = _postLikeRepository.GetPostLikeByUserId(postLikeId);
        var postLike = await postLikeIQueryable.FirstOrDefaultAsync();

        if (postLike is null)
            throw new PostLikeNotFoundException();

        return MapToPostLikeModel(postLike);
    }

    public async Task DeletePostLikeAsync(Guid postLikeId)
    {
        var postLikeIQueryable = _postLikeRepository.GetPostLikeByUserId(postLikeId);
        var postLike = await postLikeIQueryable.FirstOrDefaultAsync();

        if (postLike is not null)
        {
            await _postLikeRepository.DeletePostLikeAsync(postLike);
        }
    }

    private PostLikeModel MapToPostLikeModel(PostLike postLike)
    {
        var postLikeModel = new PostLikeModel
        {
            Id = postLike.Id,
            PostId = postLike.PostId,
            UserId = postLike.UserId,
            CreatedDate = postLike.CreatedDate,
        };

        return postLikeModel;
    }
}
