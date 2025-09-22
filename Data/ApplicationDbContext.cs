using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Models;

namespace RotaVerdeAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet para AlunoModel
        public DbSet<AlunoModel> Alunos { get; set; }

        // DbSet para TurmaModel
        public DbSet<TurmaModel> Turmas { get; set; }

        // DbSet para DesempenhoModel
        public DbSet<DesempenhoModel> Desempenhos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações adicionais (se necessário)
            modelBuilder.Entity<AlunoModel>().HasKey(a => a.Id);

            // Configurações adicionais para TurmaModel
            modelBuilder.Entity<TurmaModel>().HasKey(t => t.Id);
        }
    }
}