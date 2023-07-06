using BlogApi.Models.UserModels;

namespace BlogApi.Interfaces.IManagers;

public interface IUserManager
{
    Task<UserModel> Profile();
    Task<UserModel> RegisterAsync(CreateUserModel model);
    Task<string> LoginAsync(LoginUserModel model);
    Task<List<UserModel>> GetAllUsersAsync();
    Task<UserModel> GetUserByIdAsync(Guid userId);
    Task<UserModel> GetUserByUserNameAsync();
    Task<UserModel> UpdateAsync(UpdateUserModel model);
    Task DeleteAsync(Guid userId);
}