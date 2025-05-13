using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.Extensions.Hosting;
using Application.Activities.Queries;
using Application.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var env = builder.Environment;

string certPath = "/home/andres/Desktop/The_Final_Boss/Reactivities/API/certs/localhost+2.pem";
string keyPath = "/home/andres/Desktop/The_Final_Boss/Reactivities/API/certs/localhost+2-key.pem";

builder.WebHost.ConfigureKestrel(options =>
{
    if (env.IsDevelopment()) // Check if in Development environment
    {
        // Load the certificate and key using X509Certificate2
        var certificate = X509Certificate2.CreateFromPemFile(certPath, keyPath);

        // Configure Kestrel to use the certificate for HTTPS on localhost
        options.ListenLocalhost(5001, listenOptions =>
        {
            listenOptions.UseHttps(certificate);
        });
    }
    else
    {
        // Production setup can go here (real certs)
        // For example, options.ListenAnyIP(5001, listenOptions => { listenOptions.UseHttps("path_to_production_cert", "path_to_production_key"); });
    }
});

builder.Services.AddCors();

builder.Services.AddMediatR(x => x.RegisterServicesFromAssemblyContaining<GetActivityList.Handler>());

builder.Services.AddAutoMapper(typeof(MappingProfiles).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000", "https://localhost:3000"));

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
