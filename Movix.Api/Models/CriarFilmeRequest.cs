using Microsoft.AspNetCore.Http; 

namespace Movix.Api.Models 
{
    public class CriarFilmeRequest
    {
        public string Titulo { get; set; }
        public string? Sinopse { get; set; }
        public int AnoLancamento { get; set; }
        public int GeneroId { get; set; }
        public int ClassificacaoId { get; set; }
        public string? UrlTrailer { get; set; }

        // Link da internet
        public string? ImagemCapaUrl { get; set; }

        // Arquivo do computador
        public IFormFile? ArquivoCapa { get; set; }
    }
}