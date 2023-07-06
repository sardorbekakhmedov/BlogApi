using BlogApi.CustomExceptions.PostException;
using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.CommentModels;
using BlogApi.Models.PostModels;

namespace BlogApi.Managers;

public class PostManager : IPostManager
{
    private const string PostImagesFolderName = "PostImages";
    private readonly UserProvider _userProvider;
    private readonly IPostRepository _postRepository;
    private readonly ICommentManager _commentManager;
    private readonly IFileService _fileService;

    public PostManager(IPostRepository postRepository, ICommentManager commentManager, IFileService fileService, UserProvider userProvider)
    {
        _postRepository = postRepository;
        _commentManager = commentManager;
        _fileService = fileService;
        _userProvider = userProvider;
    }

    public async Task<PostModel> AddNewPostAsync(CreatePostModel model)
    {
        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();

        var post = new Post
        {
            Tag = model.Tag,
            Title = model.Title,
            Content = model.Content,
            UserId = (Guid)userId,
            Likes = new List<PostLike>(),
            Comments = new List<Comment>()
        };

        if (model.Image is not null)
            post.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, PostImagesFolderName);

        await _postRepository.CreatePostAsync(post);

        return MapToPostModel(post);
    }

    public async Task<List<PostModel>> GetAllPostsAsync()
    {
        var posts = await _postRepository.GetAllPostsAsync();

        return posts.Select(MapToPostModel).ToList();
    }


    public async Task<PostModel> GetPostByIdAsync(Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if(post is null)
            throw new PostNotFoundException();

        post.ViewCount++;
        await _postRepository.UpdatePostAsync(post);

        return MapToPostModel(post);
    }

    public async Task<PostModel> GetPostByIdWithLikesAndCommentsAsync(Guid postId)
    {
        var post = await _postRepository.GetPostByIdWithLikesAndCommentsAsync(postId);

        if (post is null)
            throw new PostNotFoundException();

        post.ViewCount++;
        await _postRepository.UpdatePostAsync(post);

        return MapToPostModel(post);
    }

    public async Task<PostModel> UpdatePostAsync(Guid postId, UpdatePostModel model)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post == null)
            throw new PostNotFoundException();

        post.Title = model.Title ?? post.Content;
        post.Content = model.Content ?? post.Content;
        post.Tag = model.Tag ?? post.Tag;
        post.UpdatedDate = DateTime.UtcNow;

        if (model.Image is not null)
            post.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, PostImagesFolderName);

        await _postRepository.UpdatePostAsync(post);

        return MapToPostModel(post);
    }

    public async Task DeletePostAsync(Guid postId)
    {
        var post = await _postRepository.GetPostByIdAsync(postId);

        if (post is not null)
        {
            await _postRepository.DeletePostAsync(post);
        }
    }


    private PostModel MapToPostModel(Post model)
    {
        var postModel = new PostModel
        {
            Id = model.Id,
            Title = model.Title,
            Content = model.Content,
            Tag = model.Tag,
            UserId = model.UserId,
            ImagePath = model.ImagePath,
            ViewCount = model.ViewCount,
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate,
        };

        if (model.Likes is not null)
            postModel.LikeCount = model.Likes.Count;

        if (model.Comments is null)
            return postModel;

        postModel.Comments = new List<CommentModel>();

        foreach (var comment in model.Comments)
        {
            postModel.Comments.Add(_commentManager.MapToCommentModel(comment));
        }

        return postModel;
    }
}