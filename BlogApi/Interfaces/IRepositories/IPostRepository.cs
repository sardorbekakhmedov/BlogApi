using BlogApi.Entities;
using BlogApi.HelperEntities.Pagination;

namespace BlogApi.Interfaces.IRepositories;

public interface IPostRepository
{
    Task CreatePostAsync(Post post);
    IQueryable<Post> GetAllPosts();
    IQueryable<Post> GetPostById(Guid postId);
    IQueryable<Post> GetPostByIdWithLikesAndComments(Guid postId);
    Task UpdatePostAsync(Post post);
    Task DeletePostAsync(Post post);
}