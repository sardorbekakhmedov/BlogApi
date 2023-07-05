using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.Interfaces;
using BlogApi.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Repositories;

public class PostRepository : IPostRepository
{
    private readonly UserProvider _userProvider;
    private readonly BlogApiDbContext _dbContext;
    private readonly IFileService _fileService;

    public PostRepository(BlogApiDbContext dbContext, IFileService fileService, UserProvider userProvider)
    {
        _dbContext = dbContext;
        _fileService = fileService;
        _userProvider = userProvider;
    }

}