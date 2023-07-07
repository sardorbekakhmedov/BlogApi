using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Extensions;
using BlogApi.HelperEntities.Pagination;
using BlogApi.HelperServices;
using BlogApi.Interfaces.IManagers;
using BlogApi.Models.CommentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentsController : ControllerBase
{
    private readonly ICommentManager _commentManager;
    private readonly IMemoryCache _memoryCache;
    private readonly HttpContextHelper _httpContext;

    public CommentsController(ICommentManager commentManager, IMemoryCache memoryCache, HttpContextHelper httpContext)
    {
        _commentManager = commentManager;
        _memoryCache = memoryCache;
        _httpContext = httpContext;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreateCommentModel model)
    {
        return Ok(await _commentManager.AddNewCommentAsync(model));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts([FromQuery] CommentGetFilter commentGetFilter)
    {
        var cacheKey = $"comment-controller-get, {commentGetFilter.Page}, {commentGetFilter.Size}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var comments = await _commentManager.GetAllCommentsAsync(commentGetFilter);

            return Ok(comments);
        });

        return Ok(result?.Value);
    }

    [HttpGet("{commentId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPostByIdWithLikes(Guid commentId)
    {
        return Ok(await _commentManager.GetCommentByIdWithLikesAsync(commentId));
    }

    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdatePost(Guid commentId, [FromForm] UpdateCommentModel model)
    {
        return Ok(await _commentManager.UpdateCommentAsync(commentId, model));
    }

    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeletePostPost(Guid commentId)
    {
        await _commentManager.DeleteCommentAsync(commentId);
        return Ok();
    }
}