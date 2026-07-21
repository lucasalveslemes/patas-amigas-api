namespace PatasAmigas.API.DTOs
{
    public class DoacaoDTO
    {
        public string NomeDoador { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string FormaPagamento { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}
