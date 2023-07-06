namespace BlogApi.Models.PostModels;

public class CreatePostModel
{
    public required string Tag { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public IFormFile? Image { get; set; }
}