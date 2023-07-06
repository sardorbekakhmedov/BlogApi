using BlogApi.Interfaces.IManagers;
using BlogApi.Models.CommentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts(int page, int count)
    {
        var cacheKey = $"{page}, {count}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(5);

            var comments = await _commentManager.GetAllCommentsAsync();

            comments = comments.Skip((page - 1) * count).Take(count).ToList();

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