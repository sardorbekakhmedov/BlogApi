using BlogApi.Entities;

namespace BlogApi.Models.CommentModels;

public class UpdateCommentModel
{
    public string? TextComment { get; set; }

    public Guid? PostId { get; set; }

}