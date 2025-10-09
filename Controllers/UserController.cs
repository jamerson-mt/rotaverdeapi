using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using RotaVerdeAPI.Models;

namespace RotaVerdeAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/user
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userManager.Users.Select(u => new 
            {
                u.Id,
                u.UserName,
                u.Email
            }).ToList();
            return Ok(users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] Dictionary<string, string> updates)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            if (updates.ContainsKey("UserName"))
            {
                user.UserName = updates["UserName"];
            }
            if (updates.ContainsKey("Email"))
            {
                user.Email = updates["Email"];
            }
            if (updates.ContainsKey("FullName"))
            {
                user.FullName = updates["FullName"];
            }
            if (updates.ContainsKey("Address"))
            {
                user.Address = updates["Address"];
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return NoContent();
        }
    }
}
