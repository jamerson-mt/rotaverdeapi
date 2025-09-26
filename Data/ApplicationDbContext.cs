using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RotaVerdeAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        

        // DbSet para TurmaModel
        public DbSet<TurmaModel> Turmas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configurações adicionais podem ser feitas aqui
        }
    }
}