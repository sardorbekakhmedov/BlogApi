using BlogApi.HelperEntities.Pagination;
using BlogApi.Models.PostModels;

namespace BlogApi.Interfaces.IManagers;

public interface IPostManager
{
    Task<PostModel> AddNewPostAsync(CreatePostModel model);
    Task<IEnumerable<PostModel>> GetAllPostsAsync(PostGetFilter postFilter);
    Task<PostModel> GetPostByIdWithLikesAndCommentsAsync(Guid postId);
    Task<PostModel> UpdatePostAsync(Guid postId, UpdatePostModel model);
    Task DeletePostAsync(Guid postId);
}