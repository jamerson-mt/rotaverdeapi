using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data;
using RotaVerdeAPI.Models;

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DesempenhoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DesempenhoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Desempenho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesempenhoModel>>> GetAll()
        {
            var desempenhos = await _context.Desempenhos.Include(d => d.Aluno).ToListAsync();
            return Ok(desempenhos);
        }

        // GET: api/Desempenho/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DesempenhoModel>> GetById(int id)
        {
            var desempenho = await _context.Desempenhos.Include(d => d.Aluno).SingleOrDefaultAsync(d => d.Id == id);
            if (desempenho == null)
            {
                return NotFound();
            }
            return Ok(desempenho);
        }

        // GET: api/Desempenho/atividade/{atividadeId}/aluno/{alunoId}
        [HttpGet("atividade/{atividadeId}/aluno/{alunoId}")]
        public async Task<ActionResult<DesempenhoModel>> GetByAtividadeAndAluno(string atividadeId, int alunoId)
        {
            var desempenho = await _context.Desempenhos
                .Include(d => d.Aluno)
                .SingleOrDefaultAsync(d => d.AtividadeId == atividadeId && d.AlunoId == alunoId);

            if (desempenho == null)
            {
                return NotFound(new { message = "Desempenho não encontrado para a atividade e aluno especificados." });
            }

            return Ok(desempenho);
        }

        // POST: api/Desempenho
        [HttpPost]
        public async Task<ActionResult<DesempenhoModel>> Create([FromBody] DesempenhoModel desempenho)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _context.Desempenhos.Add(desempenho);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetById), new { id = desempenho.Id }, desempenho);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new { message = "Erro ao salvar o desempenho no banco de dados.", details = ex.Message });
            }
        }

        // DELETE: api/Desempenho/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var desempenho = await _context.Desempenhos.FindAsync(id);
            if (desempenho == null)
            {
                return NotFound();
            }

            _context.Desempenhos.Remove(desempenho);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
