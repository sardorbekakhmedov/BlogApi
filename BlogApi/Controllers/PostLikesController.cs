using BlogApi.HelperEntities.Pagination;
using BlogApi.Interfaces.IManagers;
using BlogApi.Models.PostLikeModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostLikesController : ControllerBase
{
    private readonly IPostLikeManager _postLikeManager;
    private readonly IMemoryCache _memoryCache;

    public PostLikesController(IPostLikeManager postLikeManager, IMemoryCache memoryCache)
    {
        _postLikeManager = postLikeManager;
        _memoryCache = memoryCache;
    }

    [HttpPost("{postId}")]
    public async Task<IActionResult> AddPostLike(Guid postId)
    {
        return Ok(await _postLikeManager.AddPostLikeAsync(postId));
    }

    [HttpPost("pagination")]
    //[HttpGet]
    public async Task<IActionResult> GetAllPostLikes([FromForm] PostLikeGetFilter postLikeGetFilter)
    {
        var cacheKey = $"{postLikeGetFilter.Page}, {postLikeGetFilter.Size}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var postLikes = await _postLikeManager.GetAllPostLikesAsync();

            postLikes = postLikes.Skip((postLikeGetFilter.Page - 1) * postLikeGetFilter.Size).Take(postLikeGetFilter.Size).ToList();

            return Ok(postLikes);
        });

        return Ok(result);
    }

    [HttpGet("{postLikeId}")]
    public async Task<IActionResult> GetPostLikeByIdAsync(Guid postLikeId)
    {
        return Ok(await _postLikeManager.GetPostLikeByIdAsync(postLikeId));
    }

    [HttpDelete("{postLikeId}")]
    public async Task<IActionResult> DeletePostLikeAsync(Guid postLikeId)
    {
        await _postLikeManager.DeletePostLikeAsync(postLikeId);
        return Ok();
    }
}