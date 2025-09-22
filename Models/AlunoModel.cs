namespace RotaVerdeAPI.Models
{
    public class AlunoModel
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public int Idade { get; set; }
        public int? TurmaId { get; set; } // Relacionamento com a turma
    }
}
