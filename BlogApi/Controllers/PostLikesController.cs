using BlogApi.Interfaces.IManagers;
using BlogApi.Models.PostLikeModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostLikesController : ControllerBase
{
    private readonly IPostLikeManager _postLikeManager;

    public PostLikesController(IPostLikeManager postLikeManager)
    {
        _postLikeManager = postLikeManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddPostLike([FromForm] CreatePostLikeModel model)
    {
        return Ok(await _postLikeManager.AddPostLikeAsync(model));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPostLikes()
    {
        return Ok(await _postLikeManager.GetAllPostLikesAsync());
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