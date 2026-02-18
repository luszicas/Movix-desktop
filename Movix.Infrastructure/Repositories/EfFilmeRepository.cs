using Microsoft.EntityFrameworkCore;                       // EF Core
using Movix.Application.Abstractions.Repositories;   // Contrato IFilmeRepository
using Movix.Application.Filters;                     // Record FilmeFilter
using Movix.Domain.Entities;                         // Entidade Filme
using Movix.Infrastructure.Persistence;                // AppDbContext

namespace Movix.Infrastructure.Repositories;

/// <summary>
/// Implementação EF Core do repositório de filmes.
/// </summary>
public sealed class EfFilmeRepository : IFilmeRepository
{
	private readonly AppDbContext _ctx;

	public EfFilmeRepository(AppDbContext ctx) => _ctx = ctx;

	public async Task<(IReadOnlyList<Filme> Items, int Total)> SearchAsync(
		FilmeFilter filter, CancellationToken ct = default)
	{
		// Saneamento de paginação
		var page = filter.Page < 1 ? 1 : filter.Page; // ✅ Arrumei aqui (faltava o ponto e vírgula!)
		var pageSize = filter.PageSize;

		if (pageSize > 2000) pageSize = 2000;

		var q = _ctx.Filmes
			.AsNoTracking()
			.Include(f => f.Genero)
			.Include(f => f.Classificacao)
			.AsQueryable();

		// Filtros
		if (filter.GeneroId is int gid)
			q = q.Where(f => f.GeneroId == gid);

		if (filter.ClassificacaoId is int cid)
			q = q.Where(f => f.ClassificacaoId == cid);

		if (filter.Ano is int ano)
			q = q.Where(f => f.Ano == ano);

		if (!string.IsNullOrWhiteSpace(filter.Q))
		{
			var like = $"%{filter.Q.Trim()}%";
			q = q.Where(f =>
				EF.Functions.Like(f.Titulo, like) ||
				(f.Sinopse != null && EF.Functions.Like(f.Sinopse, like)));
		}

		// Ordenação
		q = (filter.SortBy?.ToLowerInvariant()) switch
		{
			"titulo" => filter.Desc ? q.OrderByDescending(f => f.Titulo) : q.OrderBy(f => f.Titulo),
			"ano" => filter.Desc ? q.OrderByDescending(f => f.Ano) : q.OrderBy(f => f.Ano),
			"createdat" => filter.Desc ? q.OrderByDescending(f => f.CreatedAt) : q.OrderBy(f => f.CreatedAt),
			_ => q.OrderByDescending(f => f.CreatedAt)
		};

		var total = await q.CountAsync(ct);

		// Se pageSize == 0 -> traz todos os itens
		if (filter.PageSize == 0)
		{
			var allItems = await q.ToListAsync(ct);
			return (allItems, total);
		}

		// Paginação normal
		var effectivePageSize = pageSize < 1 ? 12 : pageSize;
		var items = await q
			.Skip((page - 1) * effectivePageSize)
			.Take(effectivePageSize)
			.ToListAsync(ct);

		return (items, total);
	}

	public Task<Filme?> GetByIdAsync(int id, CancellationToken ct = default) =>
		_ctx.Filmes
			.AsNoTracking()
			.Include(f => f.Genero)
			.Include(f => f.Classificacao)
			.FirstOrDefaultAsync(f => f.Id == id, ct);

	public async Task<Filme> AddAsync(Filme filme)
	{
		await _ctx.Filmes.AddAsync(filme); // Prepara para inserir
		await _ctx.SaveChangesAsync();     // Executa o INSERT no Banco
		return filme;                      // Retorna o filme (agora com ID gerado)
	}

	// =======================================================
	// ✅ MÉTODOS NOVOS ADICIONADOS AQUI (UPDATE E DELETE):
	// =======================================================

	public async Task UpdateAsync(Filme filme)
	{
		_ctx.Filmes.Update(filme);
		await _ctx.SaveChangesAsync();
	}

	public async Task DeleteAsync(Filme filme)
	{
		_ctx.Filmes.Remove(filme);
		await _ctx.SaveChangesAsync();
	}
}