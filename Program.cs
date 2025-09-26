using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // Substitua pelo domínio correto
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Permite envio de cookies e credenciais
    });
});

builder.Services.AddControllers();

// Configurar o banco de dados (use SQLite como exemplo)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar o Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
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

app.UseCors("AllowSpecificOrigin"); // Use a política de CORS atualizada

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
