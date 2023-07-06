using BlogApi.Interfaces.IManagers;
using BlogApi.Models.CommentModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ICommentManager _commentManager;

    public CommentController(ICommentManager commentManager)
    {
        _commentManager = commentManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreateCommentModel model)
    {
        return Ok(await _commentManager.AddNewCommentAsync(model));
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPosts()
    {
        return Ok(await _commentManager.GetAllCommentsAsync());
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