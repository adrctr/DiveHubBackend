
using DiveHub.Application.Services;
using DiveHub.Core.Entities;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region services
builder.Services.AddSingleton<IStorageService<User>>(_ =>
{
    const string filePath = "Data/users.json"; // Optionally, retrieve this from configuration
    return new FileStorageService<User>(filePath);
});
builder.Services.AddScoped<UserService>();

builder.Services.AddSingleton<IStorageService<Dive>>(_ =>
{
    const string filePath = "Data/dives.json"; // Optionally, retrieve this from configuration
    return new FileStorageService<Dive>(filePath);
});
builder.Services.AddScoped<DiveService>();


builder.Services.AddSingleton<IStorageService<DivePoint>>(_ =>
{
    const string filePath = "Data/divesPoint.json"; // Optionally, retrieve this from configuration
    return new FileStorageService<DivePoint>(filePath);
});

builder.Services.AddScoped<DivePointService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();