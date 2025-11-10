using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.SQLite; // Adiciona o "tradutor" do SQLite

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string usuarioAtual;
        private string caminhoDoBanco; // MUDANÇA: Agora vamos usar este
        private string pastaDasImagens;
        private string pastaDosCertificados;

        public Form1(string usuarioLogado)
        {
            InitializeComponent();

            this.usuarioAtual = usuarioLogado;
            this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual}";

            string pastaDoPrograma = AppDomain.CurrentDomain.BaseDirectory;
            this.caminhoDoBanco = Path.Combine(pastaDoPrograma, "calibracao.db"); // MUDANÇA
            this.pastaDasImagens = Path.Combine(pastaDoPrograma, "Imagens");
            this.pastaDosCertificados = Path.Combine(pastaDoPrograma, "Certificados");

            // --- ESTA É A LINHA QUE FALTAVA ---
            Database.InitializeDatabase(); // Chama a função da classe Database.cs
            // ----------------------------------

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

            if (this.usuarioAtual.ToLower() == "rodridae")
            {
                btnEditar.Enabled = true;
                btnExcluir.Enabled = true;
            }
            else
            {
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarDadosDoBanco(); // MUDANÇA: Agora lê do banco de dados
        }

        private void CarregarDadosDoBanco()
        {
            dgvDados.Rows.Clear(); // Limpa a grade antes de carregar

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    // O comando SQL agora pega todas as colunas da nova tabela
                    string sql = "SELECT Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, Certificado, DataCalibracao, DataVencimento, Situacao, Executante, Instalada, Local, SubLocalizacao, Foto, Observacoes, Mecanico, CertificadoPDF FROM Instrumentos";

                    using (var comando = new SQLiteCommand(sql, conexao))
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvDados.Rows.Add(new object[] {
                                reader["Instrumento"], reader["Modelo"], reader["PN"], reader["SN"],
                                reader["IdentifSOD"], reader["IdentifOficina"], reader["Certificado"],
                                reader["DataCalibracao"], reader["DataVencimento"],
                                null, // Situação (índice 9) será calculada pelo CellFormatting
                                reader["Executante"], reader["Instalada"], reader["Local"],
                                reader["SubLocalizacao"], reader["Foto"], reader["Observacoes"],
                                reader["Mecanico"], reader["CertificadoPDF"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do banco de dados: " + ex.Message);
            }
        }

        private void btnNova_Click(object sender, EventArgs e)
        {
            FormItemNovo formularioDeCadastro = new FormItemNovo();
            if (formularioDeCadastro.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (var conexao = Database.GetConnection())
                    {
                        conexao.Open();
                        string sql = @"INSERT INTO Instrumentos (
                                        Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, 
                                        Certificado, DataCalibracao, DataVencimento, 
                                        Executante, Instalada, Local, SubLocalizacao, 
                                        Foto, Observacoes, Mecanico, CertificadoPDF
                                    ) VALUES (
                                        @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p10, 
                                        @p11, @p12, @p13, @p14, @p15, @p16, @p17
                                    )"; // Note que pulamos o @p9 (Situação)

                        using (var comando = new SQLiteCommand(sql, conexao))
                        {
                            object[] dados = formularioDeCadastro.NovoItemDados;
                            comando.Parameters.AddWithValue("@p0", dados[0]);
                            comando.Parameters.AddWithValue("@p1", dados[1]);
                            comando.Parameters.AddWithValue("@p2", dados[2]);
                            comando.Parameters.AddWithValue("@p3", dados[3]);
                            comando.Parameters.AddWithValue("@p4", dados[4]);
                            comando.Parameters.AddWithValue("@p5", dados[5]);
                            comando.Parameters.AddWithValue("@p6", dados[6]);
                            comando.Parameters.AddWithValue("@p7", ((DateTime)dados[7]).ToString("dd/MM/yyyy")); // Data
                            comando.Parameters.AddWithValue("@p8", ((DateTime)dados[8]).ToString("dd/MM/yyyy")); // Data
                            // Pulamos o índice 9 (Situação)
                            comando.Parameters.AddWithValue("@p10", dados[10]);
                            comando.Parameters.AddWithValue("@p11", dados[11]);
                            comando.Parameters.AddWithValue("@p12", dados[12]);
                            comando.Parameters.AddWithValue("@p13", dados[13]);
                            comando.Parameters.AddWithValue("@p14", dados[14]);
                            comando.Parameters.AddWithValue("@p15", dados[15]);
                            comando.Parameters.AddWithValue("@p16", dados[16]);
                            comando.Parameters.AddWithValue("@p17", dados[17]);

                            comando.ExecuteNonQuery();
                        }
                    }
                    CarregarDadosDoBanco(); // Recarrega a grade com os dados do banco
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar no banco de dados: " + ex.Message);
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A função Editar precisa ser atualizada para o banco de dados.");
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A função Excluir precisa ser atualizada para o banco de dados.");
        }

        // A função SalvarDadosNoCsv() NÃO É MAIS USADA.
        // private void SalvarDadosNoCsv() { ... } 

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            bool novoEstadoVisivel = !txtBusca.Visible;
            txtBusca.Visible = novoEstadoVisivel;
            lblBusca.Visible = novoEstadoVisivel;
            if (novoEstadoVisivel)
                txtBusca.Focus();
            else
                txtBusca.Text = "";
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            string termoBusca = txtBusca.Text.ToLower();
            foreach (DataGridViewRow linha in dgvDados.Rows)
            {
                if (linha.IsNewRow) continue;
                var celula = linha.Cells["colInstrumento"].Value;
                if (celula != null)
                {
                    if (celula.ToString().ToLower().Contains(termoBusca))
                        linha.Visible = true;
                    else
                        linha.Visible = false;
                }
                else
                {
                    linha.Visible = false;
                }
            }
        }

        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (this.dgvDados.Columns[e.ColumnIndex].Name == "colSituacao")
            {
                var celulaData = this.dgvDados.Rows[e.RowIndex].Cells["colDataVencimento"];
                DateTime dataVencimento;
                bool dataValida = false;

                if (celulaData.Value is DateTime)
                {
                    dataVencimento = (DateTime)celulaData.Value;
                    dataValida = true;
                }
                else if (celulaData.Value != null)
                {
                    string[] formatosDeData = { "dd/MM/yyyy HH:mm", "dd/MM/yyyy" };
                    dataValida = DateTime.TryParseExact(celulaData.Value.ToString().Trim(), formatosDeData, CultureInfo.InvariantCulture, DateTimeStyles.None, out dataVencimento);
                }
                else
                {
                    dataVencimento = DateTime.MinValue;
                    dataValida = false;
                }

                if (dataValida)
                {
                    int diasAVencer = (int)(dataVencimento.Date - DateTime.Today).TotalDays;
                    string situacao;
                    if (diasAVencer > 45)
                    {
                        situacao = "CALIBRADO";
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    else if (diasAVencer <= 0)
                    {
                        situacao = "VENCIDO";
                        e.CellStyle.BackColor = Color.Red;
                        e.CellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        situacao = "CALIBRAR";
                        e.CellStyle.BackColor = Color.Yellow;
                        e.CellStyle.ForeColor = Color.Black;
                    }
                    e.Value = situacao;
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = "N/A";
                    e.CellStyle.BackColor = Color.LightGray;
                    e.CellStyle.ForeColor = Color.DarkGray;
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewRow linhaSelecionada = dgvDados.Rows[e.RowIndex];
            FormDetalhes formDetalhes = new FormDetalhes(linhaSelecionada);
            formDetalhes.ShowDialog();
        }
    }
}