using System.ComponentModel.DataAnnotations;

namespace BlogApi.Models;

public class CreateUserModel
{
    public IFormFile? Image { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    [Compare("Password")]
    public required string ConfirmPassword { get; set; }
}