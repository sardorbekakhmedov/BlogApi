using BlogApi.Entities;

namespace BlogApi.Interfaces;

public interface IJwtToken
{
    string CreateJwtToken(User user);
}