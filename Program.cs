using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PulsePoint.Data;
using PulsePoint.Models;
using PulsePoint.Repositories;
using PulsePoint.Services;
using System.Text;

// L�s milj�variabler fr�n .env
LoadEnvFile();

var builder = WebApplication.CreateBuilder(args);

// Registrera controllers f�r att hantera inkommande HTTP-anrop
builder.Services.AddControllers();

// Registrera serviceklasser f�r injektion
builder.Services.AddScoped<IHealthEntryService, HealthEntryService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWorkplaceService, WorkplaceService>();

// Registrera repository-klasser f�r databas�tkomst (som anv�nds av services)
builder.Services.AddScoped<IHealthEntryRepository, HealthEntryRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IWorkplaceRepository, WorkplaceRepository>();

// Konfigurera databaskoppling mot MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<PulsePointDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Registrera Identity med anpassad User-modell och roller
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
    .AddEntityFrameworkStores<PulsePointDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Registrera JWT-tj�nst f�r token-generering 
builder.Services.AddScoped<JwtService>();

// Konfigurera JWT-autentisering som standard
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer("Bearer", options =>
{
    var keyString = builder.Configuration["Jwt:Key"]
        ?? throw new InvalidOperationException("Jwt:Key saknas i appsettings.json");

    options.RequireHttpsMetadata = false; // Till�t HTTP i utveckling

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Kontrollera att token kommer fr�n r�tt utf�rdare
        ValidateAudience = true, // Kontrollra att token �r avsedd f�r r�tt mottagare
        ValidateLifetime = true, // Kontrollera att token inte har g�tt ut
        ValidateIssuerSigningKey = true, // Kontrollera signaturen
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString))
    };
});

// Aktivera auktorisering ( [Authorize] attribut )
builder.Services.AddAuthorization();

var app = builder.Build();

// L�s konfiguration fr�n JSON och milj�variabler
builder.Configuration
    .SetBasePath(app.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

// Middleware f�r autentisering
app.UseAuthentication();
// Middleware f�r auktorisering
app.UseAuthorization();

// Mappa alla controller-endpoints
app.MapControllers();

// Se till att roller (admin, manager och user) finns i db
await SeedRoles(app);

app.Run();

// Funktion f�r att skapa roller vid uppstart om de saknas
static async Task SeedRoles(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

    string[] roles = { "admin", "user", "manager" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<int>(role));
        }
    }
}

// L�s env-fil manuellt
void LoadEnvFile(string path = ".env")
{
    if (!File.Exists(path)) return;

    foreach (var line in File.ReadAllLines(path))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

        var parts = line.Split('=', 2);
        if (parts.Length != 2) continue;

        var key = parts[0].Trim();
        var value = parts[1].Trim().Trim('"');

        Environment.SetEnvironmentVariable(key, value);
    }
}