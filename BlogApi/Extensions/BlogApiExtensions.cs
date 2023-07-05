using BlogApi.Context;
using BlogApi.Entities;
using BlogApi.HelperServices;
using BlogApi.HelperServices.JwtServices;
using BlogApi.Interfaces;
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

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<BlogApiDbContext>(config =>
        {
            config.UseNpgsql(configuration.GetConnectionString("BlogApiDb"));
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