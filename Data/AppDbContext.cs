using Microsoft.EntityFrameworkCore;
using PatasAmigas.API.Models;

namespace PatasAmigas.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Animal> Animais { get; set; }
        public DbSet<Solicitacao> Solicitacoes { get; set; }
        public DbSet<Doacao> Doacoes { get; set; }
        public DbSet<Admin> Admins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cria um admin padrão no banco automaticamente
            modelBuilder.Entity<Admin>().HasData(new Admin
            {
                Id = 1,
                Usuario = "admin",
                // Senha: admin123 (já em hash BCrypt)
                SenhaHash = "$2a$11$uU0/hMqDR/cgBtH7sB7.Xu3BPLUpNtZ1YBvFD3bvX8WFkhLCsJ/pC"
            });
        }
    }
}