namespace BlogApi.Models;

public class UserModel
{
    public Guid Id { get; set; }
    public string? ImagePath { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}