using System.Security.Claims;

namespace BlogApi.HelperServices;

public class UserProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserProvider(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    private HttpContext? Context => _contextAccessor.HttpContext;

    public string? UserName => Context?.User.FindFirstValue(ClaimTypes.Name);
    public string? Email => Context?.User.FindFirstValue(ClaimTypes.Email);

    public Guid? UserId
    {
        get
        {
            if (Guid.TryParse(Context?.User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
            {
                return userId;
            }
            return null;
        }
    }

}