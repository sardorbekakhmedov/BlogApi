namespace BlogApi.Entities;

public class PostLike : LikeParentClass
{
    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;
}