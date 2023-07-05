namespace BlogApi.Models;

public class LoginUserModel
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}