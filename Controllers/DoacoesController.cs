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
    public class DoacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DoacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/doacoes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Doacao>>> GetDoacoes()
        {
            return await _context.Doacoes
                .OrderByDescending(d => d.DataDoacao)
                .ToListAsync();
        }

        // GET: api/doacoes/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Doacao>> GetDoacao(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);

            if (doacao == null)
                return NotFound("Doação não encontrada.");

            return doacao;
        }

        // GET: api/doacoes/total
        [HttpGet("total")]
        public async Task<ActionResult<object>> GetTotal()
        {
            var total = await _context.Doacoes
                .Where(d => d.Status == "Confirmada")
                .SumAsync(d => d.Valor);

            var quantidade = await _context.Doacoes
                .Where(d => d.Status == "Confirmada")
                .CountAsync();

            return Ok(new
            {
                TotalArrecadado = total,
                QuantidadeDoacoes = quantidade
            });
        }

        // POST: api/doacoes
        [HttpPost]
        public async Task<ActionResult<Doacao>> PostDoacao(DoacaoDTO dto)
        {
            if (dto.Valor <= 0)
                return BadRequest("O valor da doação deve ser maior que zero.");

            var formasValidas = new[] { "PIX", "Transferência", "Dinheiro" };
            if (!formasValidas.Contains(dto.FormaPagamento))
                return BadRequest("Forma de pagamento inválida. Use: PIX, Transferência ou Dinheiro.");

            var doacao = new Doacao
            {
                NomeDoador = dto.NomeDoador,
                Email = dto.Email,
                Telefone = dto.Telefone,
                Valor = dto.Valor,
                FormaPagamento = dto.FormaPagamento,
                Mensagem = dto.Mensagem,
                DataDoacao = DateTime.Now,
                Status = "Pendente"
            };

            _context.Doacoes.Add(doacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDoacao),
                new { id = doacao.Id }, doacao);
        }

        // PATCH: api/doacoes/1/confirmar
        [Authorize]
        [HttpPatch("{id}/confirmar")]
        public async Task<ActionResult> ConfirmarDoacao(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);

            if (doacao == null)
                return NotFound("Doação não encontrada.");

            if (doacao.Status == "Confirmada")
                return BadRequest("Esta doação já foi confirmada.");

            doacao.Status = "Confirmada";
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/doacoes/1/cancelar
        [Authorize]
        [HttpPatch("{id}/cancelar")]
        public async Task<ActionResult> CancelarDoacao(int id)
        {
            var doacao = await _context.Doacoes.FindAsync(id);

            if (doacao == null)
                return NotFound("Doação não encontrada.");

            if (doacao.Status == "Confirmada")
                return BadRequest("Não é possível cancelar uma doação já confirmada.");

            doacao.Status = "Cancelada";
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}