using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data; // Namespace do DbContext
using RotaVerdeAPI.Models; // Namespace do modelo atividade
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AtividadeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AtividadeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/atividade
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AtividadeModel>>> GetAll()
        {
            var atividades = await _context.atividades.ToListAsync();
            return Ok(atividades);
        }

        // GET: api/atividade/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AtividadeModel>> GetById(int id)
        {
            var atividade = await _context.atividades.FindAsync(id);
            if (atividade == null)
            {
                return NotFound();
            }
            return Ok(atividade);
        }

        // POST: api/atividade
        [HttpPost]
        public async Task<ActionResult<AtividadeModel>> Create([FromBody] AtividadeModel atividade)
        {
            if (!ModelState.IsValid) // validação do modelo
            {
                return BadRequest(ModelState); // retorna 400 Bad Request com os erros de validação
            }

            _context.atividades.Add(atividade); // adiciona a nova atividade ao contexto
            await _context.SaveChangesAsync(); // salva as mudanças no banco de dados

            return CreatedAtAction(nameof(GetById), new { id = atividade. Id }, atividade); // retorna 201 Created com a localização da nova atividade
        }

    }
}
