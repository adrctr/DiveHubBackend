using DiveHub.Application.Interfaces;
using DiveHub.Application.Mapping;
using DiveHub.Application.Services;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Extensions;
using DiveHub.Infrastructure.Persistence;
using DiveHub.Infrastructure.repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());
#endregion

#region Authentication
#if DEBUG
IdentityModelEventSource.ShowPII = true;
#endif
var jwtSection = builder.Configuration.GetSection("Jwt");
var authority = jwtSection["Authority"];
var audience = jwtSection["Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = authority;
        options.Audience = audience;

        options.Events = new JwtBearerEvents
        {
#if DEBUG
            OnMessageReceived = context =>
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                Console.WriteLine($"Authorization header brute: '{authHeader}'");

                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    var token = authHeader.Substring("Bearer ".Length).Trim();
                    Console.WriteLine($"Token extrait manuellement: '{token}'");
                    context.Token = token; // assignation explicite
                }
                else
                {
                    Console.WriteLine("Authorization header absent ou mal formé.");
                }

                Console.WriteLine($"Token final dans context.Token : '{context.Token ?? "NULL"}'");

                return Task.CompletedTask;
            },
#endif
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine($"Token validé pour : {context.Principal.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

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
    app.UseCors(options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
}

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();