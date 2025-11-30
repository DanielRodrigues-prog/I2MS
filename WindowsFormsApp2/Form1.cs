using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string usuarioAtual;
        private string pastaDasImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");
        private string pastaDosCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");

        // Variáveis do Dashboard
      private Label lblDashTotal, lblDashVencidos, lblDashAVencer, lblDashEmprestados;

// Variáveis Visuais
        private Timer timerAnimacao;
     private PictureBox previewBox; // Janelinha flutuante da foto
        private DataGridView dgvHist; // Referência ao histórico para refresh

        public Form1(string usuarioLogado)
        {
            InitializeComponent();

     // Configuração de Fluidez
            this.DoubleBuffered = true;
         this.SetStyle(ControlStyles.ResizeRedraw, true);

   this.usuarioAtual = usuarioLogado;
   this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual} (SISTEMA ONLINE)";

            // Garante pastas locais
            if (!Directory.Exists(pastaDasImagens)) Directory.CreateDirectory(pastaDasImagens);
  if (!Directory.Exists(pastaDosCertificados)) Directory.CreateDirectory(pastaDosCertificados);

ConfigurarPermissoes();
          ConfigurarPreviewFoto();

            // Animação de entrada
   this.Opacity = 0;
  timerAnimacao = new Timer { Interval = 15 };
       timerAnimacao.Tick += (s, e) => { if (this.Opacity < 1) this.Opacity += 0.05; else timerAnimacao.Stop(); };
        }

        // =============================================================
        //  PERMISSÕES (LISTA DE ADMINS)
 // =============================================================
        private void ConfigurarPermissoes()
        {
            List<string> listaAdmins = new List<string>
  {
   "rodridae",
     "daniel rodrigues",
         "matheus.machado",
 "dinomarks.carvalho",
    "admin"
        };

            bool isAdmin = listaAdmins.Contains(usuarioAtual.ToLower());

      btnEditar.Enabled = isAdmin;
            btnExcluir.Enabled = isAdmin;
          btnMecanicos.Enabled = isAdmin;
            btnNova.Enabled = isAdmin;
        }

      private async void Form1_Load(object sender, EventArgs e)
        {
            ConfigurarDashboard();
            ConfigurarAbaHistorico();
     timerAnimacao.Start();
       await CarregarTudo();
      }

      private async Task CarregarTudo()
        {
            lblTitulo.Text = "Sincronizando...";
            await CarregarDadosComCalibracao();
            await CarregarDadosSemCalibracao();
    AtualizarNumerosDashboard();
  lblTitulo.Text = "FERRAMENTARIA ONLINE";
        }

        // =============================================================
   //  FUNCIONALIDADE 1: PREVIEW DE FOTO (HOVER) - MELHORADO
   // =============================================================
     private void ConfigurarPreviewFoto()
        {
        previewBox = new PictureBox 
       { 
      Size = new Size(250, 250), 
      SizeMode = PictureBoxSizeMode.Zoom, 
          BackColor = Color.White, 
                BorderStyle = BorderStyle.FixedSingle, 
      Visible = false,
       Padding = new Padding(5)
  };
        this.Controls.Add(previewBox);
            previewBox.BringToFront();

        dgvDados.CellMouseEnter += Grid_CellMouseEnter;
dgvDados.CellMouseLeave += Grid_CellMouseLeave;
            dgvSemCalibracao.CellMouseEnter += Grid_CellMouseEnter;
    dgvSemCalibracao.CellMouseLeave += Grid_CellMouseLeave;
   }

        private void Grid_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
 {
         if (e.RowIndex < 0) return;
     DataGridView dgv = sender as DataGridView;
       string colName = dgv.Columns[e.ColumnIndex].Name;

      if (colName.Contains("Foto") && dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
            {
  string arq = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
              string caminho = Path.Combine(pastaDasImagens, arq);
         if (File.Exists(caminho))
   {
         try
          {
previewBox.Image?.Dispose(); // Libera imagem anterior
             previewBox.Image = Image.FromFile(caminho);
               
      // Posiciona preview sem sair da tela
          Point cursorPos = Cursor.Position;
               int x = cursorPos.X + 20;
         int y = cursorPos.Y + 20;

    // Garante que o preview não ultrapasse os limites da tela
       if (x + previewBox.Width > Screen.PrimaryScreen.WorkingArea.Right)
           x = cursorPos.X - previewBox.Width - 20;
       if (y + previewBox.Height > Screen.PrimaryScreen.WorkingArea.Bottom)
   y = cursorPos.Y - previewBox.Height - 20;
              
       previewBox.Location = new Point(Math.Max(0, x), Math.Max(0, y));
      previewBox.Visible = true;
           }
          catch { }
     }
      }
        }
        
        private void Grid_CellMouseLeave(object sender, DataGridViewCellEventArgs e) 
        { 
            previewBox.Visible = false;
        }

        // =============================================================
        //  FUNCIONALIDADE 2: ABA HISTÓRICO - MELHORADA
        // =============================================================
  private void ConfigurarAbaHistorico()
  {
     TabPage tabHist = new TabPage("HISTÓRICO DE MOVIMENTAÇÕES");
  tabHist.BackColor = Color.White;
          tabControlPrincipal.TabPages.Add(tabHist);

// Painel superior com botões
     Panel panelBotoes = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.WhiteSmoke };
tabHist.Controls.Add(panelBotoes);

        Button btnAtualizarHist = new Button 
            { 
        Text = "🔄 Atualizar Histórico", 
     Location = new Point(10, 10), 
     Size = new Size(150, 35),
     BackColor = Color.DodgerBlue,
    ForeColor = Color.White,
      Font = new Font("Segoe UI", 9, FontStyle.Bold)
     };
        
        Button btnExportarHist = new Button 
            { 
     Text = "📊 Exportar", 
     Location = new Point(170, 10), 
     Size = new Size(120, 35),
         BackColor = Color.Green,
            ForeColor = Color.White,
  Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

     panelBotoes.Controls.Add(btnAtualizarHist);
      panelBotoes.Controls.Add(btnExportarHist);

            dgvHist = new DataGridView
            {
           Dock = DockStyle.Fill,
    AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
                AllowUserToAddRows = false,
        RowHeadersVisible = false,
     BackgroundColor = Color.White,
 BorderStyle = BorderStyle.None,
EnableHeadersVisualStyles = false
      };
            
    dgvHist.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(64, 64, 64);
      dgvHist.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgvHist.ColumnHeadersHeight = 35;
            
   // Adiciona colunas ao histórico
   dgvHist.Columns.Add("colData", "DATA");
    dgvHist.Columns.Add("colAcao", "AÇÃO");
            dgvHist.Columns.Add("colFerramenta", "FERRAMENTA");
     dgvHist.Columns.Add("colMecanico", "MECÂNICO");
dgvHist.Columns.Add("colAeronave", "AERONAVE");
            dgvHist.Columns.Add("colAdmin", "ADMINISTRADOR");

      tabHist.Controls.Add(dgvHist);

            // Eventos dos botões
    btnAtualizarHist.Click += async (s, e) => {
     btnAtualizarHist.Enabled = false;
        btnAtualizarHist.Text = "⏳ Carregando...";
       try 
         { 
        var dados = await ApiService.GetHistorico();
         dgvHist.Rows.Clear();
        if (dados != null)
          {
 foreach (var item in dados)
             {
         dgvHist.Rows.Add(
  item.Data ?? "N/A",
             item.Acao ?? "N/A",
            item.Ferramenta ?? "N/A",
                 item.Mecanico ?? "N/A",
        item.Aeronave ?? "N/A",
                item.Admin ?? "N/A"
        );
 }
      }
             MessageBox.Show($"✓ {dgvHist.Rows.Count} registros carregados!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
      } 
           catch (Exception ex) 
        { 
   MessageBox.Show("✗ Erro ao carregar histórico: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
     finally
     {
        btnAtualizarHist.Enabled = true;
      btnAtualizarHist.Text = "🔄 Atualizar Histórico";
   }
          };

  btnExportarHist.Click += (s, e) => { ExportarHistoricoExcel(); };
   }

  // =============================================================
        //  FUNCIONALIDADE 3: EXPORTAR PARA EXCEL - MELHORADO
    // =============================================================
private void ExportarParaExcel()
      {
            DataGridView dgv = null;
     string nomeAba = "";
 
  if (tabControlPrincipal.SelectedTab == tabComCalibracao) 
      { 
   dgv = dgvDados;
        nomeAba = "Instrumentos_Com_Calibracao";
            }
            else if (tabControlPrincipal.SelectedTab == tabSemCalibracao) 
            { 
                dgv = dgvSemCalibracao;
        nomeAba = "Instrumentos_Sem_Calibracao";
      }
  else if (dgvHist != null && tabControlPrincipal.SelectedTab.Text.Contains("HISTÓRICO"))
       {
   dgv = dgvHist;
       nomeAba = "Historico_Movimentacoes";
            }
    else 
         { 
                MessageBox.Show("Selecione uma aba com tabela.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
     return;
            }

       using (SaveFileDialog sfd = new SaveFileDialog() 
    { 
     Filter = "Excel (*.xlsx)|*.xlsx|CSV (*.csv)|*.csv", 
    FileName = $"Relatorio_{nomeAba}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv" 
         })
 {
         if (sfd.ShowDialog() == DialogResult.OK)
     {
       try
         {
          using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
         {
        // Escreve cabeçalhos
 string h = "";
foreach (DataGridViewColumn c in dgv.Columns)
  h += "\"" + c.HeaderText + "\";";
          sw.WriteLine(h.TrimEnd(';'));

   // Escreve dados
     foreach (DataGridViewRow r in dgv.Rows)
            {
         if (r.IsNewRow) continue;
    string l = "";
         foreach (DataGridViewCell c in r.Cells)
  {
        string val = c.Value?.ToString() ?? "";
        val = val.Replace("\"", "\"\"").Replace(";", ",");
                l += "\"" + val + "\";";
       }
             sw.WriteLine(l.TrimEnd(';'));
        }
        }
      
         MessageBox.Show($"✓ Relatório exportado com sucesso!\n{dgv.Rows.Count} registros.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
      System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
          }
              catch (Exception ex) 
  { 
     MessageBox.Show("✗ Erro ao exportar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
}
       }
            }
        }

      private void ExportarHistoricoExcel()
        {
         if (dgvHist == null || dgvHist.Rows.Count == 0)
            {
     MessageBox.Show("Nenhum dado no histórico para exportar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
          return;
   }

        using (SaveFileDialog sfd = new SaveFileDialog() 
 { 
     Filter = "CSV (*.csv)|*.csv", 
       FileName = $"Historico_Movimentacoes_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.csv" 
  })
       {
        if (sfd.ShowDialog() == DialogResult.OK)
       {
      try
    {
    using (StreamWriter sw = new StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
          {
          string h = "";
          foreach (DataGridViewColumn c in dgvHist.Columns)
 h += "\"" + c.HeaderText + "\";";
         sw.WriteLine(h.TrimEnd(';'));

   foreach (DataGridViewRow r in dgvHist.Rows)
    {
          if (r.IsNewRow) continue;
    string l = "";
                foreach (DataGridViewCell c in r.Cells)
       {
     string val = c.Value?.ToString() ?? "";
       val = val.Replace("\"", "\"\"").Replace(";", ",");
     l += "\"" + val + "\";";
      }
            sw.WriteLine(l.TrimEnd(';'));
  }
            }
       
            MessageBox.Show($"✓ Histórico exportado!\n{dgvHist.Rows.Count} registros.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
     System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
    }
  catch (Exception ex) 
              { 
      MessageBox.Show("✗ Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
  }
      }
          }
        }

        // =============================================================
        //  DASHBOARD (VISUAL)
        // =============================================================
        private void ConfigurarDashboard()
     {
          TabPage tab = new TabPage("VISÃO GERAL") { BackColor = Color.White };
            if (tabControlPrincipal.TabPages.Count > 0) tabControlPrincipal.TabPages.Insert(0, tab); else tabControlPrincipal.TabPages.Add(tab);
       tabControlPrincipal.SelectedIndex = 0;

     tab.Controls.Add(new Label { Text = "Resumo Geral", Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20), ForeColor = Color.DimGray });

 lblDashTotal = CriarCard(tab, "Total", Color.RoyalBlue, 20, 70);
     lblDashVencidos = CriarCard(tab, "Vencidos", Color.Firebrick, 240, 70);
  lblDashAVencer = CriarCard(tab, "A Vencer (30d)", Color.DarkOrange, 460, 70);
   lblDashEmprestados = CriarCard(tab, "Emprestados", Color.SeaGreen, 680, 70);

            Button btnRef = new Button { Text = "🔄 Atualizar", Size = new Size(120, 35), Location = new Point(20, 180), BackColor = Color.DodgerBlue, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
     btnRef.Click += (s, e) => { _ = CarregarTudo(); };
            tab.Controls.Add(btnRef);
        }

        private Label CriarCard(TabPage t, string tit, Color c, int x, int y)
        {
   Panel p = new Panel { Size = new Size(200, 100), Location = new Point(x, y), BackColor = c };
            p.Controls.Add(new Label { Text = tit, ForeColor = Color.White, Font = new Font("Segoe UI", 9, FontStyle.Bold), Location = new Point(10, 10), AutoSize = true });
   Label v = new Label { Text = "...", ForeColor = Color.White, Font = new Font("Segoe UI", 24, FontStyle.Bold), Location = new Point(10, 35), AutoSize = true };
      p.Controls.Add(v); t.Controls.Add(p); return v;
  }

        private void AtualizarNumerosDashboard()
   {
       if (dgvDados == null || dgvDados.Rows.Count == 0) return;
            int tot = dgvDados.Rows.Count, venc = 0, avenc = 0, emp = 0;
            foreach (DataGridViewRow r in dgvDados.Rows)
            {
if (DateTime.TryParseExact(r.Cells["colDataVencimento"].Value?.ToString(), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime d))
                {
      int x = (int)(d.Date - DateTime.Today).TotalDays; if (x <= 0) venc++; else if (x <= 30) avenc++;
           }
          if (!string.IsNullOrEmpty(r.Cells["colMecanico"].Value?.ToString())) emp++;
            }
      if (lblDashTotal != null) lblDashTotal.Text = tot.ToString(); if (lblDashVencidos != null) lblDashVencidos.Text = venc.ToString();
  if (lblDashAVencer != null) lblDashAVencer.Text = avenc.ToString(); if (lblDashEmprestados != null) lblDashEmprestados.Text = emp.ToString();
    }

    // =============================================================
  //  ARQUIVOS E CACHE
        // =============================================================
        private string SalvarArquivoLocal(byte[] b, string p, string e) { if (b == null || b.Length == 0) return ""; string n = Guid.NewGuid().ToString().Substring(0, 10) + e; try { File.WriteAllBytes(Path.Combine(p, n), b); return n; } catch { return ""; } }
        private byte[] LerArquivoLocal(string n, string p) { if (string.IsNullOrEmpty(n)) return null; string c = Path.Combine(p, n); return File.Exists(c) ? File.ReadAllBytes(c) : null; }

    // =============================================================
        //  CARREGAMENTO DE DADOS (API)
        // =============================================================
        private async Task CarregarDadosComCalibracao()
  {
       dgvDados.Rows.Clear(); 
     List<string> av = new List<string>();
     try
{
var l = await ApiService.GetCom(); 
    foreach (var r in l)
    {
    string f = SalvarArquivoLocal(r.Foto, pastaDasImagens, ".jpg"), 
         p = SalvarArquivoLocal(r.CertificadoPDF, pastaDosCertificados, ".pdf");
          dgvDados.Rows.Add(r.InstrumentoNome, r.Modelo, r.PN, r.SN, r.IdentifSOD, r.IdentifOficina, r.Certificado, r.DataCalibracao, r.DataVencimento, null, r.Executante, r.Instalada, r.Local, r.SubLocalizacao, f, r.Observacoes, r.Mecanico, p);
    dgvDados.Rows[dgvDados.Rows.Count - 1].Tag = r.ID;
   if (DateTime.TryParseExact(r.DataVencimento, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime d))
   {
       int dias = (int)(d.Date - DateTime.Today).TotalDays;
   if (dias > 0 && dias <= 45) av.Add($"{r.InstrumentoNome} ({dias} dias)");
   }
     }
   if (av.Count > 0 && tabControlPrincipal.SelectedTab == tabComCalibracao) 
     MessageBox.Show("⚠️ Atenção! Vencimentos próximos:\n" + string.Join("\n", av), "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
 }
            catch (Exception ex) 
{ 
   MessageBox.Show($"❌ Erro ao carregar instrumentos: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); 
}
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
  catch (Exception ex) 
  { 
MessageBox.Show($"❌ Erro ao carregar ferramentas: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); 
 }
     }

        // =============================================================
        //  BOTÕES CRUD
        // =============================================================
        private async void btnNova_Click(object sender, EventArgs e)
        {
  if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
     var f = new FormItemNovo(); if (f.ShowDialog() == DialogResult.OK)
           {
     object[] d = f.NovoItemDados;

    // ✅ VALIDAÇÃO: Verificar campos obrigatórios
      if (!ValidarCamposInstrumento(d))
    {
 MessageBox.Show("Campos obrigatórios não preenchidos!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
  return;
 }

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
     try 
  { 
     await ApiService.PostCom(i); 
     MessageBox.Show("✅ Instrumento adicionado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
 await CarregarTudo(); 
 } 
     catch (Exception ex) 
 { 
   MessageBox.Show($"❌ Erro ao adicionar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); 
}
 }
   }
     else if (tabControlPrincipal.SelectedTab == tabSemCalibracao)
       {
    var f = new FormItemSemCalibracao(); 
     if (f.ShowDialog() == DialogResult.OK)
       {
        object[] d = f.ItemDados;

      // ✅ VALIDAÇÃO: Verificar campos obrigatórios
   if (!ValidarCamposFerramentaSemCalibracao(d))
      {
MessageBox.Show("Campos obrigatórios não preenchidos!", "Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

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
  try 
   { 
await ApiService.PostSem(i); 
   MessageBox.Show("✅ Ferramenta adicionada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
       await CarregarTudo(); 
       } 
             catch (Exception ex) 
   { 
     MessageBox.Show($"❌ Erro ao adicionar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error); 
 }
 }
        }
      }

   // ✅ MÉTODOS DE VALIDAÇÃO
 private bool ValidarCamposInstrumento(object[] dados)
  {
          // Valida campos obrigatórios (índices 0, 1 = Nome e Modelo)
   if (dados == null || dados.Length < 10) return false;
     if (string.IsNullOrWhiteSpace(dados[0]?.ToString())) return false; // InstrumentoNome
        if (string.IsNullOrWhiteSpace(dados[1]?.ToString())) return false; // Modelo
      return true;
   }

 private bool ValidarCamposFerramentaSemCalibracao(object[] dados)
    {
     // Valida campos obrigatórios
  if (dados == null || dados.Length < 2) return false;
   if (string.IsNullOrWhiteSpace(dados[0]?.ToString())) return false; // Descrição
        if (string.IsNullOrWhiteSpace(dados[1]?.ToString())) return false; // Código
      return true;
    }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao && dgvDados.SelectedRows.Count > 0)
     {
    var r = dgvDados.SelectedRows[0]; var f = new FormItemEditar(r); if (f.ShowDialog() == DialogResult.OK)
        {
    object[] d = f.ItemEditadoDados; var i = new ApiService.Instrumento { ID = Convert.ToInt32(r.Tag), InstrumentoNome = d[0]?.ToString(), Modelo = d[1]?.ToString(), PN = d[2]?.ToString(), SN = d[3]?.ToString(), IdentifSOD = d[4]?.ToString(), IdentifOficina = d[5]?.ToString(), Certificado = d[6]?.ToString(), DataCalibracao = ((DateTime)d[7]).ToString("dd/MM/yyyy"), DataVencimento = ((DateTime)d[8]).ToString("dd/MM/yyyy"), Executante = d[10]?.ToString(), Instalada = d[11]?.ToString(), Local = d[12]?.ToString(), SubLocalizacao = d[13]?.ToString(), Foto = LerArquivoLocal(d[14]?.ToString(), pastaDasImagens), Observacoes = d[15]?.ToString(), Mecanico = d[16]?.ToString(), CertificadoPDF = LerArquivoLocal(d[17]?.ToString(), pastaDosCertificados) };
         try { await ApiService.PutCom(i.ID, i); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
     }
         }
         else if (tabControlPrincipal.SelectedTab == tabSemCalibracao && dgvSemCalibracao.SelectedRows.Count > 0)
      {
     var r = dgvSemCalibracao.SelectedRows[0]; var f = new FormItemSemCalibracao(r); if (f.ShowDialog() == DialogResult.OK)
                {
              object[] d = f.ItemDados; var i = new ApiService.SemCalibracao { ID = Convert.ToInt32(r.Tag), Descricao = d[0]?.ToString(), Codigo = d[1]?.ToString(), PN = d[2]?.ToString(), Fabricante = d[3]?.ToString(), Local = d[4]?.ToString(), CadastroLocal = d[5]?.ToString(), CodLocal = d[6]?.ToString(), Status = d[7]?.ToString(), Mecanico = d[8]?.ToString(), Foto = LerArquivoLocal(d[9]?.ToString(), pastaDasImagens), CertificadoPDF = LerArquivoLocal(d[10]?.ToString(), pastaDosCertificados) };
      try { await ApiService.PutSem(i.ID, i); await CarregarTudo(); } catch (Exception ex) { MessageBox.Show(ex.Message); }
     }
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Excluir?", "Confirma", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
   try
            {
         if (tabControlPrincipal.SelectedTab == tabComCalibracao && dgvDados.SelectedRows.Count > 0) await ApiService.DelCom(Convert.ToInt32(dgvDados.SelectedRows[0].Tag));
                else if (tabControlPrincipal.SelectedTab == tabSemCalibracao && dgvSemCalibracao.SelectedRows.Count > 0) await ApiService.DelSem(Convert.ToInt32(dgvSemCalibracao.SelectedRows[0].Tag));
   await CarregarTudo();
       }
    catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // --- NAVEGAÇÃO E OUTROS ---
        private void btnMovimentacao_Click(object sender, EventArgs e) { new FormMovimentacao().ShowDialog(); _ = CarregarTudo(); }
        private void btnMecanicos_Click(object sender, EventArgs e) { new FormMecanicos().ShowDialog(); }
        private void btnRelatorios_Click(object sender, EventArgs e) { ExportarParaExcel(); }
        private void btnBuscaAvancada_Click(object sender, EventArgs e) { }
        private void btnFiltrar_Click(object sender, EventArgs e) { }
        private void btnInstrumentos_Click(object sender, EventArgs e) { if (tabControlPrincipal.TabCount > 1) tabControlPrincipal.SelectedIndex = 1; }

        private void btnProcurar_Click(object sender, EventArgs e)
      {
      txtBusca.Visible = !txtBusca.Visible; lblBusca.Visible = !lblBusca.Visible;
            if (txtBusca.Visible) txtBusca.Focus(); else txtBusca.Text = "";
     }

        // =============================================================
        //  FUNCIONALIDADE 4: FILTROS AVANÇADOS - MELHORADO
        // =============================================================
   private void txtBusca_TextChanged(object sender, EventArgs e)
        {
    string termo = txtBusca.Text.ToLower().Trim();
     if (tabControlPrincipal.SelectedIndex == 0) return; // Ignora Dashboard
          
            DataGridView dgv = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? dgvDados : dgvSemCalibracao;
       
            dgv.SuspendLayout();
    int linhasVisiveis = 0;
  
  foreach (DataGridViewRow r in dgv.Rows)
      {
    if (r.IsNewRow) continue;
  
           bool achou = string.IsNullOrEmpty(termo); // Se vazio, mostra tudo
     
     if (!achou)
     {
       foreach (DataGridViewCell c in r.Cells)
   {
         if (c.Value != null)
  {
   string cellValue = c.Value.ToString().ToLower();
         if (cellValue.Contains(termo))
         {
     achou = true;
      break;
    }
     }
        }
   }
    
  r.Visible = achou;
       if (achou) linhasVisiveis++;
  }
            
  dgv.ResumeLayout();
  
            // Atualiza label com quantidade de resultados
   lblBusca.Text = $"Procurar: ({linhasVisiveis}/{dgv.RowCount} resultados)";
    }

        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return; 
       if (dgvDados.Columns[e.ColumnIndex].Name == "colSituacao")
  {
       if (DateTime.TryParseExact(dgvDados.Rows[e.RowIndex].Cells["colDataVencimento"].Value?.ToString(), "dd/MM/yyyy", null, DateTimeStyles.None, out DateTime d))
      {
     int x = (int)(d.Date - DateTime.Today).TotalDays;
         if (x <= 0) 
    { 
     e.Value = "⚠️ VENCIDO"; 
                  e.CellStyle.BackColor = Color.Red; 
       e.CellStyle.ForeColor = Color.White; 
       } 
               else if (x <= 45) 
       { 
         e.Value = "⏰ CALIBRAR"; 
        e.CellStyle.BackColor = Color.Yellow; 
    e.CellStyle.ForeColor = Color.Black;
                } 
           else 
  { 
          e.Value = "✓ CALIBRADO"; 
   e.CellStyle.BackColor = Color.LightGreen;
           e.CellStyle.ForeColor = Color.Black;
    }
  e.FormattingApplied = true;
                }
      else 
  { 
             e.Value = "N/A"; 
  e.CellStyle.BackColor = Color.LightGray; 
    }
        }
        }

        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) new FormDetalhes(dgvDados.Rows[e.RowIndex]).ShowDialog(); }
        private void dgvSemCalibracao_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { if (e.RowIndex >= 0) btnEditar_Click(sender, e); }
    }
}