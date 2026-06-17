using CoworkingAgendamento.Data;

namespace CoworkingAgendamento.Forms;

public class LogForm : Form
{
    private readonly LogRepositorio _repositorio = new();
    private readonly DataGridView _grid = new();

    public LogForm()
    {
        Text = "Log de Operações";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(560, 430);
        MinimumSize = new Size(560, 430);
        Padding = new Padding(10);

        _grid.Dock = DockStyle.Fill;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.RowHeadersVisible = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        Controls.Add(_grid);

        Carregar();
    }

    private void Carregar()
    {
        _grid.DataSource = _repositorio.Listar();
        _grid.Columns["Id"]!.Visible = false;
        _grid.Columns["NomeTabela"]!.HeaderText = "Tabela";
        _grid.Columns["TipoOperacao"]!.HeaderText = "Operação";
        _grid.Columns["DataHora"]!.HeaderText = "Data/Hora";
        _grid.Columns["DataHora"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm:ss";
    }
}
