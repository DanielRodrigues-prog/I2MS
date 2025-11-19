using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormSemCalibracao : Form
    {
        public FormSemCalibracao() { InitializeComponent(); }
        private async void FormSemCalibracao_Load(object sender, EventArgs e)
        {
            dgvDados.Rows.Clear();
            try { foreach (var r in await ApiService.GetSem()) dgvDados.Rows.Add(r.Descricao, r.Codigo, r.PN, r.Fabricante, r.Local, r.CadastroLocal, r.CodLocal, r.Status); } catch { }
        }
    }
}