namespace BlogApi.Entities;

public class User
{
    public Guid Id { get; set; }
    public string? ImagePath { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public string PasswordHash { get; set; } = null!;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }

    public virtual List<Post>? Posts { get; set; }
    public virtual List<PostLike>? PostLikes { get; set; }
    public virtual List<Comment>? Comments { get; set; }
    public virtual List<CommentLike>? CommentLikes { get; set; }
}