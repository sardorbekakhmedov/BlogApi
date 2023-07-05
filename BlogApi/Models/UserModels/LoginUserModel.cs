namespace BlogApi.Models.UserModels;

public class LoginUserModel
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}