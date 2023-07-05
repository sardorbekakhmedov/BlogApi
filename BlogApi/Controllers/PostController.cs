using BlogApi.Interfaces;
using BlogApi.Models.PostModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;

    public PostController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddNewPost([FromForm] CreatePostModel model)
    {
        return Ok(await _postRepository.AddNewPostAsync(model));
    }


    [HttpGet]
    public async Task<IActionResult> GetAllPosts()
    {
        return Ok(await _postRepository.GetAllPostsAsync());
    }
}