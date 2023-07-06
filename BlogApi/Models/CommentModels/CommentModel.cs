using BlogApi.Entities;

namespace BlogApi.Models.CommentModels;

public class CommentModel
{
    public Guid Id { get; set; }
    public required string TextComment { get; set; }

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int CommentLikesCount { get; set; }
}