using Microsoft.EntityFrameworkCore;
using Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapControllers();


// creating a service scope
// when we run the application, this will be cleaned up
using var scope = app.Services.CreateScope();

var service = scope.ServiceProvider;

try
{
    var context = service.GetRequiredService<AppDbContext>();

    await context.Database.MigrateAsync();

    await DbInitializer.SeedData(context);
}
catch (Exception ex)
{
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migration");
}

app.Run();
