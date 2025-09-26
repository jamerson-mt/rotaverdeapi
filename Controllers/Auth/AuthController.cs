using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RotaVerdeAPI.Models;
using RotaVerdeAPI.Models.Auth; // Atualizado para o namespace correto
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RotaVerdeAPI.Controllers.Auth
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize(Roles = "professor")]
        // POST: api/Auth/Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Todos os campos são obrigatórios." });
            }

            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = "Usuário registrado com sucesso!" });
        }

        // POST: api/Auth/Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { Message = "Usuário e senha são obrigatórios." });
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return Unauthorized(new { Message = "Usuário não encontrado!" });
            }

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);
            if (!result.Succeeded)
            {
                return Unauthorized(new { Message = "Credenciais inválidas." });
            }

            var roles = await _userManager.GetRolesAsync(user); // Obtém as roles do usuário

            return Ok(new 
            { 
                Message = "Login realizado com sucesso!", 
                User = new 
                {
                    user.UserName,
                    user.Email,
                    Roles = roles // Adiciona as roles na resposta
                }
            });
        }
        

        [Authorize]
        // POST: api/Auth/Logout
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync(); // Limpa o cookie de autenticação

            return Ok(new { Message = "Logout realizado com sucesso!" });
        }

        [Authorize(Roles = "professor")]
        // POST: api/Auth/AssignRole
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Role))
            {
                return BadRequest(new { Message = "Usuário e role são obrigatórios." });
            }

            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return NotFound(new { Message = "Usuário não encontrado." });
            }

            if (request.Role != "aluno" && request.Role != "professor")
            {
                return BadRequest(new { Message = "Role inválida. Use 'aluno' ou 'professor'." });
            }

            var result = await _userManager.AddToRoleAsync(user, request.Role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { Message = $"Role '{request.Role}' atribuída ao usuário '{request.Username}' com sucesso!" });
        }
    }
}