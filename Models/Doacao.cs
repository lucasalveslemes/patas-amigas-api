namespace PatasAmigas.API.Models
{
    public class Doacao
    {
        public int Id { get; set; }
        public string NomeDoador { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string FormaPagamento { get; set; } = string.Empty; // PIX, Transferência, Dinheiro
        public string Mensagem { get; set; } = string.Empty;
        public DateTime DataDoacao { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Pendente"; // Pendente, Confirmada, Cancelada
    }
}