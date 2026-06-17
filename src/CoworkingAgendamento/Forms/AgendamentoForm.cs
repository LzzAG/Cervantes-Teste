using CoworkingAgendamento.Data;
using CoworkingAgendamento.Models;
using Npgsql;

namespace CoworkingAgendamento.Forms;

public class AgendamentoForm : Form
{
    private readonly AgendamentoRepositorio _repositorio = new();
    private readonly SalaRepositorio _salaRepositorio = new();
    private readonly ComboBox _cboSala = new();
    private readonly DateTimePicker _dtInicio = new();
    private readonly DateTimePicker _dtFim = new();
    private readonly DataGridView _grid = new();
    private int? _idSelecionado;

    public AgendamentoForm()
    {
        Text = "Cadastro de Agendamentos";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(700, 470);
        MinimumSize = new Size(700, 470);

        var lblSala = new Label { Text = "Sala", Location = new Point(15, 18), AutoSize = true };
        _cboSala.Location = new Point(15, 38);
        _cboSala.Size = new Size(250, 23);
        _cboSala.DropDownStyle = ComboBoxStyle.DropDownList;

        var lblInicio = new Label { Text = "Início", Location = new Point(285, 18), AutoSize = true };
        _dtInicio.Location = new Point(285, 38);
        _dtInicio.Size = new Size(170, 23);
        _dtInicio.Format = DateTimePickerFormat.Custom;
        _dtInicio.CustomFormat = "dd/MM/yyyy HH:mm";
        _dtInicio.ShowUpDown = true;

        var lblFim = new Label { Text = "Fim", Location = new Point(475, 18), AutoSize = true };
        _dtFim.Location = new Point(475, 38);
        _dtFim.Size = new Size(170, 23);
        _dtFim.Format = DateTimePickerFormat.Custom;
        _dtFim.CustomFormat = "dd/MM/yyyy HH:mm";
        _dtFim.ShowUpDown = true;

        var btnNovo = new Button { Text = "Novo", Location = new Point(15, 78), Size = new Size(90, 26) };
        var btnSalvar = new Button { Text = "Salvar", Location = new Point(110, 78), Size = new Size(110, 26) };
        var btnExcluir = new Button { Text = "Excluir", Location = new Point(225, 78), Size = new Size(110, 26) };

        _grid.Location = new Point(15, 118);
        _grid.Size = new Size(660, 300);
        _grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        _grid.ReadOnly = true;
        _grid.AllowUserToAddRows = false;
        _grid.RowHeadersVisible = false;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.CellClick += Grid_CellClick;

        btnNovo.Click += (s, e) => LimparFormulario();
        btnSalvar.Click += BtnSalvar_Click;
        btnExcluir.Click += BtnExcluir_Click;

        Controls.AddRange(new Control[]
        {
            lblSala, _cboSala, lblInicio, _dtInicio, lblFim, _dtFim,
            btnNovo, btnSalvar, btnExcluir, _grid
        });

        CarregarSalas();
        CarregarAgendamentos();
        LimparFormulario();
    }

    private void CarregarSalas()
    {
        _cboSala.DataSource = _salaRepositorio.Listar();
        _cboSala.DisplayMember = "Nome";
        _cboSala.ValueMember = "Id";
    }

    private void CarregarAgendamentos()
    {
        _grid.DataSource = _repositorio.Listar();
        _grid.Columns["Id"]!.Visible = false;
        _grid.Columns["SalaId"]!.Visible = false;
        _grid.Columns["SalaNome"]!.HeaderText = "Sala";
        _grid.Columns["DataInicio"]!.HeaderText = "Início";
        _grid.Columns["DataInicio"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        _grid.Columns["DataFim"]!.HeaderText = "Fim";
        _grid.Columns["DataFim"]!.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
    }

    private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (_grid.Rows[e.RowIndex].DataBoundItem is Agendamento agendamento)
        {
            _idSelecionado = agendamento.Id;
            _cboSala.SelectedValue = agendamento.SalaId;
            _dtInicio.Value = agendamento.DataInicio;
            _dtFim.Value = agendamento.DataFim;
        }
    }

    private void LimparFormulario()
    {
        _idSelecionado = null;
        if (_cboSala.Items.Count > 0)
            _cboSala.SelectedIndex = 0;
        _dtInicio.Value = DateTime.Now;
        _dtFim.Value = DateTime.Now.AddHours(1);
        _grid.ClearSelection();
    }

    private void BtnSalvar_Click(object? sender, EventArgs e)
    {
        if (_cboSala.SelectedValue is not int salaId)
            return;

        try
        {
            if (_idSelecionado is null)
                _repositorio.Inserir(salaId, _dtInicio.Value, _dtFim.Value);
            else
                _repositorio.Atualizar(_idSelecionado.Value, salaId, _dtInicio.Value, _dtFim.Value);

            CarregarAgendamentos();
            LimparFormulario();
        }
        catch (PostgresException ex)
        {
            MessageBox.Show(ErroBanco.Traduzir(ex), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private void BtnExcluir_Click(object? sender, EventArgs e)
    {
        if (_idSelecionado is null)
            return;

        try
        {
            _repositorio.Excluir(_idSelecionado.Value);
            CarregarAgendamentos();
            LimparFormulario();
        }
        catch (PostgresException ex)
        {
            MessageBox.Show(ErroBanco.Traduzir(ex), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
