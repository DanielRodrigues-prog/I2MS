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

        // --- VARIÁVEIS DO DASHBOARD ---
        private Label lblDashTotal;
        private Label lblDashVencidos;
        private Label lblDashAVencer;
        private Label lblDashEmprestados;

        public Form1(string usuarioLogado)
        {
            InitializeComponent();

            this.usuarioAtual = usuarioLogado;
            this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual} (SISTEMA ONLINE)";

            // Garante pastas locais
            if (!Directory.Exists(pastaDasImagens)) Directory.CreateDirectory(pastaDasImagens);
            if (!Directory.Exists(pastaDosCertificados)) Directory.CreateDirectory(pastaDosCertificados);

            ConfigurarPermissoes();
        }

        private void ConfigurarPermissoes()
        {
            bool isAdmin = (usuarioAtual.ToLower() == "rodridae" || usuarioAtual.ToLower() == "admin");
            btnEditar.Enabled = isAdmin;
            btnExcluir.Enabled = isAdmin;
            btnMecanicos.Enabled = isAdmin;
            btnNova.Enabled = isAdmin;
        }

        // --- AQUI É O SEGREDO: CARREGA O DASHBOARD QUANDO A JANELA ABRE ---
        private async void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarDashboard(); // Desenha a tela colorida
            await CarregarTudo();  // Baixa os dados
        }

        private async Task CarregarTudo()
        {
            lblTitulo.Text = "Sincronizando dados...";
            await CarregarDadosComCalibracao();
            await CarregarDadosSemCalibracao();

            // Atualiza os números do dashboard depois de carregar os dados
            AtualizarNumerosDashboard();

            lblTitulo.Text = "SISTEMA ONLINE (Azure)";
        }

        // =============================================================
        //  DASHBOARD (VISUAL)
        // =============================================================
        private void ConfigurarDashboard()
        {
            // 1. Cria a nova Aba
            TabPage tabDash = new TabPage("VISÃO GERAL");
            tabDash.BackColor = Color.WhiteSmoke;

            // Adiciona ela na primeira posição (0)
            tabControlPrincipal.TabPages.Insert(0, tabDash);
            tabControlPrincipal.SelectedIndex = 0; // Força abrir nela

            // 2. Título
            Label lblTituloDash = new Label();
            lblTituloDash.Text = "Resumo da Ferramentaria";
            lblTituloDash.ForeColor = Color.DimGray;
            lblTituloDash.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            lblTituloDash.AutoSize = true;
            lblTituloDash.Location = new Point(20, 20);
            tabDash.Controls.Add(lblTituloDash);

            // 3. Criação dos Cards (Posição X, Y)
            lblDashTotal = CriarCard(tabDash, "Total de Itens", Color.RoyalBlue, 20, 70);
            lblDashVencidos = CriarCard(tabDash, "Itens Vencidos", Color.Firebrick, 240, 70);
            lblDashAVencer = CriarCard(tabDash, "A Vencer (30 dias)", Color.DarkOrange, 460, 70);
            lblDashEmprestados = CriarCard(tabDash, "Emprestados", Color.SeaGreen, 680, 70);

            // Botão de Atualizar Manual
            Button btnRefresh = new Button();
            btnRefresh.Text = "Atualizar Agora";
            btnRefresh.BackColor = Color.White;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Size = new Size(120, 30);
            btnRefresh.Location = new Point(20, 200);
            btnRefresh.Click += (s, e) => { _ = CarregarTudo(); };
            tabDash.Controls.Add(btnRefresh);
        }

        private Label CriarCard(TabPage aba, string titulo, Color cor, int x, int y)
        {
            Panel pnl = new Panel();
            pnl.Size = new Size(200, 100);
            pnl.Location = new Point(x, y);
            pnl.BackColor = cor;

            Label lblTit = new Label();
            lblTit.Text = titulo;
            lblTit.ForeColor = Color.White;
            lblTit.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            lblTit.Location = new Point(10, 10);
            lblTit.AutoSize = true;
            pnl.Controls.Add(lblTit);

            Label lblValor = new Label();
            lblValor.Text = "...";
            lblValor.ForeColor = Color.White;
            lblValor.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblValor.Location = new Point(10, 35);
            lblValor.AutoSize = true;
            pnl.Controls.Add(lblValor);

            aba.Controls.Add(pnl);
            return lblValor;
        }

        private void AtualizarNumerosDashboard()
        {
            if (dgvDados == null || dgvDados.Rows.Count == 0) return;

            int total = dgvDados.Rows.Count;
            int vencidos = 0;
            int aVencer = 0;
            int emprestados = 0;

            foreach (DataGridViewRow row in dgvDados.Rows)
            {
                // Vencimento
                var cellVenc = row.Cells["colDataVencimento"].Value?.ToString();
                if (DateTime.TryParseExact(cellVenc, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    int dias = (int)(dt.Date - DateTime.Today).TotalDays;
                    if (dias <= 0) vencidos++;
                    else if (dias <= 30) aVencer++;
                }

                // Empréstimo
                var cellMec = row.Cells["colMecanico"].Value?.ToString();
                if (!string.IsNullOrEmpty(cellMec)) emprestados++;
            }

            if (lblDashTotal != null) lblDashTotal.Text = total.ToString();
            if (lblDashVencidos != null) lblDashVencidos.Text = vencidos.ToString();
            if (lblDashAVencer != null) lblDashAVencer.Text = aVencer.ToString();
            if (lblDashEmprestados != null) lblDashEmprestados.Text = emprestados.ToString();
        }

        // =============================================================
        //  ARQUIVOS
        // =============================================================
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

        // =============================================================
        //  CARREGAMENTO (GET)
        // =============================================================
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

                if (vencer.Count > 0 && tabControlPrincipal.SelectedTab == tabComCalibracao)
                    MessageBox.Show("Vencimentos Próximos:\n" + string.Join("\n", vencer), "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    string p = SalvarArquivoLocal(r.CertificadoPDF, pastaDosCertificados, ".pdf");
                    dgvSemCalibracao.Rows.Add(r.Descricao, r.Codigo, r.PN, r.Fabricante, r.Local, r.CadastroLocal, r.CodLocal, r.Status, r.Mecanico);
                    dgvSemCalibracao.Rows[dgvSemCalibracao.Rows.Count - 1].Tag = r.ID;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar Sem Calibração: " + ex.Message); }
        }

        // =============================================================
        //  BOTÕES CRUD
        // =============================================================
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
                    try { await ApiService.PostCom(i); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
            else if (tabControlPrincipal.SelectedTab == tabSemCalibracao)
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
                    try { await ApiService.PostSem(i); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
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
                        try { await ApiService.PutCom(i.ID, i); await CarregarTudo(); MessageBox.Show("Atualizado!"); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
            }
            else if (tabControlPrincipal.SelectedTab == tabSemCalibracao)
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
                        try { await ApiService.PutSem(i.ID, i); await CarregarTudo(); MessageBox.Show("Atualizado!"); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
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
                        try { await ApiService.DelCom(Convert.ToInt32(dgvDados.SelectedRows[0].Tag)); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
            }
            else if (tabControlPrincipal.SelectedTab == tabSemCalibracao)
            {
                if (dgvSemCalibracao.SelectedRows.Count > 0)
                {
                    if (MessageBox.Show("Excluir?", "Confirma", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        try { await ApiService.DelSem(Convert.ToInt32(dgvSemCalibracao.SelectedRows[0].Tag)); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                    }
                }
            }
        }

        // --- VISUAL / NAVEGAÇÃO ---
        private void btnMovimentacao_Click(object sender, EventArgs e) { new FormMovimentacao().ShowDialog(); _ = CarregarTudo(); }
        private void btnMecanicos_Click(object sender, EventArgs e) { new FormMecanicos().ShowDialog(); }
        private void btnRelatorios_Click(object sender, EventArgs e) { MessageBox.Show("Em breve."); }
        private void btnBuscaAvancada_Click(object sender, EventArgs e) { }
        private void btnFiltrar_Click(object sender, EventArgs e) { }
        private void btnInstrumentos_Click(object sender, EventArgs e) { if (tabControlPrincipal.TabCount > 1) tabControlPrincipal.SelectedIndex = 1; }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            txtBusca.Visible = !txtBusca.Visible;
            lblBusca.Visible = !lblBusca.Visible;
            if (txtBusca.Visible) txtBusca.Focus(); else txtBusca.Text = "";
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            string t = txtBusca.Text.ToLower();
            if (tabControlPrincipal.SelectedIndex == 0) return; // Não busca no dashboard

            DataGridView dgv = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? dgvDados : dgvSemCalibracao;
            string c = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? "colInstrumento" : "colSemDescricao";
            foreach (DataGridViewRow r in dgv.Rows) { if (!r.IsNewRow) r.Visible = (r.Cells[c].Value != null && r.Cells[c].Value.ToString().ToLower().Contains(t)); }
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

        // DUPLO CLIQUE
        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                FormDetalhes f = new FormDetalhes(dgvDados.Rows[e.RowIndex]);
                f.ShowDialog();
            }
        }

        private void dgvSemCalibracao_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditar_Click(sender, e);
            }
        }

        private void btnImportarCSV_Click(object sender, EventArgs e)
        {
            // Método deixado vazio ou removido no designer.
            // Como removemos o botão no Designer.cs, esse método pode ficar órfão sem problemas
            // ou você pode apagá-lo.
        }
    }
}