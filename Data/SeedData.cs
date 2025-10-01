using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Models;

namespace RotaVerdeAPI.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                // Aplicar as migrations automaticamente
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roles = new[] { "aluno", "professor" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).Result)
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).Wait();
                    }
                }

                // Criar usuário padrão com a role "professor"
                var defaultProfessorEmail = "professor@rotaverde.com";
                var defaultProfessorPassword = "Professor123!";
                if (userManager.FindByEmailAsync(defaultProfessorEmail).Result == null)
                {
                    var professorUser = new ApplicationUser
                    {
                        UserName = defaultProfessorEmail,
                        Email = defaultProfessorEmail,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(professorUser, defaultProfessorPassword).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(professorUser, "professor").Wait();
                    }
                }
            }
        }
    }
}
