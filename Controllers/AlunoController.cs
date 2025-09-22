using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data; // Namespace do DbContext
using RotaVerdeAPI.Models; // Namespace do modelo Aluno

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AlunoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Aluno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoModel>>> GetAll()
        {
            var alunos = await _context.Alunos.ToListAsync();
            return Ok(alunos);
        }

        // GET: api/Aluno/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoModel>> GetById(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return Ok(aluno);
        }

        // POST: api/Aluno
        [HttpPost]
        public async Task<ActionResult<AlunoModel>> Create([FromBody] AlunoModel aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = aluno.Id }, aluno);
        }

        // PUT: api/Aluno/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] AlunoModel aluno)
        {
            if (id != aluno.Id)
            {
                return BadRequest();
            }

            _context.Entry(aluno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Alunos.Any(a => a.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Aluno/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Verifica se o aluno existe no banco de dados
            var aluno = await _context.Alunos.SingleOrDefaultAsync(a => a.Id == id);
            if (aluno == null)
            {
                return NotFound(new { Message = "Aluno não encontrado." });
            }

            // Remove o aluno
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
