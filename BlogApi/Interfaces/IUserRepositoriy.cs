using BlogApi.Entities;
using BlogApi.Models.UserModels;

namespace BlogApi.Interfaces;

public interface IUserRepository
{
    Task<UserModel> RegisterAsync(CreateUserModel model);
    Task<string> LoginAsync(LoginUserModel model);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserAsync(Guid userId);
    Task<UserModel> GetUserAsync(string userName);
    Task<UserModel> UpdateAsync(UpdateUserModel model);
    Task DeleteAsync(Guid userId);
}