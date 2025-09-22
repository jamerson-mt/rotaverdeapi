using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RotaVerdeAPI.Models
{
    public class DesempenhoModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int AlunoId { get; set; }

        public required string AtividadeId { get; set; } // Descrição da atividade realizada

        [ForeignKey("AlunoId")]
        public AlunoModel? Aluno { get; set; }

        [Required]
        public TimeSpan TempoGasto { get; set; } // Tempo gasto na atividade em horas, minutos e segundos
    }
}
