using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace RotaVerdeAPI.Models
{
    public class TurmaModel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public int AnoLetivo { get; set; }
        public List<IdentityUser>? Usuarios { get; set; } // Relacionamento com usuários usando Identity
    }
}
