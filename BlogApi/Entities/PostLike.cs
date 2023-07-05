namespace BlogApi.Entities;

public class PostLike : LikeParentClass
{
    public Guid BlogId { get; set; }
    public virtual Post Blog { get; set; } = null!;
}