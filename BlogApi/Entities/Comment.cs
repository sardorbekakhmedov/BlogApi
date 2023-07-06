using System.ComponentModel.DataAnnotations;

namespace BlogApi.Entities;

public class Comment
{
    [Key]
    public Guid Id { get; set; }
    public required string TextComment { get; set; }


    public Guid UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public Guid PostId { get; set; }
    public virtual Post Post { get; set; } = null!;

    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
}