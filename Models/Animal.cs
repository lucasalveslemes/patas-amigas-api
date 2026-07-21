namespace PatasAmigas.API.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string Porte { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string FotoUrl { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public bool Disponivel{ get; set; } = true;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
    }
}
