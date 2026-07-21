using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatasAmigas.API.Data;
using PatasAmigas.API.DTOs;
using PatasAmigas.API.Models;

namespace PatasAmigas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SolicitacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/solicitacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Solicitacao>>> GetSolicitacoes()
        {
            return await _context.Solicitacoes
                .Include(s => s.Animal) // Traz os dados do animal junto
                .ToListAsync();
        }

        // GET: api/solicitacoes/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Solicitacao>> GetSolicitacao(int id)
        {
            var solicitacao = await _context.Solicitacoes
                .Include(s => s.Animal)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (solicitacao == null)
                return NotFound("Solicitação não encontrada.");

            return solicitacao;
        }

        // POST: api/solicitacoes
        [HttpPost]
        public async Task<ActionResult<Solicitacao>> PostSolicitacao(SolicitacaoDTO dto)
        {
            // Verifica se o animal existe
            var animal = await _context.Animais.FindAsync(dto.AnimalId);
            if (animal == null)
                return NotFound("Animal não encontrado.");

            // Verifica se o animal está disponível
            if (!animal.Disponivel)
                return BadRequest("Este animal já foi adotado.");

            var solicitacao = new Solicitacao
            {
                NomeSolicitante = dto.NomeSolicitante,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Endereco = dto.Endereco,
                MotivoAdocao = dto.MotivoAdocao,
                TemOutrosAnimais = dto.TemOutrosAnimais,
                TipoResidencia = dto.TipoResidencia,
                AnimalId = dto.AnimalId,
                DataSolicitacao = DateTime.Now,
                Status = "Pendente"
            };

            _context.Solicitacoes.Add(solicitacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSolicitacao),
                new { id = solicitacao.Id }, solicitacao);
        }

        // PATCH: api/solicitacoes/1/status
        [Authorize]
        [HttpPatch("{id}/status")]
        public async Task<ActionResult> AtualizarStatus(int id, [FromBody] string novoStatus)
        {
            var statusValidos = new[] { "Pendente", "Aprovada", "Recusada" };
            if (!statusValidos.Contains(novoStatus))
                return BadRequest("Status inválido. Use: Pendente, Aprovada ou Recusada.");

            var solicitacao = await _context.Solicitacoes.FindAsync(id);
            if (solicitacao == null)
                return NotFound("Solicitação não encontrada.");

            solicitacao.Status = novoStatus;

            // Se aprovada, marca o animal como indisponível automaticamente
            if (novoStatus == "Aprovada")
            {
                var animal = await _context.Animais.FindAsync(solicitacao.AnimalId);
                if (animal != null)
                    animal.Disponivel = false;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}