
using DiveHub.Application.Interfaces;
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
// Ajouter les services de stockage JSON pour Dive et DivePoint
builder.Services.AddSingleton<IStorageService<User>>(provider => new FileStorageService<User>("Data/users.json"));
builder.Services.AddSingleton<IStorageService<Dive>>(provider => new FileStorageService<Dive>("Data/dives.json"));
builder.Services.AddSingleton<IStorageService<DivePoint>>(provider => new FileStorageService<DivePoint>("Data/divepoints.json"));
builder.Services.AddSingleton<IStorageService<DivePhoto>>(provider => new FileStorageService<DivePhoto>("Data/divephoto.json"));

// Ajouter les services de l'application
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiveService, DiveService>();
builder.Services.AddScoped<IDivePointService,DivePointService>();
builder.Services.AddScoped<IDivePhotoService,DivePhotoService>();

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