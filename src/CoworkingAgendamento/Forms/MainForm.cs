namespace CoworkingAgendamento.Forms;

public class MainForm : Form
{
    public MainForm()
    {
        Text = "Agendamento de Salas - Coworking";
        StartPosition = FormStartPosition.CenterScreen;
        Size = new Size(360, 240);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        var btnSalas = new Button
        {
            Text = "Salas",
            Size = new Size(220, 50),
            Location = new Point(60, 60)
        };
        btnSalas.Click += (s, e) => new SalaForm().ShowDialog();

        Controls.Add(btnSalas);
    }
}
