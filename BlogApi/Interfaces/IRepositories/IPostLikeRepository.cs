using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface IPostLikeRepository
{
    Task CreatePostLikeAsync(PostLike postLike);
    Task<List<PostLike>> GetAllPostLikesAsync();
    Task<PostLike?> GetPostLikeByIdAsync(Guid postLikeId);
    Task DeletePostLikeAsync(PostLike postLike);
}