using Npgsql;

namespace CoworkingAgendamento.Data;

public static class ErroBanco
{
    public static string Traduzir(PostgresException ex)
    {
        return ex.ConstraintName switch
        {
            "uq_sala_nome" => "Já existe uma sala com esse nome.",
            "sala_nome_check" => "O nome da sala é obrigatório.",
            "chk_periodo_valido" => "A data/hora final deve ser maior que a data/hora inicial.",
            "uq_sem_sobreposicao" => "Já existe um agendamento para essa sala nesse horário.",
            _ => ex.MessageText
        };
    }
}
