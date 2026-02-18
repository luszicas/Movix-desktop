using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movix.Infrastructure.Entities;

namespace Movix.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsuariosController : ControllerBase
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public UsuariosController(UserManager<ApplicationUser> userManager)
		{
			_userManager = userManager;
		}

		// ==========================================
		// 1. GET: api/usuarios (LISTA NA TABELA)
		// ==========================================
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				var usuarios = await _userManager.Users.ToListAsync();
				var lista = new List<UsuarioDto>();

				foreach (var user in usuarios)
				{
					var roles = await _userManager.GetRolesAsync(user);
					lista.Add(new UsuarioDto
					{
						Id = user.Id.ToString(),
						Username = user.UserName ?? string.Empty,
						Email = user.Email ?? string.Empty,
						Perfil = roles.FirstOrDefault() ?? "Sem Perfil",
						IsActive = user.IsActive
					});
				}

				return Ok(lista);
			}
			catch (Exception ex)
			{
				return BadRequest("Erro ao listar usuários: " + ex.Message);
			}
		}

		// ==========================================
		// 2. POST: api/usuarios (CADASTRO)
		// ==========================================
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] CriarUsuarioRequest model)
		{
			try
			{
				var user = new ApplicationUser
				{
					UserName = model.Username,
					Email = model.Email,
					EmailConfirmed = true,
					IsActive = true,
					CreatedAt = DateTime.UtcNow
				};

				var result = await _userManager.CreateAsync(user, model.Senha);

				if (result.Succeeded)
				{
					if (!string.IsNullOrEmpty(model.Perfil))
					{
						await _userManager.AddToRoleAsync(user, model.Perfil);
					}

					return Ok(new { message = "Usuário criado com sucesso!" });
				}

				return BadRequest(result.Errors.Select(e => e.Description));
			}
			catch (Exception ex)
			{
				return BadRequest("Erro ao salvar usuário: " + ex.Message);
			}
		}

		// ==========================================
		// 3. DELETE: api/usuarios/{id} (EXCLUSÃO)
		// ==========================================
		// Agora o DELETE na verdade DESATIVA ou REATIVA o usuário
		[HttpDelete("{id}")]
		public async Task<IActionResult> DesativarOuAtivar(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound("Usuário não encontrado.");

			// Inverte o status: se tá ativo, desativa. Se tá desativado, ativa.
			user.IsActive = !user.IsActive;

			var result = await _userManager.UpdateAsync(user);
			if (result.Succeeded)
			{
				return Ok(new { message = user.IsActive ? "Usuário ativado!" : "Usuário desativado!" });
			}

			return BadRequest("Erro ao mudar status do usuário.");
		}


		// GET: api/usuarios/{id}
		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			var roles = await _userManager.GetRolesAsync(user);

			return Ok(new UsuarioDto
			{
				// AQUI ESTÁ A CORREÇÃO: .ToString() para converter Guid em string
				Id = user.Id.ToString(),
				Username = user.UserName ?? string.Empty,
				Email = user.Email ?? string.Empty,
				Perfil = roles.FirstOrDefault() ?? "Usuário",
				IsActive = user.IsActive
			});
		}

		// PUT: api/usuarios/{id} (PARA SALVAR A EDIÇÃO)
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(string id, [FromBody] CriarUsuarioRequest model)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			// Atualiza os dados básicos
			user.Email = model.Email;
			user.UserName = model.Username;

			// Se o usuário digitou uma senha nova, a gente troca
			if (!string.IsNullOrWhiteSpace(model.Senha))
			{
				var token = await _userManager.GeneratePasswordResetTokenAsync(user);
				await _userManager.ResetPasswordAsync(user, token, model.Senha);
			}

			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded)
			{
				// Atualiza o perfil (Remove o antigo e põe o novo)
				var currentRoles = await _userManager.GetRolesAsync(user);
				await _userManager.RemoveFromRolesAsync(user, currentRoles);
				await _userManager.AddToRoleAsync(user, model.Perfil);

				return Ok(new { message = "Usuário atualizado com sucesso!" });
			}

			return BadRequest(result.Errors.Select(e => e.Description));
		}
		// --- DTOs (Data Transfer Objects) ---
		public class CriarUsuarioRequest
		{
			public string Email { get; set; } = string.Empty;
			public string Username { get; set; } = string.Empty;
			public string Senha { get; set; } = string.Empty;
			public string Perfil { get; set; } = string.Empty;
		}

		public class UsuarioDto
		{
			public string Id { get; set; } = string.Empty;
			public string Username { get; set; } = string.Empty;
			public string Email { get; set; } = string.Empty;
			public string Perfil { get; set; } = string.Empty;
			public bool IsActive { get; set; }
		}
	}
}