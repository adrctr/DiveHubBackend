
using System.Text;
using DiveHub.Application.Interfaces;
using DiveHub.Application.Mapping;
using DiveHub.Application.Services;
using DiveHub.Core.Interfaces;
using DiveHub.Infrastructure.Extensions;
using DiveHub.Infrastructure.Persistence;
using DiveHub.Infrastructure.repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

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

builder.Services.AddDatabaseInitialization("Data Source=DiveHubDB.db");
#endregion

#region services
// Ajouter les services de l'application
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDiveService, DiveService>();
#endregion


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // ← Point essentiel : Authority pour JWKS
        options.Authority = "https://mgdklegnrjrzbnomclfs.supabase.co/auth/v1";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = "https://mgdklegnrjrzbnomclfs.supabase.co",

            ValidateAudience = false, // ← désactivé car Supabase n’envoie pas d'audience
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true
        };

        // ← Permet les détails d'erreurs (utile pour debug)
        options.IncludeErrorDetails = true;
    });

builder.Services
    .AddAuthorization();

#region AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
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
    app.UseCors(options => options.WithOrigins("http://localhost:5228").AllowAnyMethod().AllowAnyHeader());
}

app.UseCors("AllowSpecificOrigins");

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();