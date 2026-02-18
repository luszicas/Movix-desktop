using Microsoft.AspNetCore.Mvc;
using Movix.Application.Abstractions.Repositories;
using Movix.Application.DTOs;
using Movix.Application.Filters;
using Movix.Application.Services;
using Movix.Api.Models;
using Movix.Domain.Entities;

namespace Movix.Api.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FilmesController : ControllerBase
	{
		private readonly IFilmeQueryService _service;
		private readonly IFilmeRepository _repo;

		public FilmesController(IFilmeQueryService service, IFilmeRepository repo)
		{
			_service = service;
			_repo = repo;
		}

		// GET: api/filmes (Consulta)
		[HttpGet]
		public async Task<ActionResult<PagedResult<FilmeDto>>> Get([FromQuery] FilmeFilter filter, CancellationToken ct)
		{
			var result = await _service.SearchAsync(filter, ct);
			Response.Headers["X-Total-Count"] = result.Total.ToString();
			return Ok(result);
		}

		// GET: api/filmes/{id} (Detalhe)
		[HttpGet("{id:int}")]
		public async Task<ActionResult<FilmeDto>> GetById(int id, CancellationToken ct)
		{
			var f = await _repo.GetByIdAsync(id, ct);
			if (f is null) return NotFound();

			var dto = new FilmeDto(f.Id, f.Titulo, f.Sinopse, f.Ano, f.ImagemCapaUrl,
								   f.GeneroId, f.Genero?.Nome ?? "", f.ClassificacaoId, f.Classificacao?.Nome ?? "", f.UrlTrailer);
			return Ok(dto);
		}

		// POST: api/filmes (Cadastro com Upload)
		[HttpPost]
		public async Task<IActionResult> Post([FromForm] CriarFilmeRequest model)
		{
			try
			{
				// 1. Upload da Imagem (se veio arquivo)
				if (model.ArquivoCapa != null)
				{
					var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
					if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

					var nomeArquivo = $"{Guid.NewGuid()}_{model.ArquivoCapa.FileName}";
					var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

					using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
					{
						await model.ArquivoCapa.CopyToAsync(stream);
					}

					var baseUrl = $"{Request.Scheme}://{Request.Host}";
					model.ImagemCapaUrl = $"{baseUrl}/imagens/{nomeArquivo}";
				}

				// 2. Cria a Entidade
				var novoFilme = new Filme
				{
					Titulo = model.Titulo,
					Sinopse = model.Sinopse,
					Ano = model.AnoLancamento,
					GeneroId = model.GeneroId,
					ClassificacaoId = model.ClassificacaoId,
					UrlTrailer = model.UrlTrailer,
					ImagemCapaUrl = model.ImagemCapaUrl
				};

				// 3. Salva no Banco DE VERDADE
				await _repo.AddAsync(novoFilme);

				return Ok(new { mensagem = "Filme salvo com sucesso!", id = novoFilme.Id, capa = model.ImagemCapaUrl });
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Erro ao salvar: {ex.Message}");
				return BadRequest("Erro ao salvar no banco: " + ex.Message);
			}
		}

		// ==========================================================
		// MÉTODOS NOVOS (EDIÇÃO E EXCLUSÃO)
		// ==========================================================

		// PUT: api/filmes/{id} (Edição)
		[HttpPut("{id:int}")]
		public async Task<IActionResult> Put(int id, [FromForm] CriarFilmeRequest model, CancellationToken ct)
		{
			try
			{
				var filmeExistente = await _repo.GetByIdAsync(id, ct);
				if (filmeExistente == null) return NotFound("Filme não encontrado.");

				// 1. Upload da Nova Imagem (se enviou uma nova capa)
				if (model.ArquivoCapa != null)
				{
					var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagens");
					if (!Directory.Exists(pasta)) Directory.CreateDirectory(pasta);

					var nomeArquivo = $"{Guid.NewGuid()}_{model.ArquivoCapa.FileName}";
					var caminhoCompleto = Path.Combine(pasta, nomeArquivo);

					using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
					{
						await model.ArquivoCapa.CopyToAsync(stream);
					}

					var baseUrl = $"{Request.Scheme}://{Request.Host}";
					filmeExistente.ImagemCapaUrl = $"{baseUrl}/imagens/{nomeArquivo}";
				}
				else if (!string.IsNullOrEmpty(model.ImagemCapaUrl))
				{
					filmeExistente.ImagemCapaUrl = model.ImagemCapaUrl;
				}

				// 2. Atualiza as informações do filme
				filmeExistente.Titulo = model.Titulo;
				filmeExistente.Sinopse = model.Sinopse;
				filmeExistente.Ano = model.AnoLancamento;
				filmeExistente.GeneroId = model.GeneroId;
				filmeExistente.ClassificacaoId = model.ClassificacaoId;
				filmeExistente.UrlTrailer = model.UrlTrailer;

				// 3. Salva as alterações no banco
				// ATENÇÃO: Se o UpdateAsync ficar vermelho, precisamos criar ele no IFilmeRepository!
				await _repo.UpdateAsync(filmeExistente);

				return Ok(new { mensagem = "Filme atualizado com sucesso!" });
			}
			catch (Exception ex)
			{
				return BadRequest("Erro ao atualizar no banco: " + ex.Message);
			}
		}

		// DELETE: api/filmes/{id} (Exclusão)
		[HttpDelete("{id:int}")]
		public async Task<IActionResult> Delete(int id, CancellationToken ct)
		{
			try
			{
				var filme = await _repo.GetByIdAsync(id, ct);
				if (filme == null) return NotFound("Filme não encontrado.");

				// ATENÇÃO: Se o DeleteAsync ficar vermelho, precisamos criar ele no IFilmeRepository!
				await _repo.DeleteAsync(filme);

				return Ok(new { mensagem = "Filme excluído com sucesso!" });
			}
			catch (Exception ex)
			{
				return BadRequest("Erro ao excluir no banco: " + ex.Message);
			}
		}
	}
}