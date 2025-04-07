using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PulsePoint.Models;
using PulsePoint.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PulsePointDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Lägg till Identity
builder.Services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<PulsePointDbContext>()
    .AddDefaultTokenProviders();


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
