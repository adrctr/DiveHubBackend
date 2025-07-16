using DiveHub.Application.Interfaces;
using DiveHub.Application.Mapping;
using DiveHub.Application.Services;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Extensions;
using DiveHub.Infrastructure.Persistence;
using DiveHub.Infrastructure.repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

#region EF Core SQLite
// Ajout des services nécessaires
builder.Services.AddDbContext<SQLiteDbContext>(options =>
    options.UseSqlite("Data Source=DiveHubDB.db"));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDiveRepository, DiveRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

builder.Services.AddDatabaseInitialization("Data Source=DiveHubDB.db");
#endregion
#region services
// Ajouter les services de l'application
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiveService, DiveService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();

#endregion

#region AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

#region Authentication
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://dev-dxwbqxcepg0d5pa0.ca.auth0.com/";
        options.Audience = "https://divehub.api";
    });

builder.Services.AddAuthorization();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Crée la base de données au démarrage si elle n'existe pas
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        initializer.Initialize(); // Appelle la méthode pour créer la base de données
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
}

app.UseAuthentication();

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();