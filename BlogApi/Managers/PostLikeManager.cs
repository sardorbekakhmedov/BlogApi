using BlogApi.CustomExceptions.PostException;
using BlogApi.CustomExceptions.PostLikeException;
using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.PostLikeModels;

namespace BlogApi.Managers;

public class PostLikeManager : IPostLikeManager
{
    private readonly IPostLikeRepository _postLikeRepository;
    private readonly IPostRepository _postRepository;
    private readonly UserProvider _userProvider;

    public PostLikeManager(IPostLikeRepository postLikeRepository, IPostRepository postRepository,
        UserProvider userProvider)
    {
        _postLikeRepository = postLikeRepository;
        _postRepository = postRepository;
        _userProvider = userProvider;
    }

    public async Task<PostLikeModel> AddPostLikeAsync(CreatePostLikeModel model)
    {
        var post = await _postRepository.GetPostByIdAsync(model.PostId);

        if (post is null)
            throw new PostNotFoundException();

        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();

        var postLike = new PostLike
        {
            Id = Guid.NewGuid(),
            UserId = (Guid)userId,
            PostId = model.PostId
        };

        if (post.Likes is not null)
            post.Likes.Add(postLike);
        else
            post.Likes = new List<PostLike> { postLike };

        await _postRepository.UpdatePostAsync(post);
        await _postLikeRepository.CreatePostLikeAsync(postLike);

        return MapToPostLikeModel(postLike);
    }

    public async Task<List<PostLikeModel>> GetAllPostLikesAsync()
    {
        var postLikes = await _postLikeRepository.GetAllPostLikesAsync();

        return postLikes.Select(MapToPostLikeModel).ToList();
    }

    public async Task<PostLikeModel> GetPostLikeByIdAsync(Guid postLikeId)
    {
        var postLike = await _postLikeRepository.GetPostLikeByIdAsync(postLikeId);

        if(postLike is null)
            throw new PostLikeNotFoundException();

        return MapToPostLikeModel(postLike);
    }

    public async Task DeletePostLikeAsync(Guid postLikeId)
    {
        var postLike = await _postLikeRepository.GetPostLikeByIdAsync(postLikeId);

        if (postLike is not null)
        {
            var post = await _postRepository.GetPostByIdAsync(postLike.PostId);

            if (post is not null)
            {
                if (post.Likes is not null && post.Likes.Contains(postLike))
                {
                    post.Likes.Remove(postLike);
                    await _postRepository.UpdatePostAsync(post);
                }
            }
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