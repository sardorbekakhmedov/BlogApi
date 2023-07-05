using BlogApi.Models.PostModels;

namespace BlogApi.Interfaces;

public interface IPostRepository
{
    Task<PostModel> AddNewPostAsync(CreatePostModel model);
    Task<List<PostModel>> GetAllPostsAsync();
}