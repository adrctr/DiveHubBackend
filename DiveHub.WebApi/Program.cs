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

//#region EF Core SQLite
//// Ajout des services nécessaires
//builder.Services.AddDbContext<DiveHubDbContext>(options =>
//    options.UseSqlite("Data Source=DiveHubDB.db"));

//builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped<IDiveRepository, DiveRepository>();
//builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();

//builder.Services.AddDatabaseInitialization("Data Source=DiveHubDB.db");
//#endregion

#region EF Core PostgreSQL

// Récupère la connexion : 
// 1. Si DATABASE_URL est défini en variable d’env → prend ça
// 2. Sinon → prend depuis appsettings.json / appsettings.Development.json
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL")
                       ?? builder.Configuration.GetConnectionString("PostgresConnection");

// Ajout des services nécessaires 
builder.Services.AddDbContext<DiveHubDbContext>(options =>
    options.UseNpgsql(connectionString));

// Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDiveRepository, DiveRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDatabaseInitialization(connectionString);
#endregion

#region services
// Ajouter les services de l'application
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiveService, DiveService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();

#endregion

#region AutoMapper
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<MappingProfile>();
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    // Crée la base de données au démarrage si elle n'existe pas
    using (var scope = app.Services.CreateScope())
    {
        var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
        initializer.Initialize(); // Appelle la méthode pour créer la base de données
    }

    app.MapOpenApi();
    app.MapScalarApiReference();
    //app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader());
    app.UseCors(options => options.WithOrigins("https://divehub-ui.onrender.com").AllowAnyMethod().AllowAnyHeader());

//}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();