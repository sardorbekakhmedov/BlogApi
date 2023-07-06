using BlogApi.Models.PostModels;

namespace BlogApi.Interfaces.IManagers;

public interface IPostManager
{
    Task<PostModel> AddNewPostAsync(CreatePostModel model);
    Task<List<PostModel>> GetAllPostsAsync();
    Task<PostModel> GetPostByIdAsync(Guid postId);
    Task<PostModel> GetPostByIdWithLikesAsync(Guid postId);
    Task<PostModel> UpdatePostAsync(Guid postId, UpdatePostModel model);
    Task DeletePostAsync(Guid postId);
}