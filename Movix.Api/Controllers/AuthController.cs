using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Movix.Infrastructure.Entities; // <--- Verifique se o nome da sua classe de usuário é ApplicationUser mesmo
using Movix.Application.DTOs;

namespace Movix.Api.Controllers
{
    [ApiController]
    [Route("api/auth")] // <--- ISSO AQUI CRIA O ENDEREÇO "api/auth"
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost("login")] // <--- ISSO AQUI COMPLETA PARA "api/auth/login"
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Senha, false);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Logado com sucesso" });
                }
            }
            return Unauthorized("Usuário ou senha inválidos");
        }


		[HttpPost("registrar")] // <--- Endereço: api/auth/registrar
		public async Task<IActionResult> Registrar([FromBody] RegistroDto model)
		{
			var user = new ApplicationUser
			{
				UserName = model.Email,
				Email = model.Email,
				IsActive = true,
				CreatedAt = DateTime.UtcNow
			};

			var result = await _userManager.CreateAsync(user, model.Senha);

			if (result.Succeeded)
			{
				// Por padrão, todo mundo que se cadastra sozinho entra como "Usuario"
				//await _userManager.AddToRoleAsync(user, "Usuario");
				return Ok(new { message = "Conta criada com sucesso!" });
			}

			return BadRequest(result.Errors.Select(e => e.Description));
		}

		// DTO para o Registro
		public class RegistroDto
		{
			public string Email { get; set; } = string.Empty;
			public string Senha { get; set; } = string.Empty;
		}
	}


}

