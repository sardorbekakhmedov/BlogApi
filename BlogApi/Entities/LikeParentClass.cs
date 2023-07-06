using System.ComponentModel.DataAnnotations;

namespace BlogApi.Entities;

public abstract class LikeParentClass
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}