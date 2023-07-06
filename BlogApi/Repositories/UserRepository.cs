using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BlogApiDbContext _dbContext;

    public UserRepository(BlogApiDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateUserAsync(User user)
    {
        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _dbContext.Users.Where(user => !user.IsDeleted).ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId && !user.IsDeleted);
    }

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName && !user.IsDeleted);
    }

    public async Task UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(User user)
    {
        user.IsDeleted = true;
        await _dbContext.SaveChangesAsync();
    }
}