namespace CoworkingAgendamento.Forms;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "Agendamento de Salas - Coworking";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(360, 300);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var btnSalas = new Button { Text = "Salas", Size = new Size(220, 45), Location = new Point(60, 30) };
        var btnAgendamentos = new Button { Text = "Agendamentos", Size = new Size(220, 45), Location = new Point(60, 90) };
        var btnLogs = new Button { Text = "Log de Operações", Size = new Size(220, 45), Location = new Point(60, 150) };

        btnSalas.Click += (s, e) => new SalaForm().ShowDialog();
        btnAgendamentos.Click += (s, e) => new AgendamentoForm().ShowDialog();
        btnLogs.Click += (s, e) => new LogForm().ShowDialog();

        Controls.AddRange(new Control[] { btnSalas, btnAgendamentos, btnLogs });
    }
}
