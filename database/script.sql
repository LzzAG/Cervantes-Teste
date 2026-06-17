CREATE EXTENSION IF NOT EXISTS btree_gist;

CREATE TABLE sala (
    id   SERIAL       PRIMARY KEY,
    nome VARCHAR(100) NOT NULL CHECK (length(trim(nome)) > 0)
);

CREATE UNIQUE INDEX uq_sala_nome ON sala (lower(trim(nome)));

CREATE TABLE agendamento (
    id          SERIAL    PRIMARY KEY,
    sala_id     INTEGER   NOT NULL REFERENCES sala (id) ON DELETE CASCADE,
    data_inicio TIMESTAMP NOT NULL,
    data_fim    TIMESTAMP NOT NULL,
    CONSTRAINT chk_periodo_valido CHECK (data_fim > data_inicio),
    CONSTRAINT uq_sem_sobreposicao EXCLUDE USING gist (
        sala_id WITH =,
        tsrange(data_inicio, data_fim) WITH &&
    )
);

CREATE INDEX idx_agendamento_sala ON agendamento (sala_id);

CREATE TABLE log_operacao (
    id            SERIAL      PRIMARY KEY,
    nome_tabela   VARCHAR(63) NOT NULL,
    tipo_operacao VARCHAR(10) NOT NULL CHECK (tipo_operacao IN ('INSERT', 'UPDATE', 'DELETE')),
    data_hora     TIMESTAMP   NOT NULL DEFAULT now()
);

CREATE OR REPLACE FUNCTION fn_registrar_log()
RETURNS trigger AS $$
BEGIN
    INSERT INTO log_operacao (nome_tabela, tipo_operacao, data_hora)
    VALUES (TG_TABLE_NAME, TG_OP, now());

    IF TG_OP = 'DELETE' THEN
        RETURN OLD;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_log_sala
    AFTER INSERT OR UPDATE OR DELETE ON sala
    FOR EACH ROW EXECUTE FUNCTION fn_registrar_log();

CREATE TRIGGER trg_log_agendamento
    AFTER INSERT OR UPDATE OR DELETE ON agendamento
    FOR EACH ROW EXECUTE FUNCTION fn_registrar_log();

CREATE OR REPLACE FUNCTION fn_impede_exclusao_sala()
RETURNS trigger AS $$
BEGIN
    IF EXISTS (SELECT 1 FROM agendamento WHERE sala_id = OLD.id AND data_fim > now()) THEN
        RAISE EXCEPTION 'Nao e possivel excluir a sala: existem agendamentos futuros vinculados a ela.';
    END IF;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER trg_impede_exclusao_sala
    BEFORE DELETE ON sala
    FOR EACH ROW EXECUTE FUNCTION fn_impede_exclusao_sala();
