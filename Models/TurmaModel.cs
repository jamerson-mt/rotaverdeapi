using System.Collections.Generic;

namespace RotaVerdeAPI.Models
{
    public class TurmaModel
    {
        public int Id { get; set; }
        public string ?Nome { get; set; }
        public List<AlunoModel>? Alunos { get; set; } // Relacionamento com alunos
    }
}
