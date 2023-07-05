using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Context;

public sealed class BlogApiDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<CommentLike> CommentLikes => Set<CommentLike>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostLike> PostLikes => Set<PostLike>();

    public BlogApiDbContext(DbContextOptions<BlogApiDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
        //Database.Migrate();
    }
}