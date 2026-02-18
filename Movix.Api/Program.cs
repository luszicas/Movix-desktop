using Microsoft.AspNetCore.Identity;
using Movix.Infrastructure;
using Movix.Infrastructure.Entities;
using Movix.Infrastructure.Persistence;
using Movix.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. Configuração de Serviços (Dependency Injection)
// ==========================================

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Identity (Login e Senha)
builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
{
    options.Password.RequiredLength = 6;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// CORS (Permite que o Desktop e Web acessem a API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ??
            new[] {
                "https://localhost:5001",
                "http://localhost:5000",
                "http://localhost:5200", // Adicionei portas comuns caso mude
                "https://localhost:7100"
            })
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// ==========================================
// 2. Configuração do Pipeline (Middleware)
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- ESSENCIAL PARA AS IMAGENS DO FILME FUNCIONAREM ---
app.UseStaticFiles();
// ------------------------------------------------------

app.UseCors("DevCors");

app.UseAuthentication(); // Garante que o Identity leia o cookie/token
app.UseAuthorization();

// Rota de teste na raiz
app.MapGet("/", () => Results.Ok(new { name = "Movix.Api", status = "ok" }));

app.MapControllers();

// ==========================================
// 3. Inicialização e Seed do Banco
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DatabaseSeeder.EnsureSeededAsync(services);
    }
    catch (Exception ex)
    {
        // Log simples caso o seed falhe (pra não travar tudo silenciosamente)
        Console.WriteLine($"Erro ao rodar Seed: {ex.Message}");
    }
}

app.Run();