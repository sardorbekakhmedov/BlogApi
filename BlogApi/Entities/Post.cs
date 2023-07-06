using System;

namespace BlogApi.Entities;

public class Post
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Tag { get; set; }
    public string? ImagePath { get; set; }
    public int ViewCount { get; set; }
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public required Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public virtual List<PostLike>? Likes { get; set; }
    public virtual List<Comment>? Comments { get; set; }
}