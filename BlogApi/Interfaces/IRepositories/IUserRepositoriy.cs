using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByUserNameAsync(string userName);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}