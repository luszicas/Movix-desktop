using Movix.Application.Filters;
using Movix.Domain.Entities;

namespace Movix.Application.Abstractions.Repositories;

public interface IFilmeRepository
{
	// Métodos de Leitura
	Task<(IReadOnlyList<Filme> Items, int Total)> SearchAsync(FilmeFilter filter, CancellationToken ct = default);
	Task<Filme?> GetByIdAsync(int id, CancellationToken ct = default);

	// Métodos de Escrita
	Task<Filme> AddAsync(Filme filme);

	// ✅ MÉTODOS NOVOS QUE FALTAVAM:
	Task UpdateAsync(Filme filme);
	Task DeleteAsync(Filme filme);
}