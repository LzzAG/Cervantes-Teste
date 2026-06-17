# Agendamento de Salas - Coworking

Aplicação desktop em Windows Forms com PostgreSQL para cadastro de salas e agendamentos. As regras de negócio são validadas no banco (constraints e triggers).

## Como rodar

1. Criar o banco e rodar o script:

   ```
   psql -d coworking -f database/script.sql
   ```

2. Ajustar a string de conexão em `src/CoworkingAgendamento/appsettings.json`.

3. Executar:

   ```
   dotnet run --project src/CoworkingAgendamento
   ```
