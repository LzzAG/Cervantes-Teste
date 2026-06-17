using CoworkingAgendamento.Models;
using Npgsql;

namespace CoworkingAgendamento.Data;

public class LogRepositorio
{
    public List<LogOperacao> Listar()
    {
        var lista = new List<LogOperacao>();
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand(
            "SELECT id, nome_tabela, tipo_operacao, data_hora FROM log_operacao ORDER BY data_hora DESC", conexao);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new LogOperacao
            {
                Id = reader.GetInt32(0),
                NomeTabela = reader.GetString(1),
                TipoOperacao = reader.GetString(2),
                DataHora = reader.GetDateTime(3)
            });
        }
        return lista;
    }
}
