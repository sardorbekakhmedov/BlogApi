using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Models.PostModels;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class PostRepository : IPostRepository
{
    private readonly UserProvider _userProvider;
    private readonly BlogApiDbContext _dbContext;
    private readonly IFileService _fileService;

    public PostRepository(BlogApiDbContext dbContext, IFileService fileService, UserProvider userProvider)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _userProvider = userProvider;
    }

    public async Task<PostModel> AddNewPostAsync(CreatePostModel model)
    {
        var post = new Post
        {
            Tag = model.Tag,
            Title = model.Title,
            Content = model.Content,
            UserId = model.UserId,
        };

        if (model.Image is not null)
            post.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, "PostImages");

        await _dbContext.Posts.AddAsync(post);
        await _dbContext.SaveChangesAsync();

        return MapToPostModel(post);
    }

    public async Task<List<PostModel>> GetAllPostsAsync()
    {
        var posts = await _dbContext.Posts.ToListAsync();

        return posts.Select(MapToPostModel).ToList();
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
            CreatedDate = model.CreatedDate,
            UpdatedDate = model.UpdatedDate
        };

        if (model.Likes is not null)
            postModel.LikeCount = model.Likes.Count;

        return postModel;
    }

}