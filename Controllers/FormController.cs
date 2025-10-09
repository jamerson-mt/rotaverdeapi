using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RotaVerdeAPI.Data; // Namespace do DbContext
using RotaVerdeAPI.Models; // Namespace do modelo FormModel
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FormController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/form
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FormModel>>> GetAll()
        {
            var forms = await _context.forms.ToListAsync();
            return Ok(forms);
        }

        // GET: api/form/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<FormModel>> GetById(int id)
        {
            var form = await _context.forms.FindAsync(id);
            if (form == null)
            {
                return NotFound();
            }
            return Ok(form);
        }

        // POST: api/form
        [HttpPost]
        public async Task<ActionResult<FormModel>> Create([FromBody] FormModel form)
        {
            if (!ModelState.IsValid) // validação do modelo
            {
                return BadRequest(new
                {
                    Message = "Erro de validação nos dados enviados.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                }); // retorna 400 Bad Request com os erros detalhados
            }

            try
            {
                _context.forms.Add(form); // adiciona o novo formulário ao contexto
                await _context.SaveChangesAsync(); // salva as mudanças no banco de dados

                return CreatedAtAction(nameof(GetById), new { id = form.Id }, form); // retorna 201 Created com a localização do novo formulário
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new
                {
                    Message = "Erro ao salvar os dados no banco de dados.",
                    Details = ex.Message
                }); // retorna 500 Internal Server Error com detalhes
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Ocorreu um erro inesperado.",
                    Details = ex.Message
                }); // retorna 500 Internal Server Error com detalhes
            }
        }
    }
}
