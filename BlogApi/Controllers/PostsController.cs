using BlogApi.Extensions;
using BlogApi.HelperEntities.Pagination;
using BlogApi.HelperServices;
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
    private readonly HttpContextHelper _contextHelper;

    public PostsController(IPostManager postManager, IMemoryCache memoryCache, HttpContextHelper contextHelper)
    {
        _postManager = postManager;
        _memoryCache = memoryCache;
        _contextHelper = contextHelper;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreatePostModel model)
    {
        return Ok(await _postManager.AddNewPostAsync(model));
    }
    
    [HttpPost("pagination")]
    //[HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts(PostFilter postFilter)
    {
        var cacheKey = $"{postFilter.Page}, {postFilter.Size}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var posts = await _postManager.GetAllPostsAsync();

           // posts = posts.Skip((postFilter.Page - 1) * postFilter.Size).Take(postFilter.Size).ToList();

            return Ok(posts.ToPagedListAsync(postFilter, _contextHelper));
        });

        return Ok(result);
    }

    [HttpGet("{postId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostByIdWithLikesAndComments(Guid postId)
    {
        var post = await _postManager.GetPostByIdWithLikesAndCommentsAsync(postId);
        return Ok(post);
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(Guid postId, [FromForm] UpdatePostModel model)
    {
        var post = await _postManager.UpdatePostAsync(postId, model);
        return Ok(post);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePostPost(Guid postId)
    {
        await _postManager.DeletePostAsync(postId);
        return Ok();
    }
}