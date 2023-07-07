using BlogApi.Entities;

namespace BlogApi.Interfaces.IRepositories;

public interface IUserRepository
{
    Task CreateUserAsync(User user);
    IQueryable<User> GetAllUsers();
    IQueryable<User> GetUserById(Guid userId);
    IQueryable<User> GetUserByUserName(string userName);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(User user);
}