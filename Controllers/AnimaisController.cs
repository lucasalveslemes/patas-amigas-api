using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatasAmigas.API.Data;
using PatasAmigas.API.Models;
using PatasAmigas.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace PatasAmigas.API.Controllers
{
    [ApiController] //Diz ao .NET que essa classe é controller de API
    [Route("api/[controller]")] //Define a URL base -> [controller] vira o nome da classe sem sufixo Controller
    public class AnimaisController : ControllerBase
    {
        private readonly AppDbContext _context; //Injeção dde dependencia. Por isso configuramos builder.Services.AddDbContext
        private readonly Cloudinary _cloudinary; //Cloudinary adicionado na injeção de dependência 

        public AnimaisController(AppDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        //GET : api/animais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimais()
        {
            return await _context.Animais.ToListAsync();
        }

        //GET: api/animais/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animais.FindAsync(id);
            if (animal == null) return NotFound("Animal não encontrado");
            return animal;
        }

        //GET: api/animais/disponiveis
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetDisponivies()
        {
            return await _context.Animais
                .Where(a => a.Disponivel == true)
                .ToListAsync();
        }

        //POST: api/animais
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Animal>> PostAnimal(AnimalDTO dto)
        {
            var animal = new Animal
            {
                Nome = dto.Nome,
                Especie = dto.Especie,
                Porte = dto.Porte,
                Descricao = dto.Descricao,
                FotoUrl = dto.FotoUrl,
                Cidade = dto.Cidade,
                Disponivel = dto.Disponivel,
                DataCadastro = DateTime.Now
            };

            _context.Animais.Add(animal);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnimal), new { id = animal.Id }, animal);
        }
        //PUT: api/animais/1
        [HttpPut]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> PutAnimal(int id, AnimalDTO dto)
        {
            var animal = await _context.Animais.FindAsync(id);

            if (animal == null) return NotFound("Animal não encontrado.");

            animal.Nome = dto.Nome;
            animal.Especie = dto.Especie;
            animal.Porte = dto.Porte;
            animal.Descricao = dto.Descricao;
            animal.FotoUrl = dto.FotoUrl;
            animal.Cidade = dto.Cidade;
            animal.Disponivel = dto.Disponivel;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/animais/1/foto
        [Authorize]
        [HttpPost("{id}/foto")]
        public async Task<ActionResult> UploadFoto(int id, IFormFile foto)
        {
            var animal = await _context.Animais.FindAsync(id);
            if (animal == null)
                return NotFound("Animal não encontrado.");

            if (foto == null || foto.Length == 0)
                return BadRequest("Nenhuma imagem enviada.");

            // Valida o tipo do arquivo
            var tiposPermitidos = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!tiposPermitidos.Contains(foto.ContentType))
                return BadRequest("Formato inválido. Use JPG, PNG ou WEBP.");

            // Valida o tamanho — máximo 5MB
            if (foto.Length > 5 * 1024 * 1024)
                return BadRequest("Imagem muito grande. Máximo 5MB.");

            using var stream = foto.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(foto.FileName, stream),
                Folder = "patas-amigas",
                PublicId = $"animal-{id}",
                Overwrite = true,
                Transformation = new Transformation()
                    .Width(800).Height(600)
                    .Crop("fill").Gravity("auto")
                    .Quality("auto").FetchFormat("auto")
            };

            var resultado = await _cloudinary.UploadAsync(uploadParams);

            if (resultado.Error != null)
                return BadRequest($"Erro no upload: {resultado.Error.Message}");

            animal.FotoUrl = resultado.SecureUrl.ToString();
            await _context.SaveChangesAsync();

            return Ok(new { fotoUrl = animal.FotoUrl });
        }

        // DELETE: api/animais/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animais.FindAsync(id);

            if (animal == null)
                return NotFound("Animal não encontrado.");

            _context.Animais.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/animais/1/adotado
        [Authorize]
        [HttpPatch("{id}/adotado")]
        public async Task<ActionResult> MarcarComoAdotado(int id)
        {
            var animal = await _context.Animais.FindAsync(id);

            if (animal == null)
                return NotFound("Animal não encontrado.");

            animal.Disponivel = false;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
