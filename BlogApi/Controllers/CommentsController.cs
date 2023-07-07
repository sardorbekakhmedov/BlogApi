using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.HelperEntities.Pagination;
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

    public CommentsController(ICommentManager commentManager, IMemoryCache memoryCache)
    {
        _commentManager = commentManager;
        _memoryCache = memoryCache;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreateCommentModel model)
    {
        return Ok(await _commentManager.AddNewCommentAsync(model));
    }

    [HttpPost("pagination")]
    //[HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts([FromForm] CommentGetFilter commentGetFilter)
    {
        var cacheKey = $"{commentGetFilter.Page}, {commentGetFilter.Size}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var comments = await _commentManager.GetAllCommentsAsync();

            comments = comments.Skip((commentGetFilter.Page - 1) * commentGetFilter.Size).Take(commentGetFilter.Size).ToList();

            return Ok(comments);
        });

        return Ok(result);
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