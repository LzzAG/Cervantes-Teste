using CoworkingAgendamento.Models;
using Npgsql;

namespace CoworkingAgendamento.Data;

public class SalaRepositorio
{
    public List<Sala> Listar()
    {
        var salas = new List<Sala>();
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand("SELECT id, nome FROM sala ORDER BY nome", conexao);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            salas.Add(new Sala { Id = reader.GetInt32(0), Nome = reader.GetString(1) });
        }
        return salas;
    }

    public void Inserir(string nome)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand("INSERT INTO sala (nome) VALUES (@nome)", conexao);
        cmd.Parameters.AddWithValue("nome", nome);
        cmd.ExecuteNonQuery();
    }

    public void Atualizar(int id, string nome)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand("UPDATE sala SET nome = @nome WHERE id = @id", conexao);
        cmd.Parameters.AddWithValue("nome", nome);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = Db.AbrirConexao();
        using var cmd = new NpgsqlCommand("DELETE FROM sala WHERE id = @id", conexao);
        cmd.Parameters.AddWithValue("id", id);
        cmd.ExecuteNonQuery();
    }
}
