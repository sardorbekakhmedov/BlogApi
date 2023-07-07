using BlogApi.CustomExceptions.PostException;
using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.Extensions;
using BlogApi.HelperEntities.Pagination;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.CommentModels;
using BlogApi.Models.PostModels;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Managers;

public class PostManager : IPostManager
{
    private const string PostImagesFolderName = "PostImages";
    private readonly UserProvider _userProvider;
    private readonly HttpContextHelper _httpContextHelper;
    private readonly IPostRepository _postRepository;
    private readonly ICommentManager _commentManager;
    private readonly IFileService _fileService;

    public PostManager(IPostRepository postRepository, ICommentManager commentManager, IFileService fileService,
        UserProvider userProvider, HttpContextHelper httpContextHelper)
    {
        _postRepository = postRepository;
        _commentManager = commentManager;
        _fileService = fileService;
        _userProvider = userProvider;
        _httpContextHelper = httpContextHelper;
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

    public async Task<IEnumerable<PostModel>> GetAllPostsAsync(PostGetFilter postFilter)
    {
        var postsIQueryable = _postRepository.GetAllPosts();

        if (postFilter.FromDateTime is not null)
        {
            postsIQueryable = postsIQueryable.Where(post => post.CreatedDate > postFilter.FromDateTime);
        }

        if (postFilter.ToDateTime is not null)
        {
            postsIQueryable = postsIQueryable.Where(post => post.CreatedDate < postFilter.ToDateTime);
        }

        if (postFilter.Tag is not null)
        {
            postsIQueryable = postsIQueryable.Where(post => post.Tag.Contains(postFilter.Tag));
        }

        postsIQueryable = postsIQueryable.OrderBy(o => o.CreatedDate);

        var posts = await postsIQueryable.ToPagedListAsync(postFilter, _httpContextHelper);

        return posts.Select(MapToPostModel).ToList();
    }

    public async Task<PostModel> GetPostByIdWithLikesAndCommentsAsync(Guid postId)
    {
        var postsIQueryable = _postRepository.GetPostByIdWithLikesAndComments(postId);
        var post = await postsIQueryable.FirstOrDefaultAsync();

        if (post is null)
            throw new PostNotFoundException();

        post.ViewCount++;
        await _postRepository.UpdatePostAsync(post);

        return MapToPostModel(post);
    }

    public async Task<PostModel> UpdatePostAsync(Guid postId, UpdatePostModel model)
    {
        var postIQueryable = _postRepository.GetPostById(postId);
        var post = await postIQueryable.FirstOrDefaultAsync();

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
        var postIQueryable = _postRepository.GetPostById(postId);
        var post = await postIQueryable.FirstOrDefaultAsync();

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