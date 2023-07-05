using BlogApi.Interfaces;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] CreateUserModel model)
    {
        return Ok(await _userRepository.RegisterAsync(model));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginUserModel model)
    {
        return Ok(await _userRepository.LoginAsync(model));
    }


    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userRepository.GetAllUsersAsync());
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAllUsers(Guid userId)
    {
        return Ok(await _userRepository.GetUserAsync(userId));
    }


    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update([FromForm] UpdateUserModel model )
    {
        return Ok(await _userRepository.UpdateAsync(model));
    }

    [HttpDelete("{userId}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _userRepository.DeleteAsync(userId);
        return Ok();
    }
}