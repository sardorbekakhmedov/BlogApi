using BlogApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlogApiServices(builder.Configuration);


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseSwagger();
//app.UseSwaggerUI();

//app.UseAutoMigrateBlogApiDb();  // Auto-migrate
//app.UseHttpContextHelper(); // Custom extension for pagination

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


public partial class Program
{ }

