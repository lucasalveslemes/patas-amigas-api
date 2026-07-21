namespace PatasAmigas.API.DTOs
{
    public class SolicitacaoDTO
    {
        public string NomeSolicitante { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string MotivoAdocao { get; set; } = string.Empty;
        public string TemOutrosAnimais { get; set; } = string.Empty;
        public string TipoResidencia { get; set; } = string.Empty;
        public int AnimalId { get; set; }
    }
}