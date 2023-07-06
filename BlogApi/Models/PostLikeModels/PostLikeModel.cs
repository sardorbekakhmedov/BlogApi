namespace BlogApi.Models.PostLikeModels;

public class PostLikeModel
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid PostId { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}