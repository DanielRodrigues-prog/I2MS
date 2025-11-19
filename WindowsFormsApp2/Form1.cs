using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string usuarioAtual;
        private string pastaDasImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");
        private string pastaDosCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");

        public Form1(string usuarioLogado)
        {
            InitializeComponent();
            this.usuarioAtual = usuarioLogado;
            if (!Directory.Exists(pastaDasImagens)) Directory.CreateDirectory(pastaDasImagens);
            if (!Directory.Exists(pastaDosCertificados)) Directory.CreateDirectory(pastaDosCertificados);

            // Permissões simples
            bool admin = (usuarioLogado.ToLower() == "rodridae");
            btnEditar.Enabled = admin; btnExcluir.Enabled = admin; btnMecanicos.Enabled = admin; btnNova.Enabled = admin;
        }

        private async void Form1_Load(object sender, EventArgs e) { await CarregarTudo(); }

        private async Task CarregarTudo()
        {
            lblTitulo.Text = "Carregando...";
            await CarregarCom();
            await CarregarSem();
            lblTitulo.Text = "SISTEMA ONLINE";
        }

        private string SalvarLocal(byte[] b, string p, string ext)
        {
            if (b == null || b.Length == 0) return "";
            string n = Guid.NewGuid().ToString().Substring(0, 8) + ext;
            File.WriteAllBytes(Path.Combine(p, n), b);
            return n;
        }
        private byte[] LerLocal(string n, string p) => (!string.IsNullOrEmpty(n) && File.Exists(Path.Combine(p, n))) ? File.ReadAllBytes(Path.Combine(p, n)) : null;

        private async Task CarregarCom()
        {
            dgvDados.Rows.Clear();
            try
            {
                var l = await ApiService.GetCom();
                foreach (var r in l)
                {
                    string f = SalvarLocal(r.Foto, pastaDasImagens, ".jpg");
                    string p = SalvarLocal(r.CertificadoPDF, pastaDosCertificados, ".pdf");
                    dgvDados.Rows.Add(r.InstrumentoNome, r.Modelo, r.PN, r.SN, r.IdentifSOD, r.IdentifOficina, r.Certificado, r.DataCalibracao, r.DataVencimento, null, r.Executante, r.Instalada, r.Local, r.SubLocalizacao, f, r.Observacoes, r.Mecanico, p);
                    dgvDados.Rows[dgvDados.Rows.Count - 1].Tag = r.ID;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        private async Task CarregarSem()
        {
            dgvSemCalibracao.Rows.Clear();
            try
            {
                var l = await ApiService.GetSem();
                foreach (var r in l)
                {
                    SalvarLocal(r.Foto, pastaDasImagens, ".jpg");
                    dgvSemCalibracao.Rows.Add(r.Descricao, r.Codigo, r.PN, r.Fabricante, r.Local, r.CadastroLocal, r.CodLocal, r.Status, r.Mecanico);
                    dgvSemCalibracao.Rows[dgvSemCalibracao.Rows.Count - 1].Tag = r.ID;
                }
            }
            catch { }
        }

        private async void btnNova_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                var f = new FormItemNovo();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var d = f.NovoItemDados;
                    var i = new ApiService.Instrumento
                    {
                        InstrumentoNome = d[0]?.ToString(),
                        Modelo = d[1]?.ToString(),
                        PN = d[2]?.ToString(),
                        SN = d[3]?.ToString(),
                        IdentifSOD = d[4]?.ToString(),
                        IdentifOficina = d[5]?.ToString(),
                        Certificado = d[6]?.ToString(),
                        DataCalibracao = ((DateTime)d[7]).ToString("dd/MM/yyyy"),
                        DataVencimento = ((DateTime)d[8]).ToString("dd/MM/yyyy"),
                        Executante = d[10]?.ToString(),
                        Instalada = d[11]?.ToString(),
                        Local = d[12]?.ToString(),
                        SubLocalizacao = d[13]?.ToString(),
                        Foto = LerLocal(d[14]?.ToString(), pastaDasImagens),
                        Observacoes = d[15]?.ToString(),
                        Mecanico = d[16]?.ToString(),
                        CertificadoPDF = LerLocal(d[17]?.ToString(), pastaDosCertificados)
                    };
                    await ApiService.PostCom(i); await CarregarCom();
                }
            }
            else
            {
                var f = new FormItemSemCalibracao();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var d = f.ItemDados;
                    var i = new ApiService.SemCalibracao
                    {
                        Descricao = d[0]?.ToString(),
                        Codigo = d[1]?.ToString(),
                        PN = d[2]?.ToString(),
                        Fabricante = d[3]?.ToString(),
                        Local = d[4]?.ToString(),
                        CadastroLocal = d[5]?.ToString(),
                        CodLocal = d[6]?.ToString(),
                        Status = d[7]?.ToString(),
                        Mecanico = d[8]?.ToString(),
                        Foto = LerLocal(d[9]?.ToString(), pastaDasImagens),
                        CertificadoPDF = LerLocal(d[10]?.ToString(), pastaDosCertificados)
                    };
                    await ApiService.PostSem(i); await CarregarSem();
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count > 0)
            {
                var row = dgvDados.SelectedRows[0];
                int id = Convert.ToInt32(row.Tag);
                var f = new FormItemEditar(row);
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var d = f.ItemEditadoDados;
                    var i = new ApiService.Instrumento
                    {
                        ID = id,
                        InstrumentoNome = d[0]?.ToString(),
                        Modelo = d[1]?.ToString(),
                        PN = d[2]?.ToString(),
                        SN = d[3]?.ToString(),
                        IdentifSOD = d[4]?.ToString(),
                        IdentifOficina = d[5]?.ToString(),
                        Certificado = d[6]?.ToString(),
                        DataCalibracao = ((DateTime)d[7]).ToString("dd/MM/yyyy"),
                        DataVencimento = ((DateTime)d[8]).ToString("dd/MM/yyyy"),
                        Executante = d[10]?.ToString(),
                        Instalada = d[11]?.ToString(),
                        Local = d[12]?.ToString(),
                        SubLocalizacao = d[13]?.ToString(),
                        Foto = LerLocal(d[14]?.ToString(), pastaDasImagens),
                        Observacoes = d[15]?.ToString(),
                        Mecanico = d[16]?.ToString(),
                        CertificadoPDF = LerLocal(d[17]?.ToString(), pastaDosCertificados)
                    };
                    await ApiService.PutCom(id, i); await CarregarCom();
                }
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("Excluir?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    await ApiService.DelCom(Convert.ToInt32(dgvDados.SelectedRows[0].Tag)); await CarregarCom();
                }
            }
        }

        private void btnMovimentacao_Click(object sender, EventArgs e) { new FormMovimentacao().ShowDialog(); CarregarCom(); }
        private void btnMecanicos_Click(object sender, EventArgs e) { new FormMecanicos().ShowDialog(); }
        private void btnProcurar_Click(object sender, EventArgs e) { txtBusca.Visible = !txtBusca.Visible; }
        private void txtBusca_TextChanged(object sender, EventArgs e) { /* Lógica de filtro visual igual ao anterior */ }
        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e) { /* Lógica visual de cores igual ao anterior */ }
        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) new FormDetalhes(dgvDados.Rows[e.RowIndex]).ShowDialog(); }
    }
}