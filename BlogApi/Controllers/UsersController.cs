using BlogApi.Interfaces.IManagers;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManager _userManager;

    public UsersController(IUserManager userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers()
    {
        return Ok(await _userManager.GetAllUsersAsync());
    }

    [HttpGet("{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        return Ok(await _userManager.GetUserByIdAsync(userId));
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        return Ok(await _userManager.Profile());
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromForm] UpdateUserModel model )
    {
        return Ok(await _userManager.UpdateAsync(model));
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete(Guid userId)
    {
        await _userManager.DeleteAsync(userId);
        return Ok();
    }
}