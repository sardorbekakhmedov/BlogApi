using Microsoft.IdentityModel.Tokens;

namespace BlogApi.HelperServices.JwtServices;


public class CustomLifeTime
{
    public static bool CustomLifeTimeValidator(DateTime? notBefore, DateTime? expires,
        SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        if (expires != null)
            return expires > DateTime.UtcNow;

        return false;
    }
}