using System.Text.Json;
using Npgsql;

namespace CoworkingAgendamento.Data;

public static class Db
{
    private static readonly string ConnectionString = CarregarConnectionString();

    public static NpgsqlConnection AbrirConexao()
    {
        var conexao = new NpgsqlConnection(ConnectionString);
        conexao.Open();
        return conexao;
    }

    private static string CarregarConnectionString()
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        using var doc = JsonDocument.Parse(File.ReadAllText(caminho));
        return doc.RootElement
            .GetProperty("ConnectionStrings")
            .GetProperty("Postgres")
            .GetString()!;
    }
}
