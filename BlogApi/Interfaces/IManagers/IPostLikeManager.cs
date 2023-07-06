using BlogApi.Models.PostLikeModels;

namespace BlogApi.Interfaces.IManagers;

public interface IPostLikeManager
{
    Task<PostLikeModel> AddPostLikeAsync(CreatePostLikeModel model);
    Task<List<PostLikeModel>> GetAllPostLikesAsync();
    Task<PostLikeModel> GetPostLikeByIdAsync(Guid postLikeId);
    Task DeletePostLikeAsync(Guid postLikeId);
}