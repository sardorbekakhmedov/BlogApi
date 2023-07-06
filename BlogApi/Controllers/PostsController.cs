using BlogApi.Interfaces.IManagers;
using BlogApi.Models.PostModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostManager _postManager;

    public PostsController(IPostManager postManager)
    {
        _postManager = postManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreatePostModel model)
    {
        return Ok(await _postManager.AddNewPostAsync(model));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts()
    {
        return Ok(await _postManager.GetAllPostsAsync());
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