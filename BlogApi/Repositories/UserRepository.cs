using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IJwtToken _jwtToken;
    private readonly UserProvider _userProvider;
    private readonly BlogApiDbContext _dbContext;
    private readonly IFileService _fileService;

    public UserRepository(BlogApiDbContext dbContext, IFileService fileService, IJwtToken jwtToken, UserProvider userProvider)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _jwtToken = jwtToken;
        _userProvider = userProvider;
    }

    public async Task<UserModel> RegisterAsync(CreateUserModel model)
    {
        if (await _dbContext.Users.AnyAsync(user => user.UserName == model.UserName && !user.IsDeleted))
        {
            throw new Exception("Username already exists!");
        }

        var user = new User
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            IsDeleted = false,
            UserName = model.UserName,
        };

        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password);

        if (model.Image is not null)
            user.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, "UserImages");

        await _dbContext.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return MapToUserModel(user);
    }

    public async Task<string> LoginAsync(LoginUserModel model)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == model.UserName && !user.IsDeleted);

        if (user is not null)
        {
            var result = new PasswordHasher<User>()
                .VerifyHashedPassword(user, user.PasswordHash, model.Password);

            if (result != PasswordVerificationResult.Success)
                throw new Exception("User password incorrect!");

            var jwtToken = _jwtToken.CreateJwtToken(user);

            return jwtToken;
        }

        throw new Exception("Username or Password incorrect!");
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = await _dbContext.Users.Where(user => !user.IsDeleted).ToListAsync();

        return users.Select(MapToUserModel).ToList();
    }
    
    public async Task<UserModel> GetUserAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId && !user.IsDeleted);
        return MapToUserModel(user ?? throw new Exception("User not found!"));
    }

    public async Task<UserModel> GetUserAsync(string userName)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName && !user.IsDeleted); 
        return MapToUserModel(user ?? throw new Exception("User not found!"));
    }

    public async Task<UserModel> UpdateAsync(UpdateUserModel model)
    {
        var userId = _userProvider.UserId;
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId && !user.IsDeleted);

        if (user is null)
            throw new Exception("User not found!");

        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.UserName = model.UserName ?? user.UserName;
        user.Email = model.Email ?? user.Email;
        user.UpdatedDate = DateTime.UtcNow;

        if (model.Image is not null)
            user.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, "UserImages");

        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password);
        
        await _dbContext.SaveChangesAsync();

        return MapToUserModel(user);
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == userId && !user.IsDeleted);

        if (user is null)
            throw new Exception("User not found!");
        
        if(user.ImagePath is not null)
            _fileService.DeleteFile(user.ImagePath);

        user.IsDeleted = true;
        await _dbContext.SaveChangesAsync();
    }

    private UserModel MapToUserModel(User user)
    {
        var userModel = new UserModel
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UpdatedDate = user.UpdatedDate,
            CreatedDate = user.CreatedDate,
            ImagePath = user.ImagePath,
        };

        return userModel;
    }
}