using System;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormMecanicos : Form
    {
        public FormMecanicos() { InitializeComponent(); }
        private async void FormMecanicos_Load(object sender, EventArgs e) { await Carregar(); }
        private async System.Threading.Tasks.Task Carregar()
        {
            dgvMecanicos.Rows.Clear();
            try { foreach (var m in await ApiService.GetMecs()) dgvMecanicos.Rows.Add(m.MecanicoID, m.Nome, m.StatusBloqueio); } catch { }
        }
        private async void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (txtMecanicoID.Text != "") { await ApiService.AddMec(new ApiService.Mecanico { MecanicoID = txtMecanicoID.Text, Nome = txtNome.Text }); txtMecanicoID.Clear(); txtNome.Clear(); await Carregar(); }
        }
        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvMecanicos.SelectedRows.Count > 0) { await ApiService.DelMec(dgvMecanicos.SelectedRows[0].Cells[0].Value.ToString()); await Carregar(); }
        }
    }
}