using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace WindowsFormsApp2
{
    public partial class FormHistoricoFerramenta : Form
    {
        private DataGridViewRow _ferramenta;
        private List<ApiService.HistoricoItem> _historico;

        // Paineis
        private Panel pnlTimeline;
        private Panel pnlStats;
        private Panel pnlGrafico;
        private DataGridView dgvHistorico;

        // Detalhes (Foto e PDF)
        private PictureBox pbFoto;
        private Button btnVerPDF;
        private string _pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");
        private string _pastaCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");

        public FormHistoricoFerramenta(DataGridViewRow ferramenta)
        {
            InitializeComponent();

            if (ferramenta == null)
            {
                MessageBox.Show("Erro: Nenhuma ferramenta selecionada.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            _ferramenta = ferramenta;
            _historico = new List<ApiService.HistoricoItem>();

            this.Text = "Histórico e Estatísticas";
            this.Size = new Size(1250, 850);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 245);

            CriarInterface();
            _ = CarregarDados();
        }

        private void FormHistoricoFerramenta_Load(object sender, EventArgs e) { }

        private void CriarInterface()
        {
            // === 1. CABEÇALHO ===
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(41, 128, 185)
            };

            string nomeFerramenta = _ferramenta.Cells["colInstrumento"]?.Value?.ToString() ??
                                  _ferramenta.Cells["colSemDescricao"]?.Value?.ToString() ?? "Ferramenta";

            string pn = _ferramenta.Cells["colPN"]?.Value?.ToString() ??
                       _ferramenta.Cells["colSemPN"]?.Value?.ToString() ?? "";

            Label lblNome = new Label
            {
                Text = nomeFerramenta,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(20, 10)
            };

            Label lblPN = new Label
            {
                Text = $"P/N: {pn}",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Location = new Point(22, 38)
            };

            pnlHeader.Controls.AddRange(new Control[] { lblNome, lblPN });
            this.Controls.Add(pnlHeader);

            // === CONTEÚDO PRINCIPAL ===
            Panel pnlMain = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20)
            };
            this.Controls.Add(pnlMain);
            pnlMain.BringToFront();

            // === 2. PAINEL DE DETALHES (FOTO/PDF) ===
            Panel pnlDetalhes = new Panel
            {
                Location = new Point(900, 50),
                Size = new Size(280, 280),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Label lblFotoTitle = new Label { Text = "FOTO / CERTIFICADO", Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true, ForeColor = Color.DimGray };

            pbFoto = new PictureBox
            {
                Location = new Point(10, 35),
                Size = new Size(260, 180),
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.WhiteSmoke
            };

            btnVerPDF = new Button
            {
                Text = "📄 Abrir Certificado",
                Location = new Point(10, 225),
                Size = new Size(260, 40),
                BackColor = Color.FromArgb(52, 152, 219),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Enabled = false
            };
            btnVerPDF.Click += BtnVerPDF_Click;

            pnlDetalhes.Controls.AddRange(new Control[] { lblFotoTitle, pbFoto, btnVerPDF });
            pnlMain.Controls.Add(pnlDetalhes);

            // === 3. CARDS DE ESTATÍSTICAS ===
            pnlStats = new Panel
            {
                Location = new Point(20, 50),
                Size = new Size(860, 120),
                BackColor = Color.Transparent
            };
            pnlMain.Controls.Add(pnlStats);

            // === 4. TIMELINE ===
            Label lblTimelineTitle = new Label
            {
                Text = "📅 LINHA DO TEMPO",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 200),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            pnlMain.Controls.Add(lblTimelineTitle);

            pnlTimeline = new Panel
            {
                Location = new Point(20, 230),
                Size = new Size(860, 130),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoScroll = true
            };
            pnlMain.Controls.Add(pnlTimeline);

            // === 5. GRÁFICO (PIZZA) ===
            Label lblGraficoTitle = new Label
            {
                Text = "📊 TEMPO DE USO",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(20, 400),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            pnlMain.Controls.Add(lblGraficoTitle);

            pnlGrafico = new Panel
            {
                Location = new Point(20, 430),
                Size = new Size(350, 250),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            pnlGrafico.Paint += PnlGrafico_Paint;
            pnlMain.Controls.Add(pnlGrafico);

            // === 6. TABELA DE HISTÓRICO ===
            Label lblTabelaTitle = new Label
            {
                Text = "📋 REGISTROS DETALHADOS",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(400, 400),
                AutoSize = true,
                ForeColor = Color.FromArgb(52, 73, 94)
            };
            pnlMain.Controls.Add(lblTabelaTitle);

            dgvHistorico = new DataGridView
            {
                Location = new Point(400, 430),
                Size = new Size(780, 250),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Data", Name = "colData", Width = 110 });
            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Ação", Name = "colAcao", Width = 80 });
            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mecânico", Name = "colMec", Width = 140 });
            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Aeronave", Name = "colAero", Width = 90 });
            dgvHistorico.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Admin", Name = "colAdmin", Width = 110 });

            pnlMain.Controls.Add(dgvHistorico);
        }

        private async Task CarregarDados()
        {
            try
            {
                // 1. Carregar Foto e PDF
                CarregarFotoEPDF();

                // 2. Carregar Histórico
                var todosHistoricos = await ApiService.GetHistorico();

                if (todosHistoricos == null)
                {
                    _historico = new List<ApiService.HistoricoItem>();
                    PreencherEstatisticas();
                    DesenharTimeline();
                    return;
                }

                // --- LÓGICA DE FILTRO CORRIGIDA ---

                // Tenta pegar o ID da Tag da linha
                string idFerramenta = _ferramenta.Tag?.ToString() ?? "";

                // Pega o nome
                string nomeFerramenta = _ferramenta.Cells["colInstrumento"]?.Value?.ToString() ??
                                      _ferramenta.Cells["colSemDescricao"]?.Value?.ToString() ?? "";

                // Filtra se bater o ID *OU* se bater o Nome (mais seguro)
                _historico = todosHistoricos
                             .Where(h =>
                                (h.Ferramenta != null && h.Ferramenta == idFerramenta) || // Bate pelo ID
                                (h.Ferramenta != null && h.Ferramenta.Trim().Equals(nomeFerramenta.Trim(), StringComparison.OrdinalIgnoreCase)) // Bate pelo Nome
                             )
                             .ToList();

                PreencherEstatisticas();
                DesenharTimeline();
                PreencherTabela();
                pnlGrafico.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }

        private void CarregarFotoEPDF()
        {
            string nomeFoto = _ferramenta.Cells["colFoto"]?.Value?.ToString();
            // Se for sem calibração, tenta pegar de outra coluna se necessário
            if (_ferramenta.DataGridView != null && _ferramenta.DataGridView.Name == "dgvSemCalibracao")
            {
                // Verifique se a coluna se chama colFoto mesmo na tabela sem calibração
                // nomeFoto = _ferramenta.Cells["colFotoSem"]?.Value?.ToString(); 
            }

            if (!string.IsNullOrEmpty(nomeFoto))
            {
                string path = Path.Combine(_pastaImagens, nomeFoto);
                if (File.Exists(path))
                {
                    try
                    {
                        using (var ms = new MemoryStream(File.ReadAllBytes(path)))
                            pbFoto.Image = Image.FromStream(ms);
                    }
                    catch { }
                }
            }

            string nomePDF = _ferramenta.Cells["colCertificadoPDF"]?.Value?.ToString();
            if (!string.IsNullOrEmpty(nomePDF))
            {
                string path = Path.Combine(_pastaCertificados, nomePDF);
                if (File.Exists(path))
                {
                    btnVerPDF.Enabled = true;
                    btnVerPDF.Tag = path;
                }
                else
                {
                    btnVerPDF.Text = "PDF não encontrado";
                    btnVerPDF.Enabled = false;
                }
            }
            else
            {
                btnVerPDF.Text = "Sem Certificado";
                btnVerPDF.Enabled = false;
            }
        }

        private void BtnVerPDF_Click(object sender, EventArgs e)
        {
            if (btnVerPDF.Tag != null)
            {
                try { Process.Start(btnVerPDF.Tag.ToString()); }
                catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
            }
        }

        private void PreencherEstatisticas()
        {
            int totalEmprestimos = _historico.Count(h => h.Acao != null && h.Acao.ToUpper().Contains("SAÍDA"));
            int totalCalibracoes = ObterTotalCalibracoes();
            int diasEmUso = CalcularDiasEmUso();
            int diasParado = CalcularDiasParado();

            CriarCard(pnlStats, "EMPRÉSTIMOS", totalEmprestimos.ToString(), Color.FromArgb(52, 152, 219), 0);
            CriarCard(pnlStats, "CALIBRAÇÕES", totalCalibracoes.ToString(), Color.FromArgb(46, 204, 113), 215);
            CriarCard(pnlStats, "DIAS USO", diasEmUso.ToString(), Color.FromArgb(230, 126, 34), 430);
            CriarCard(pnlStats, "DIAS PARADO", diasParado.ToString(), Color.FromArgb(149, 165, 166), 645);
        }

        private void CriarCard(Panel parent, string titulo, string valor, Color cor, int x)
        {
            Panel card = new Panel { Location = new Point(x, 0), Size = new Size(200, 100), BackColor = cor };
            Label lblTitulo = new Label { Text = titulo, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.White, Location = new Point(10, 15), AutoSize = true };
            Label lblValor = new Label { Text = valor, Font = new Font("Segoe UI", 26, FontStyle.Bold), ForeColor = Color.White, Location = new Point(10, 40), AutoSize = true };
            card.Controls.AddRange(new Control[] { lblTitulo, lblValor });
            parent.Controls.Add(card);
        }

        private void DesenharTimeline()
        {
            pnlTimeline.Controls.Clear();
            if (_historico == null || _historico.Count == 0)
            {
                Label lblVazio = new Label { Text = "Sem histórico.", Font = new Font("Segoe UI", 12), ForeColor = Color.Gray, AutoSize = true, Location = new Point(20, 50) };
                pnlTimeline.Controls.Add(lblVazio);
                return;
            }

            int x = 50;
            int spacing = 180;
            // Ordena por data decrescente (mais recente primeiro)
            var historicoOrdenado = _historico.OrderByDescending(h => h.Data).Take(10).Reverse().ToList();

            Panel linha = new Panel { Location = new Point(40, 70), Size = new Size((historicoOrdenado.Count * spacing), 3), BackColor = Color.LightGray };
            pnlTimeline.Controls.Add(linha);

            foreach (var item in historicoOrdenado)
            {
                Color cor = (item.Acao != null && item.Acao.ToUpper().Contains("SAÍDA")) ? Color.FromArgb(231, 76, 60) : Color.FromArgb(46, 204, 113);
                Panel bolinha = new Panel { Location = new Point(x, 62), Size = new Size(18, 18), BackColor = cor };
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath(); path.AddEllipse(0, 0, 18, 18); bolinha.Region = new Region(path);

                Label lblData = new Label { Text = item.Data, Font = new Font("Segoe UI", 7, FontStyle.Bold), AutoSize = true, Location = new Point(x - 20, 45) };
                Label lblAcao = new Label { Text = item.Acao, Font = new Font("Segoe UI", 8, FontStyle.Bold), ForeColor = cor, AutoSize = true, Location = new Point(x - 10, 85) };

                pnlTimeline.Controls.AddRange(new Control[] { bolinha, lblData, lblAcao });
                bolinha.BringToFront();
                x += spacing;
            }
        }

        private void PreencherTabela()
        {
            dgvHistorico.Rows.Clear();
            if (_historico == null) return;
            foreach (var item in _historico.OrderByDescending(h => h.Data))
            {
                int idx = dgvHistorico.Rows.Add(item.Data, item.Acao, item.Mecanico, item.Aeronave, item.Admin);
                if (item.Acao != null && item.Acao.ToUpper().Contains("SAÍDA")) dgvHistorico.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(255, 235, 235);
                else dgvHistorico.Rows[idx].DefaultCellStyle.BackColor = Color.FromArgb(235, 255, 235);
            }
        }

        private void PnlGrafico_Paint(object sender, PaintEventArgs e)
        {
            int diasUso = CalcularDiasEmUso();
            int diasParado = CalcularDiasParado();
            int total = diasUso + diasParado;
            if (total == 0) return;

            float percUso = (float)diasUso / total;
            float anguloUso = percUso * 360;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle(20, 30, 160, 160);
            g.FillPie(new SolidBrush(Color.FromArgb(230, 126, 34)), rect, 0, anguloUso);
            g.FillPie(new SolidBrush(Color.FromArgb(149, 165, 166)), rect, anguloUso, 360 - anguloUso);

            g.FillRectangle(new SolidBrush(Color.FromArgb(230, 126, 34)), 200, 60, 15, 15);
            g.DrawString($"Em Uso: {diasUso}d ({(percUso * 100):F0}%)", new Font("Segoe UI", 9), Brushes.Black, 220, 60);
            g.FillRectangle(new SolidBrush(Color.FromArgb(149, 165, 166)), 200, 100, 15, 15);
            g.DrawString($"Parado: {diasParado}d ({((1 - percUso) * 100):F0}%)", new Font("Segoe UI", 9), Brushes.Black, 220, 100);
        }

        private int CalcularDiasEmUso()
        {
            if (_historico == null) return 0;
            return _historico.Count(h => h.Acao != null && h.Acao.ToUpper().Contains("SAÍDA"));
        }

        private int CalcularDiasParado()
        {
            return Math.Max(0, 365 - CalcularDiasEmUso());
        }

        private int ObterTotalCalibracoes()
        {
            if (_historico == null) return 0;
            return _historico.Count(h => (h.Acao != null && h.Acao.ToUpper().Contains("CALIBRA")) || (h.Aeronave != null && h.Aeronave.ToUpper().Contains("CALIBRA")));
        }
    }
}