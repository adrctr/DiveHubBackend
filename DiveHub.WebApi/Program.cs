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

// CORS Policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // si tu envoies des tokens ou cookies
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

#region EF Core PostgreSQL
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") //GetEnvironment Variable for Render purpose
                       ?? builder.Configuration.GetConnectionString("PostgresConnection");

builder.Services.AddDbContext<DiveHubDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IDiveRepository, DiveRepository>();
builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddDatabaseInitialization(connectionString!);
#endregion

#region services
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

// Initialise la DB au démarrage
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    initializer.Initialize();
}

app.MapOpenApi();
app.MapScalarApiReference();

// ⚠️ Logger après Build()
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Chaîne de connexion utilisée : {ConnectionString}", connectionString);
// ⚠️ Ordre correct
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");  // après HTTPS, avant les controllers
app.UseAuthorization();         // si tu ajoutes auth
app.UseStaticFiles();
app.MapControllers();

app.Run();
