using BlogApi.CustomExceptions.UserExceptions;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace BlogApi.Managers;

public class UserManager : IUserManager
{
    private const string UserImagesFolderName = "UserImages";
    private readonly IJwtToken _jwtToken;
    private readonly UserProvider _userProvider;
    private readonly IUserRepository _userRepository;
    private readonly IFileService _fileService;

    public UserManager(IUserRepository userRepository, IFileService fileService, IJwtToken jwtToken, UserProvider userProvider)
    {
        _userRepository = userRepository;
        _fileService = fileService;
        _jwtToken = jwtToken;
        _userProvider = userProvider;
    }

    public async Task<UserModel> Profile()
    {
        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();

        var user = await _userRepository.GetUserByIdAsync((Guid)userId);

        if (user is null)
            throw new UserNotFoundException();

        return MapToUserModel(user);
    }

    public async Task<UserModel> RegisterAsync(CreateUserModel model)
    {
        if (await _userRepository.GetUserByUserNameAsync(model.UserName) is not null)
        {
            throw new UsernameAlreadyExistsException(model.UserName);
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
            user.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, UserImagesFolderName);

        await _userRepository.CreateUserAsync(user);

        return MapToUserModel(user);
    }

    public async Task<string> LoginAsync(LoginUserModel model)
    {
        var user = await _userRepository.GetUserByUserNameAsync(model.UserName);

        if (user is null)
            throw new UsernameIncorrectException(model.UserName);

        var result = new PasswordHasher<User>()
            .VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if (result != PasswordVerificationResult.Success)
            throw new PasswordIncorrectException(model.Password);

        var jwtToken = _jwtToken.CreateJwtToken(user);

        return jwtToken;
    }

    public async Task<List<UserModel>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();

        return users.Select(MapToUserModel).ToList();
    }

    public async Task<UserModel> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return MapToUserModel(user ?? throw new UserNotFoundException());
    }

    public async Task<UserModel> GetUserByUserNameAsync()
    {
        var userName = _userProvider.UserName;

        if (userName is null)
            throw new UserNotFoundException();

        var user = await _userRepository.GetUserByUserNameAsync(userName);
        return MapToUserModel(user ?? throw new UserNotFoundException());
    }

    public async Task<UserModel> UpdateAsync(UpdateUserModel model)
    {
        var userId = _userProvider.UserId;

        if (userId is null)
            throw new UserNotFoundException();
        
        var user = await _userRepository.GetUserByIdAsync((Guid)userId);

        if (user is null)
            throw new UserNotFoundException();

        user.FirstName = model.FirstName ?? user.FirstName;
        user.LastName = model.LastName ?? user.LastName;
        user.UserName = model.UserName ?? user.UserName;
        user.Email = model.Email ?? user.Email;
        user.UpdatedDate = DateTime.UtcNow;

        if (model.Image is not null)
            user.ImagePath = await _fileService.SaveFileToWwwrootAsync(model.Image, UserImagesFolderName);

        if (!string.IsNullOrEmpty(model.Password))
            user.PasswordHash = new PasswordHasher<User>().HashPassword(user, model.Password);

        await _userRepository.UpdateUserAsync(user);

        return MapToUserModel(user);
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user is not null)
        {
            user.IsDeleted = true;
            await _userRepository.DeleteUserAsync(user);
        }
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