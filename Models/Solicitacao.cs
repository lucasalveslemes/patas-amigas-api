namespace PatasAmigas.API.Models
{
    public class Solicitacao
    {
        public int Id { get; set; }

        // Dados do solicitante
        public string NomeSolicitante { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string MotivoAdocao { get; set; } = string.Empty;
        public string TemOutrosAnimais { get; set; } = string.Empty;
        public string TipoResidencia { get; set; } = string.Empty;

        // Relacionamento com Animal
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; } // Navigation Property

        public DateTime DataSolicitacao { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pendente"; // Pendente, Aprovada, Recusada
    }
}