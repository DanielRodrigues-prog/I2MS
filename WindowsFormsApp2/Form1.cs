using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string usuarioAtual;
        private string pastaDasImagens;
        private string pastaDosCertificados;

        public Form1(string usuarioLogado)
        {
            InitializeComponent();

            this.usuarioAtual = usuarioLogado;
            this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual}";

            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            this.pastaDasImagens = Path.Combine(baseDir, "Imagens");
            this.pastaDosCertificados = Path.Combine(baseDir, "Certificados");

            // Garante que o banco de dados existe
            Database.InitializeDatabase();

            // Garante que as pastas existem
            if (!Directory.Exists(this.pastaDasImagens))
            {
                try { Directory.CreateDirectory(this.pastaDasImagens); }
                catch (Exception ex) { MessageBox.Show("Erro ao criar pasta 'Imagens': " + ex.Message); }
            }
            if (!Directory.Exists(this.pastaDosCertificados))
            {
                try { Directory.CreateDirectory(this.pastaDosCertificados); }
                catch (Exception ex) { MessageBox.Show("Erro ao criar pasta 'Certificados': " + ex.Message); }
            }

            // Lógica de Permissão
            if (this.usuarioAtual.ToLower() == "rodridae")
            {
                btnEditar.Enabled = true;
                btnExcluir.Enabled = true;
                btnMecanicos.Enabled = true;
                btnMovimentacao.Enabled = true;
                btnNova.Enabled = true; // Admin cria em ambas
            }
            else
            {
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                btnMecanicos.Enabled = false;
                btnMovimentacao.Enabled = true; // Todos movimentam
                btnNova.Enabled = false; // Usuário padrão não cria
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarDadosComCalibracao();
            CarregarDadosSemCalibracao();
        }

        // --- CARREGAMENTO DOS DADOS ---

        private void CarregarDadosComCalibracao()
        {
            dgvDados.Rows.Clear();
            List<string> proximosAVencer = new List<string>();

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM Instrumentos";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            dgvDados.Rows.Add(new object[] {
                                r["Instrumento"], r["Modelo"], r["PN"], r["SN"],
                                r["IdentifSOD"], r["IdentifOficina"], r["Certificado"],
                                r["DataCalibracao"], r["DataVencimento"],
                                null, // Situação calculada depois
                                r["Executante"], r["Instalada"], r["Local"],
                                r["SubLocalizacao"], r["Foto"], r["Observacoes"],
                                r["Mecanico"], r["CertificadoPDF"]
                            });

                            dgvDados.Rows[dgvDados.Rows.Count - 1].Tag = r["ID"];

                            // Lógica de Vencimento
                            string dataVencTexto = r["DataVencimento"].ToString();
                            if (DateTime.TryParseExact(dataVencTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataVenc))
                            {
                                int dias = (int)(dataVenc.Date - DateTime.Today).TotalDays;
                                if (dias > 0 && dias <= 45)
                                    proximosAVencer.Add($"{r["Instrumento"]} (Vence em {dias} dias)");
                            }
                        }
                    }
                }

                if (proximosAVencer.Count > 0 && tabControlPrincipal.SelectedTab == tabComCalibracao)
                {
                    string msg = "Itens próximos do vencimento:\n\n" + string.Join("\n", proximosAVencer);
                    MessageBox.Show(msg, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar dados calibráveis: " + ex.Message); }
        }

        private void CarregarDadosSemCalibracao()
        {
            dgvSemCalibracao.Rows.Clear();
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT * FROM FerramentasSemCalibracao";

                    using (var cmd = new SQLiteCommand(sql, conn))
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            dgvSemCalibracao.Rows.Add(new object[] {
                                r["Descricao"], r["Codigo"], r["PN"],
                                r["Fabricante"], r["Local"], r["CadastroLocal"],
                                r["CodLocal"], r["Status"], r["Mecanico"]
                            });
                            // Guardamos o ID na Tag
                            dgvSemCalibracao.Rows[dgvSemCalibracao.Rows.Count - 1].Tag = r["ID"];
                            // Guardamos campos ocultos (Foto e PDF) na Tag também ou em células ocultas se preferir.
                            // Para simplificar a edição, vamos buscar do banco novamente no formulário de edição usando o ID.
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar dados sem calibração: " + ex.Message); }
        }

        // --- BOTÕES ---

        private void btnNova_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                // ABA 1: COM CALIBRAÇÃO
                FormItemNovo formulario = new FormItemNovo();
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conn = Database.GetConnection())
                        {
                            conn.Open();
                            string sql = @"INSERT INTO Instrumentos (
                                            Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, 
                                            Certificado, DataCalibracao, DataVencimento, 
                                            Executante, Instalada, Local, SubLocalizacao, 
                                            Foto, Observacoes, Mecanico, CertificadoPDF
                                        ) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17)";

                            using (var cmd = new SQLiteCommand(sql, conn))
                            {
                                object[] d = formulario.NovoItemDados;
                                cmd.Parameters.AddWithValue("@p0", d[0]); cmd.Parameters.AddWithValue("@p1", d[1]);
                                cmd.Parameters.AddWithValue("@p2", d[2]); cmd.Parameters.AddWithValue("@p3", d[3]);
                                cmd.Parameters.AddWithValue("@p4", d[4]); cmd.Parameters.AddWithValue("@p5", d[5]);
                                cmd.Parameters.AddWithValue("@p6", d[6]);
                                cmd.Parameters.AddWithValue("@p7", ((DateTime)d[7]).ToString("dd/MM/yyyy"));
                                cmd.Parameters.AddWithValue("@p8", ((DateTime)d[8]).ToString("dd/MM/yyyy"));
                                cmd.Parameters.AddWithValue("@p10", d[10]); cmd.Parameters.AddWithValue("@p11", d[11]);
                                cmd.Parameters.AddWithValue("@p12", d[12]); cmd.Parameters.AddWithValue("@p13", d[13]);
                                cmd.Parameters.AddWithValue("@p14", d[14]); cmd.Parameters.AddWithValue("@p15", d[15]);
                                cmd.Parameters.AddWithValue("@p16", d[16]); cmd.Parameters.AddWithValue("@p17", d[17]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        CarregarDadosComCalibracao();
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao salvar: " + ex.Message); }
                }
            }
            else
            {
                // ABA 2: SEM CALIBRAÇÃO (NOVO)
                FormItemSemCalibracao formulario = new FormItemSemCalibracao();
                if (formulario.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conn = Database.GetConnection())
                        {
                            conn.Open();
                            string sql = @"INSERT INTO FerramentasSemCalibracao (
                                            Descricao, Codigo, PN, Fabricante, Local, CadastroLocal, 
                                            CodLocal, Status, Mecanico, Foto, CertificadoPDF
                                           ) VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10)";

                            using (var cmd = new SQLiteCommand(sql, conn))
                            {
                                object[] d = formulario.ItemDados;
                                // d[0] a d[7] são os campos visíveis, d[8] mecanico, d[9] foto, d[10] pdf
                                for (int i = 0; i <= 10; i++)
                                {
                                    // Nota: no form sem calibração o ID é o ultimo elemento, ignoramos aqui no insert
                                    if (i < d.Length - 1)
                                        cmd.Parameters.AddWithValue($"@p{i}", d[i]);
                                }
                                cmd.ExecuteNonQuery();
                            }
                        }
                        CarregarDadosSemCalibracao();
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao salvar sem calibração: " + ex.Message); }
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (tabControlPrincipal.SelectedTab == tabComCalibracao)
            {
                // EDITAR COM CALIBRAÇÃO
                if (dgvDados.SelectedRows.Count > 0)
                {
                    DataGridViewRow linha = dgvDados.SelectedRows[0];
                    FormItemEditar form = new FormItemEditar(linha);
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (var conn = Database.GetConnection())
                            {
                                conn.Open();
                                string sql = @"UPDATE Instrumentos SET
                                                Instrumento=@p0, Modelo=@p1, PN=@p2, SN=@p3, IdentifSOD=@p4, IdentifOficina=@p5, 
                                                Certificado=@p6, DataCalibracao=@p7, DataVencimento=@p8, Executante=@p10, 
                                                Instalada=@p11, Local=@p12, SubLocalizacao=@p13, Foto=@p14, Observacoes=@p15, 
                                                Mecanico=@p16, CertificadoPDF=@p17 WHERE ID=@id";
                                using (var cmd = new SQLiteCommand(sql, conn))
                                {
                                    object[] d = form.ItemEditadoDados;
                                    cmd.Parameters.AddWithValue("@p0", d[0]); cmd.Parameters.AddWithValue("@p1", d[1]);
                                    cmd.Parameters.AddWithValue("@p2", d[2]); cmd.Parameters.AddWithValue("@p3", d[3]);
                                    cmd.Parameters.AddWithValue("@p4", d[4]); cmd.Parameters.AddWithValue("@p5", d[5]);
                                    cmd.Parameters.AddWithValue("@p6", d[6]);
                                    cmd.Parameters.AddWithValue("@p7", ((DateTime)d[7]).ToString("dd/MM/yyyy"));
                                    cmd.Parameters.AddWithValue("@p8", ((DateTime)d[8]).ToString("dd/MM/yyyy"));
                                    cmd.Parameters.AddWithValue("@p10", d[10]); cmd.Parameters.AddWithValue("@p11", d[11]);
                                    cmd.Parameters.AddWithValue("@p12", d[12]); cmd.Parameters.AddWithValue("@p13", d[13]);
                                    cmd.Parameters.AddWithValue("@p14", d[14]); cmd.Parameters.AddWithValue("@p15", d[15]);
                                    cmd.Parameters.AddWithValue("@p16", d[16]); cmd.Parameters.AddWithValue("@p17", d[17]);
                                    cmd.Parameters.AddWithValue("@id", linha.Tag);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            CarregarDadosComCalibracao();
                            MessageBox.Show("Item atualizado.");
                        }
                        catch (Exception ex) { MessageBox.Show("Erro ao editar: " + ex.Message); }
                    }
                }
                else { MessageBox.Show("Selecione uma linha para editar."); }
            }
            else
            {
                // EDITAR SEM CALIBRAÇÃO
                if (dgvSemCalibracao.SelectedRows.Count > 0)
                {
                    DataGridViewRow linha = dgvSemCalibracao.SelectedRows[0];
                    // Precisamos buscar os dados completos do banco (incluindo foto e pdf que não estão na grid)
                    long id = Convert.ToInt64(linha.Tag);

                    // Vamos criar um "mock" de linha com todos os dados para passar pro formulário
                    // Na prática ideal, o Form receberia o ID e buscaria do banco, mas vamos adaptar o DataGridViewRow
                    // Para simplificar, passamos a linha da grid e no FormItemSemCalibracao buscamos o resto se precisar
                    // Mas aqui, vamos buscar os extras e adicionar à linha temporariamente ou passar direto.

                    // A melhor forma com a estrutura atual é passar a linha, e dentro do FormItemSemCalibracao
                    // fazer uma consulta pelo ID (Tag) para pegar Foto e PDF.
                    // Vou assumir que você atualizou o FormItemSemCalibracao como combinamos.

                    FormItemSemCalibracao form = new FormItemSemCalibracao(linha); // Passa a linha, o form busca o resto pelo ID na Tag
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (var conn = Database.GetConnection())
                            {
                                conn.Open();
                                string sql = @"UPDATE FerramentasSemCalibracao SET
                                                Descricao=@p0, Codigo=@p1, PN=@p2, Fabricante=@p3, 
                                                Local=@p4, CadastroLocal=@p5, CodLocal=@p6, Status=@p7, 
                                                Mecanico=@p8, Foto=@p9, CertificadoPDF=@p10
                                               WHERE ID=@id";

                                using (var cmd = new SQLiteCommand(sql, conn))
                                {
                                    object[] d = form.ItemDados;
                                    for (int i = 0; i <= 10; i++) cmd.Parameters.AddWithValue($"@p{i}", d[i]);
                                    cmd.Parameters.AddWithValue("@id", id);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                            CarregarDadosSemCalibracao();
                            MessageBox.Show("Item atualizado.");
                        }
                        catch (Exception ex) { MessageBox.Show("Erro ao editar sem calibração: " + ex.Message); }
                    }
                }
                else { MessageBox.Show("Selecione uma linha para editar."); }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DataGridView dgvAtivo = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? dgvDados : dgvSemCalibracao;
            string tabelaSql = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? "Instrumentos" : "FerramentasSemCalibracao";

            if (dgvAtivo.SelectedRows.Count > 0)
            {
                var confirmacao = MessageBox.Show("Tem certeza que deseja excluir?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        object idParaExcluir = dgvAtivo.SelectedRows[0].Tag;
                        using (var conn = Database.GetConnection())
                        {
                            conn.Open();
                            string sql = $"DELETE FROM {tabelaSql} WHERE ID = @id";
                            using (var cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@id", idParaExcluir);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        if (tabelaSql == "Instrumentos") CarregarDadosComCalibracao();
                        else CarregarDadosSemCalibracao();

                        MessageBox.Show("Item excluído.");
                    }
                    catch (Exception ex) { MessageBox.Show("Erro ao excluir: " + ex.Message); }
                }
            }
            else
            {
                MessageBox.Show("Selecione uma linha para excluir.");
            }
        }

        private void btnMovimentacao_Click(object sender, EventArgs e)
        {
            FormMovimentacao form = new FormMovimentacao();
            form.ShowDialog();
            // Recarrega ambas para atualizar o campo "Mecânico"
            CarregarDadosComCalibracao();
            CarregarDadosSemCalibracao();
        }

        private void btnMecanicos_Click(object sender, EventArgs e)
        {
            FormMecanicos form = new FormMecanicos();
            form.ShowDialog();
        }

        // --- BUSCA ---
        private void btnProcurar_Click(object sender, EventArgs e)
        {
            bool novoEstado = !txtBusca.Visible;
            txtBusca.Visible = novoEstado;
            lblBusca.Visible = novoEstado;
            if (novoEstado) txtBusca.Focus();
            else txtBusca.Text = "";
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            string termo = txtBusca.Text.ToLower();
            DataGridView dgvAtivo = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? dgvDados : dgvSemCalibracao;
            string colunaBusca = (tabControlPrincipal.SelectedTab == tabComCalibracao) ? "colInstrumento" : "colSemDescricao";

            foreach (DataGridViewRow linha in dgvAtivo.Rows)
            {
                if (linha.IsNewRow) continue;
                var celula = linha.Cells[colunaBusca].Value;
                if (celula != null)
                {
                    linha.Visible = celula.ToString().ToLower().Contains(termo);
                }
                else
                {
                    linha.Visible = false;
                }
            }
        }

        // --- FORMATAÇÃO DA SITUAÇÃO (Aba 1) ---
        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (dgvDados.Columns[e.ColumnIndex].Name == "colSituacao")
            {
                var celulaData = dgvDados.Rows[e.RowIndex].Cells["colDataVencimento"];
                DateTime dataVencimento = DateTime.MinValue;
                bool dataValida = false;

                if (celulaData.Value is DateTime)
                {
                    dataVencimento = (DateTime)celulaData.Value;
                    dataValida = true;
                }
                else if (celulaData.Value != null)
                {
                    string[] formatos = { "dd/MM/yyyy HH:mm", "dd/MM/yyyy" };
                    dataValida = DateTime.TryParseExact(celulaData.Value.ToString().Trim(), formatos, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataVencimento);
                }

                if (dataValida)
                {
                    int dias = (int)(dataVencimento.Date - DateTime.Today).TotalDays;
                    if (dias > 45)
                    {
                        e.Value = "CALIBRADO"; e.CellStyle.BackColor = Color.LightGreen; e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (dias <= 0)
                    {
                        e.Value = "VENCIDO"; e.CellStyle.BackColor = Color.Red; e.CellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        e.Value = "CALIBRAR"; e.CellStyle.BackColor = Color.Yellow; e.CellStyle.ForeColor = Color.Black;
                    }
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "N/A"; e.CellStyle.BackColor = Color.LightGray; e.CellStyle.ForeColor = Color.DarkGray;
                    e.FormattingApplied = true;
                }
            }
        }

        // --- DETALHES (Duplo Clique) ---
        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow linha = dgvDados.Rows[e.RowIndex];
            FormDetalhes form = new FormDetalhes(linha);
            form.ShowDialog();
        }

        // Adicionei o duplo clique para a aba sem calibração também, caso queira ver a foto maior
        private void btnSemCalibracao_Click(object sender, EventArgs e)
        {
            // Este botão foi removido do designer, mas mantenho a função vazia para não quebrar caso ainda tenha referência
        }
    }
}