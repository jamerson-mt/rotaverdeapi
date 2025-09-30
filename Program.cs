using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data;
using Microsoft.AspNetCore.Identity;
using RotaVerdeAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
void ConfigureCors(IServiceCollection services)
{
    services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });

        options.AddPolicy("AllowVercel", policy =>
        {
            policy.WithOrigins("https://rotaverde.vercel.app")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
    });
}

ConfigureCors(builder.Services);

builder.Services.AddControllers();

// Configurar o banco de dados (use SQLite como exemplo)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar o Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();

// Método para criar roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "aluno", "professor" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.UseHttpsRedirection();

app.UseCors("AllowVercel"); // Use a política de CORS atualizada

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
