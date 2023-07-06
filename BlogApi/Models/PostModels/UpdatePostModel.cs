namespace BlogApi.Models.PostModels;

public class UpdatePostModel
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Tag { get; set; }
    public IFormFile? Image { get; set; }
}