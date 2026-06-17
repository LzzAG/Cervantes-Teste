using CoworkingAgendamento.Data;
using CoworkingAgendamento.Models;
using Npgsql;

namespace CoworkingAgendamento.Forms;

public class SalaForm : Form
{
    private readonly SalaRepositorio _repositorio = new();
    private readonly DataGridView _grid = new();
    private readonly TextBox _txtNome = new();
    private int? _idSelecionado;

    public SalaForm()
    {
        Text = "Cadastro de Salas";
        StartPosition = FormStartPosition.CenterParent;
        Size = new Size(560, 430);
        MinimumSize = new Size(560, 430);

        var lblNome = new Label { Text = "Nome", Location = new Point(15, 18), AutoSize = true };
        _txtNome.Location = new Point(15, 38);
        _txtNome.Size = new Size(300, 23);

        var btnNovo = new Button { Text = "Novo", Location = new Point(330, 37), Size = new Size(90, 26) };
        var btnSalvar = new Button { Text = "Salvar", Location = new Point(425, 37), Size = new Size(110, 26) };
        var btnExcluir = new Button { Text = "Excluir", Location = new Point(425, 70), Size = new Size(110, 26) };

        _grid.Location = new Point(15, 110);
        _grid.Size = new Size(520, 270);
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

        Controls.AddRange(new Control[] { lblNome, _txtNome, btnNovo, btnSalvar, btnExcluir, _grid });

        CarregarSalas();
        LimparFormulario();
    }

    private void CarregarSalas()
    {
        _grid.DataSource = _repositorio.Listar();
        _grid.Columns["Id"]!.HeaderText = "Código";
        _grid.Columns["Nome"]!.HeaderText = "Nome";
    }

    private void Grid_CellClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0)
            return;

        if (_grid.Rows[e.RowIndex].DataBoundItem is Sala sala)
        {
            _idSelecionado = sala.Id;
            _txtNome.Text = sala.Nome;
        }
    }

    private void LimparFormulario()
    {
        _idSelecionado = null;
        _txtNome.Clear();
        _grid.ClearSelection();
        _txtNome.Focus();
    }

    private void BtnSalvar_Click(object? sender, EventArgs e)
    {
        try
        {
            if (_idSelecionado is null)
                _repositorio.Inserir(_txtNome.Text);
            else
                _repositorio.Atualizar(_idSelecionado.Value, _txtNome.Text);

            CarregarSalas();
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
            CarregarSalas();
            LimparFormulario();
        }
        catch (PostgresException ex)
        {
            MessageBox.Show(ErroBanco.Traduzir(ex), "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}
