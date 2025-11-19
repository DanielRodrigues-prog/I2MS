using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormMovimentacao : Form
    {
        private string user = Environment.UserName.ToLower();
        private string nomeMec = "";
        private bool valido = false;

        public FormMovimentacao() { InitializeComponent(); txtMecanicoID.Text = user; }
        private async void FormMovimentacao_Load(object sender, EventArgs e)
        {
            var info = await ApiService.VerMec(user);
            if (info.Existe) { nomeMec = info.Nome; lblNomeMecanico.Text = "Nome: " + nomeMec; lblStatusMecanico.Text = "VÁLIDO"; lblStatusMecanico.ForeColor = Color.Green; valido = true; }
            else { lblStatusMecanico.Text = "NÃO CADASTRADO"; lblStatusMecanico.ForeColor = Color.Red; }
            btnCheckIn.Enabled = btnCheckOut.Enabled = valido;
        }
        private async void txtInstrumentoID_TextChanged(object sender, EventArgs e)
        {
            if (txtInstrumentoID.Text.Length > 2)
            {
                var s = await ApiService.VerFerr(txtInstrumentoID.Text);
                if (s.Encontrada)
                {
                    if (string.IsNullOrEmpty(s.MecanicoAtual)) { lblStatusFerramenta.Text = "DISPONÍVEL"; lblStatusFerramenta.ForeColor = Color.Green; }
                    else { lblStatusFerramenta.Text = "COM " + s.MecanicoAtual; lblStatusFerramenta.ForeColor = Color.Red; }
                }
                else { lblStatusFerramenta.Text = "NÃO ENCONTRADA"; }
            }
        }
        private void btnCheckOut_Click(object sender, EventArgs e) { Mover("PEGAR"); }
        private void btnCheckIn_Click(object sender, EventArgs e) { Mover("DEVOLVER"); }
        private async void Mover(string tipo)
        {
            if (!valido) return;
            string res = await ApiService.RegMov(tipo, txtInstrumentoID.Text, user, nomeMec);
            if (res == "OK") { MessageBox.Show("Sucesso!"); txtInstrumentoID.Clear(); } else MessageBox.Show(res);
        }
        private void btnFechar_Click(object sender, EventArgs e) { Close(); }
    }
}