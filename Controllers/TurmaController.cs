using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data; // Namespace do DbContext
using RotaVerdeAPI.Models; // Namespace do modelo Turma
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TurmaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TurmaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Turma
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TurmaModel>>> GetAll()
        {
            var turmas = await _context.Turmas.ToListAsync();
            return Ok(turmas);
        }

        // GET: api/Turma/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<TurmaModel>> GetById(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }
            return Ok(turma);
        }

        // POST: api/Turma
        [HttpPost]
        public async Task<ActionResult<TurmaModel>> Create([FromBody] TurmaModel turma)
        {
            _context.Turmas.Add(turma);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = turma.Id }, turma);
        }

        // PUT: api/Turma/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] TurmaModel turma)
        {
            if (id != turma.Id)
            {
                return BadRequest();
            }

            _context.Entry(turma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Turmas.Any(t => t.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Turma/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var turma = await _context.Turmas.FindAsync(id);
            if (turma == null)
            {
                return NotFound();
            }

            _context.Turmas.Remove(turma);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
