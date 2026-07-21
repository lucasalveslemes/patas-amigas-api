using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PatasAmigas.API.Data;
using PatasAmigas.API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PatasAmigas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDTO dto)
        {
            var admin = await _context.Admins
                .FirstOrDefaultAsync(a => a.Usuario == dto.Usuario);

            if (admin == null)
                return Unauthorized("Usuário ou senha inválidos.");

            var senhaValida = BCrypt.Net.BCrypt.Verify(dto.Senha, admin.SenhaHash);
            if (!senhaValida)
                return Unauthorized("Usuário ou senha inválidos.");

            var token = GerarToken(admin.Usuario);

            return Ok(new
            {
                token,
                usuario = admin.Usuario,
                expiracao = DateTime.Now.AddHours(
                    int.Parse(_config["Jwt:ExpiracaoHoras"]!))
            });
        }

        private string GerarToken(string usuario)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));

            var credenciais = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(
                    int.Parse(_config["Jwt:ExpiracaoHoras"]!)),
                signingCredentials: credenciais
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}