using BlogApi.Interfaces.IManagers;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BlogApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserManager _userManager;
    private readonly IMemoryCache _memoryCache;

    public UsersController(IUserManager userManager, IMemoryCache memoryCache)
    {
        _userManager = userManager;
        _memoryCache = memoryCache;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllUsers(int userPage, int userCount)
    {
        var cacheKey = $"{userPage}, {userCount}";

        var result = await _memoryCache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.SlidingExpiration = TimeSpan.FromMinutes(1);

            var users = await _userManager.GetAllUsersAsync();

            users = users.Skip((userPage - 1) * userCount).Take(userCount).ToList();

            return Ok(users);
        });

        return Ok(result);
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