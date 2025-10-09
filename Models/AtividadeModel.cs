using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RotaVerdeAPI.Models
{
    public class AtividadeModel
    {
        public int Id { get; set; } // Identificador único da turma
        public required string Nome { get; set; } // Nome da turma
        public required string AlunoId { get; set; }  // ID do aluno do time da atividade
        // propiedade de tempo da atividade 
        public required int Tempo { get; set;}

    }
}
