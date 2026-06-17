namespace CoworkingAgendamento.Models;

public class LogOperacao
{
    public int Id { get; set; }
    public string NomeTabela { get; set; } = string.Empty;
    public string TipoOperacao { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
}
