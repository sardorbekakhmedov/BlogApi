using BlogApi.Interfaces.IManagers;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IUserManager _userManager;

    public AccountsController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] CreateUserModel model)
    {
        return Ok(await _userManager.RegisterAsync(model));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginUserModel model)
    {
        return Ok(await _userManager.LoginAsync(model));
    }
}