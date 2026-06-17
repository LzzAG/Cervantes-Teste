using CoworkingAgendamento.Models;
using Npgsql;

namespace CoworkingAgendamento.Data;

public class AgendamentoRepositorio
{
    public List<Agendamento> Listar()
    {
        var lista = new List<Agendamento>();
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand(
            @"SELECT a.id, a.sala_id, s.nome, a.data_inicio, a.data_fim
              FROM agendamento a
              JOIN sala s ON s.id = a.sala_id
              ORDER BY a.data_inicio", conexao);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Agendamento
            {
                Id = reader.GetInt32(0),
                SalaId = reader.GetInt32(1),
                SalaNome = reader.GetString(2),
                DataInicio = reader.GetDateTime(3),
                DataFim = reader.GetDateTime(4)
            });
        }
        return lista;
    }

    public void Inserir(int salaId, DateTime inicio, DateTime fim)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand(
            "INSERT INTO agendamento (sala_id, data_inicio, data_fim) VALUES (@sala, @inicio, @fim)", conexao);
        cmd.Parameters.AddWithValue("sala", salaId);
        cmd.Parameters.AddWithValue("inicio", inicio);
        cmd.Parameters.AddWithValue("fim", fim);
        cmd.ExecuteNonQuery();
    }

    public void Atualizar(int id, int salaId, DateTime inicio, DateTime fim)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand(
            "UPDATE agendamento SET sala_id = @sala, data_inicio = @inicio, data_fim = @fim WHERE id = @id", conexao);
        cmd.Parameters.AddWithValue("sala", salaId);
        cmd.Parameters.AddWithValue("inicio", inicio);
        cmd.Parameters.AddWithValue("fim", fim);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand("DELETE FROM agendamento WHERE id = @id", conexao);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }
}
