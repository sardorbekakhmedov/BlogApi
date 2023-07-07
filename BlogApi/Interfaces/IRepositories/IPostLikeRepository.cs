using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface IPostLikeRepository
{
    Task CreatePostLikeAsync(PostLike postLike);
    IQueryable<PostLike> GetAllPostLikes();
    IQueryable<PostLike> GetPostLikeById(Guid postLikeId);
    IQueryable<PostLike> GetPostLikeByUserId(Guid userId);
    Task DeletePostLikeAsync(PostLike postLike);
}