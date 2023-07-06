using BlogApi.Entities;
using BlogApi.Models.PostModels;

namespace BlogApi.Interfaces.IRepositories;

public interface IPostRepository
{
    Task CreatePostAsync(Post post);
    Task<List<Post>> GetAllPostsAsync();
    Task<Post?> GetPostByIdAsync(Guid postId);
    Task<Post?> GetPostByIdWithLikesAndCommentsAsync(Guid postId);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}