using BlogApi.Models.CommentModels;

namespace BlogApi.Models.PostModels;

public class PostModel
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public required string Tag { get; set; }
    public string? ImagePath { get; set; }
    public int ViewCount { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime CreatedDate { get; set; } 

    public required Guid UserId { get; set; }

    public int LikeCount { get; set; }
    public virtual List<CommentModel>? Comments { get; set; }
}