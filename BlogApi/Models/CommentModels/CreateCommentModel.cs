namespace BlogApi.Models.CommentModels;

public class CreateCommentModel
{
    public required string TextComment { get; set; }

    public Guid PostId { get; set; }
}