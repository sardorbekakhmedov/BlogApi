namespace BlogApi.Entities;

public class CommentLike : LikeParentClass
{
    public Guid CommentId { get; set; }
    public virtual Comment Comment { get; set; } = null!;
}