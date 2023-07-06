using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.HelperServices.JwtServices;
using BlogApi.Interfaces;
using BlogApi.Interfaces.IManagers;
using BlogApi.Interfaces.IRepositories;
using BlogApi.Managers;
using BlogApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Extensions;

public static class BlogApiExtensions
{
    public static void AddBlogApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddHttpContextAccessor();
        services.AddScoped<UserProvider>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IPostManager, PostManager>();
        services.AddScoped<ICommentManager, CommentManager>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<BlogApiDbContext>(config =>
        {
            var connectionString = configuration.GetConnectionString("BlogApiDb");
            config.UseNpgsql(connectionString);
        });

        services.AddScoped<JwtOptions>();
        services.AddJwtConfiguration(configuration);
        services.AddScoped<IJwtToken, JwtToken>();

        services.AddSwaggerGenWithToken();
    }
    public static void UseAutoMigrateBlogApiDb(this WebApplication app)
    {
        if (app.Services.GetService<BlogApiDbContext>() != null)
        {
            var blogApiDb = app.Services.GetRequiredService<BlogApiDbContext>();
            blogApiDb.Database.Migrate();
        }
    }
}