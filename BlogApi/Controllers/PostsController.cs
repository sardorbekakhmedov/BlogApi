using BlogApi.Extensions;
using BlogApi.Interfaces.IManagers;
using BlogApi.Models.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostManager _postManager;
    private readonly IMemoryCache _memoryCache;

    public PostsController(IPostManager postManager, IMemoryCache memoryCache)
    {
        _postManager = postManager;
        _memoryCache = memoryCache;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreatePostModel model)
    {
        return Ok(await _postManager.AddNewPostAsync(model));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts(int postPage, int postCount)
    {
        var cacheKey = $"{postPage}, {postCount}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(5);

            var posts = await _postManager.GetAllPostsAsync();

            posts = posts.Skip((postPage - 1) * postCount).Take(postCount).ToList();

            return Ok(posts);
        });

        return Ok(result);
    }

    [HttpGet("{postId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostByIdWithLikes(Guid postId)
    {
        return Ok(await _postManager.GetPostByIdWithLikesAndCommentsAsync(postId));
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromForm] UpdatePostModel model)
    {
        return Ok(await _postManager.UpdatePostAsync(postId, model));
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePostPost(Guid postId)
    {
        await _postManager.DeletePostAsync(postId);
        return Ok();
    }
}