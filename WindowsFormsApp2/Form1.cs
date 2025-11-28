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
            this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual} (SISTEMA ONLINE)";

            if (!Directory.Exists(pastaDasImagens)) Directory.CreateDirectory(pastaDasImagens);
            if (!Directory.Exists(pastaDosCertificados)) Directory.CreateDirectory(pastaDosCertificados);

            ConfigurarPermissoes();
        }

        private void ConfigurarPermissoes()
        {
            bool isAdmin = (usuarioAtual.ToLower() == "matheus.machado" || usuarioAtual.ToLower() == "admin");
            btnEditar.Enabled = isAdmin;
            btnExcluir.Enabled = isAdmin;
            btnMecanicos.Enabled = isAdmin;
            btnNova.Enabled = isAdmin;
        }

        private async void Form1_Load(object sender, EventArgs e) { await CarregarTudo(); }

        private async Task CarregarTudo()
        {
            lblTitulo.Text = "Sincronizando...";
            await CarregarDadosComCalibracao();
            await CarregarDadosSemCalibracao();
            lblTitulo.Text = "SISTEMA ONLINE (Azure)";
        }

        // --- ARQUIVOS ---
        private string SalvarArquivoLocal(byte[] dados, string pasta, string extensao)
        {
            if (dados == null || dados.Length == 0) return "";
            string nomeArquivo = Guid.NewGuid().ToString().Substring(0, 10) + extensao;
            try { File.WriteAllBytes(Path.Combine(pasta, nomeArquivo), dados); return nomeArquivo; } catch { return ""; }
        }
        private byte[] LerArquivoLocal(string nomeArquivo, string pasta)
        {
            if (string.IsNullOrEmpty(nomeArquivo)) return null;
            string caminho = Path.Combine(pasta, nomeArquivo);
            if (File.Exists(caminho)) return File.ReadAllBytes(caminho);
            return null;
        }

        // --- CARREGAR DADOS ---
        private async Task CarregarDadosComCalibracao()
        {
            dgvDados.Rows.Clear();
            List<string> vencer = new List<string>();
            try
            {
                var l = await ApiService.GetCom();
                foreach (var r in l)
                {
                    string f = SalvarArquivoLocal(r.Foto, pastaDasImagens, ".jpg");
                    string p = SalvarArquivoLocal(r.CertificadoPDF, pastaDosCertificados, ".pdf");
                    dgvDados.Rows.Add(r.InstrumentoNome, r.Modelo, r.PN, r.SN, r.IdentifSOD, r.IdentifOficina, r.Certificado, r.DataCalibracao, r.DataVencimento, null, r.Executante, r.Instalada, r.Local, r.SubLocalizacao, f, r.Observacoes, r.Mecanico, p);
                    dgvDados.Rows[dgvDados.Rows.Count - 1].Tag = r.ID;
                    if (DateTime.TryParseExact(r.DataVencimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dv))
                    {
                        int d = (int)(dv.Date - DateTime.Today).TotalDays;
                        if (d > 0 && d <= 45) vencer.Add($"{r.InstrumentoNome} ({d} dias)");
                    }
                }
                if (vencer.Count > 0 && tabControlPrincipal.SelectedTab == tabComCalibracao) MessageBox.Show("Vencimentos:\n" + string.Join("\n", vencer));
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar Com Calibração: " + ex.Message); }
        }

        private async Task CarregarDadosSemCalibracao()
        {
            dgvSemCalibracao.Rows.Clear();
            try
            {
                var l = await ApiService.GetSem();
                foreach (var r in l)
                {
                    string f = SalvarArquivoLocal(r.Foto, pastaDasImagens, ".jpg");
                    dgvSemCalibracao.Rows.Add(r.Descricao, r.Codigo, r.PN, r.Fabricante, r.Local, r.CadastroLocal, r.CodLocal, r.Status, r.Mecanico);
                    dgvSemCalibracao.Rows[dgvSemCalibracao.Rows.Count - 1].Tag = r.ID;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar Sem Calibração: " + ex.Message); }
        }

        // --- BOTÕES CRUD ---

        private async void btnNova_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                FormItemNovo f = new FormItemNovo();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    object[] d = f.NovoItemDados;
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
                        Foto = LerArquivoLocal(d[14]?.ToString(), pastaDasImagens),
                        Observacoes = d[15]?.ToString(),
                        Mecanico = d[16]?.ToString(),
                        CertificadoPDF = LerArquivoLocal(d[17]?.ToString(), pastaDosCertificados)
                    };
                    try { await ApiService.PostCom(i); await CarregarDadosComCalibracao(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
            else
            {
                FormItemSemCalibracao f = new FormItemSemCalibracao();
                if (f.ShowDialog() == DialogResult.OK)
                {
                    object[] d = f.ItemDados;
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
                        Foto = LerArquivoLocal(d[9]?.ToString(), pastaDasImagens),
                        CertificadoPDF = LerArquivoLocal(d[10]?.ToString(), pastaDosCertificados)
                    };
                    try { await ApiService.PostSem(i); await CarregarDadosSemCalibracao(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                if (dgvDados.SelectedRows.Count > 0)
                {
                    var row = dgvDados.SelectedRows[0];
                    FormItemEditar f = new FormItemEditar(row);
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        object[] d = f.ItemEditadoDados;
                        var i = new ApiService.Instrumento
                        {
                            ID = Convert.ToInt32(row.Tag),
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
                            Foto = LerArquivoLocal(d[14]?.ToString(), pastaDasImagens),
                            Observacoes = d[15]?.ToString(),
                            Mecanico = d[16]?.ToString(),
                            CertificadoPDF = LerArquivoLocal(d[17]?.ToString(), pastaDosCertificados)
                        };
                        try { await ApiService.PutCom(i.ID, i); await CarregarDadosComCalibracao(); MessageBox.Show("Atualizado!"); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
                else MessageBox.Show("Selecione um item.");
            }
            else
            {
                if (dgvSemCalibracao.SelectedRows.Count > 0)
                {
                    var row = dgvSemCalibracao.SelectedRows[0];
                    FormItemSemCalibracao f = new FormItemSemCalibracao(row);
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        object[] d = f.ItemDados;
                        var i = new ApiService.SemCalibracao
                        {
                            ID = Convert.ToInt32(row.Tag),
                            Descricao = d[0]?.ToString(),
                            Codigo = d[1]?.ToString(),
                            PN = d[2]?.ToString(),
                            Fabricante = d[3]?.ToString(),
                            Local = d[4]?.ToString(),
                            CadastroLocal = d[5]?.ToString(),
                            CodLocal = d[6]?.ToString(),
                            Status = d[7]?.ToString(),
                            Mecanico = d[8]?.ToString(),
                            Foto = LerArquivoLocal(d[9]?.ToString(), pastaDasImagens),
                            CertificadoPDF = LerArquivoLocal(d[10]?.ToString(), pastaDosCertificados)
                        };
                        try { await ApiService.PutSem(i.ID, i); await CarregarDadosSemCalibracao(); MessageBox.Show("Atualizado!"); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
                else MessageBox.Show("Selecione um item.");
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                if (dgvDados.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Excluir?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try { await ApiService.DelCom(Convert.ToInt32(dgvDados.SelectedRows[0].Tag)); await CarregarDadosComCalibracao(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
            }
            else
            {
                if (dgvSemCalibracao.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Excluir?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try { await ApiService.DelSem(Convert.ToInt32(dgvSemCalibracao.SelectedRows[0].Tag)); await CarregarDadosSemCalibracao(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
            }
        }

        // --- VISUAL / NAVEGAÇÃO ---
        private void btnMovimentacao_Click(object sender, EventArgs e) { new FormMovimentacao().ShowDialog(); _ = CarregarDadosComCalibracao(); }
        private void btnMecanicos_Click(object sender, EventArgs e) { new FormMecanicos().ShowDialog(); }
        private void btnRelatorios_Click(object sender, EventArgs e) { MessageBox.Show("Em breve."); }
        private void btnBuscaAvancada_Click(object sender, EventArgs e) { }
        private void btnFiltrar_Click(object sender, EventArgs e) { }
        private void btnInstrumentos_Click(object sender, EventArgs e) { }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            txtBusca.Visible = !txtBusca.Visible;
            lblBusca.Visible = !lblBusca.Visible;
            if (txtBusca.Visible) txtBusca.Focus(); else txtBusca.Text = "";
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            string t = txtBusca.Text.ToLower();
            DataGridView d = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? dgvDados : dgvSemCalibracao;
            string c = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? "colInstrumento" : "colSemDescricao";
            foreach (DataGridViewRow r in d.Rows) { if (!r.IsNewRow) r.Visible = (r.Cells[c].Value != null && r.Cells[c].Value.ToString().ToLower().Contains(t)); }
        }

        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvDados.Columns[e.ColumnIndex].Name == "colSituacao")
            {
                var cell = dgvDados.Rows[e.RowIndex].Cells["colDataVencimento"];
                if (cell.Value != null && DateTime.TryParseExact(cell.Value.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    int d = (int)(dt.Date - DateTime.Today).TotalDays;
                    if (d > 45) { e.Value = "CALIBRADO"; e.CellStyle.BackColor = Color.LightGreen; e.CellStyle.ForeColor = Color.Black; }
                    else if (d <= 0) { e.Value = "VENCIDO"; e.CellStyle.BackColor = Color.Red; e.CellStyle.ForeColor = Color.White; }
                    else { e.Value = "CALIBRAR"; e.CellStyle.BackColor = Color.Yellow; e.CellStyle.ForeColor = Color.Black; }
                    e.FormattingApplied = true;
                }
                else { e.Value = "N/A"; e.CellStyle.BackColor = Color.LightGray; }
            }
        }

        // --- DUPLO CLIQUE NAS GRIDS ---

        // Aba Com Calibração
        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Abre form de detalhes (apenas leitura/visualização rica)
                FormDetalhes f = new FormDetalhes(dgvDados.Rows[e.RowIndex]);
                f.ShowDialog();
            }
        }

        // Aba Sem Calibração (NOVO)
        private void dgvSemCalibracao_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Como "Sem Calibração" não tem form de Detalhes específico,
                // vamos chamar a função de Editar para ver as informações.
                btnEditar_Click(sender, e);
            }
        }

        private void panelPrincipal_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}