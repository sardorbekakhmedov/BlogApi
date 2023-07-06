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

    [HttpPost]
    public async Task<IActionResult> AddPostLike([FromForm] CreatePostLikeModel model)
    {
        return Ok(await _postLikeManager.AddPostLikeAsync(model));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPostLikes(int page, int count)
    {
        var cacheKey = $"{page}, {count}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(5);

            var postLikes = await _postLikeManager.GetAllPostLikesAsync();

            postLikes = postLikes.Skip((page - 1) * count).Take(count).ToList();

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