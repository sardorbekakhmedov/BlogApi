using BlogApi.Entities;

namespace BlogApi.Models.PostLikeModels;

public class CreatePostLikeModel
{
    public Guid PostId { get; set; }
}