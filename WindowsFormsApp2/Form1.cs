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

            string pastaDoPrograma = AppDomain.CurrentDomain.BaseDirectory;
            this.pastaDasImagens = Path.Combine(pastaDoPrograma, "Imagens");
            this.pastaDosCertificados = Path.Combine(pastaDoPrograma, "Certificados");

            Database.InitializeDatabase();

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

            if (this.usuarioAtual.ToLower() == "daniel rodrigues")
            {
                btnEditar.Enabled = true;
                btnExcluir.Enabled = true;
                btnMecanicos.Enabled = true;
            }
            else
            {
                btnEditar.Enabled = false;
                btnExcluir.Enabled = false;
                btnMecanicos.Enabled = false;
            }
            // A movimentação é permitida para todos, a lógica de permissão está dentro do formulário
            btnMovimentacao.Enabled = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CarregarDadosDoBanco();
        }

        private void CarregarDadosDoBanco()
        {
            dgvDados.Rows.Clear();
            List<string> proximosAVencer = new List<string>();

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sql = "SELECT ID, Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, Certificado, DataCalibracao, DataVencimento, Situacao, Executante, Instalada, Local, SubLocalizacao, Foto, Observacoes, Mecanico, CertificadoPDF FROM Instrumentos";

                    using (var comando = new SQLiteCommand(sql, conexao))
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvDados.Rows.Add(new object[] {
                                reader["Instrumento"], reader["Modelo"], reader["PN"], reader["SN"],
                                reader["IdentifSOD"], reader["IdentifOficina"], reader["Certificado"],
                                reader["DataCalibracao"], reader["DataVencimento"],
                                null,
                                reader["Executante"], reader["Instalada"], reader["Local"],
                                reader["SubLocalizacao"], reader["Foto"], reader["Observacoes"],
                                reader["Mecanico"], reader["CertificadoPDF"]
                            });

                            dgvDados.Rows[dgvDados.Rows.Count - 1].Tag = reader["ID"];

                            string dataVencTexto = reader["DataVencimento"].ToString();
                            if (DateTime.TryParseExact(dataVencTexto, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataVenc))
                            {
                                int dias = (int)(dataVenc.Date - DateTime.Today).TotalDays;
                                if (dias > 0 && dias <= 45)
                                {
                                    proximosAVencer.Add($"{reader["Instrumento"]} (Vence em {dias} dias)");
                                }
                            }
                        }
                    }
                }

                if (proximosAVencer.Count > 0)
                {
                    string mensagem = "Os seguintes itens estão próximos do vencimento (45 dias ou menos):\n\n" +
                                      string.Join("\n", proximosAVencer);
                    MessageBox.Show(mensagem, "Alerta de Vencimento", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                    )";

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
                            comando.Parameters.AddWithValue("@p7", ((DateTime)dados[7]).ToString("dd/MM/yyyy"));
                            comando.Parameters.AddWithValue("@p8", ((DateTime)dados[8]).ToString("dd/MM/yyyy"));
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
                    CarregarDadosDoBanco();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar no banco de dados: " + ex.Message);
                }
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count > 0)
            {
                DataGridViewRow linhaSelecionada = dgvDados.SelectedRows[0];
                FormItemEditar formularioDeEdicao = new FormItemEditar(linhaSelecionada);

                if (formularioDeEdicao.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (var conexao = Database.GetConnection())
                        {
                            conexao.Open();
                            string sql = @"UPDATE Instrumentos SET
                                            Instrumento = @p0, Modelo = @p1, PN = @p2, SN = @p3, 
                                            IdentifSOD = @p4, IdentifOficina = @p5, Certificado = @p6, 
                                            DataCalibracao = @p7, DataVencimento = @p8, Executante = @p10, 
                                            Instalada = @p11, Local = @p12, SubLocalizacao = @p13, 
                                            Foto = @p14, Observacoes = @p15, Mecanico = @p16, CertificadoPDF = @p17
                                         WHERE ID = @id";

                            using (var comando = new SQLiteCommand(sql, conexao))
                            {
                                object[] dados = formularioDeEdicao.ItemEditadoDados;
                                comando.Parameters.AddWithValue("@p0", dados[0]);
                                comando.Parameters.AddWithValue("@p1", dados[1]);
                                comando.Parameters.AddWithValue("@p2", dados[2]);
                                comando.Parameters.AddWithValue("@p3", dados[3]);
                                comando.Parameters.AddWithValue("@p4", dados[4]);
                                comando.Parameters.AddWithValue("@p5", dados[5]);
                                comando.Parameters.AddWithValue("@p6", dados[6]);
                                comando.Parameters.AddWithValue("@p7", ((DateTime)dados[7]).ToString("dd/MM/yyyy"));
                                comando.Parameters.AddWithValue("@p8", ((DateTime)dados[8]).ToString("dd/MM/yyyy"));
                                comando.Parameters.AddWithValue("@p10", dados[10]);
                                comando.Parameters.AddWithValue("@p11", dados[11]);
                                comando.Parameters.AddWithValue("@p12", dados[12]);
                                comando.Parameters.AddWithValue("@p13", dados[13]);
                                comando.Parameters.AddWithValue("@p14", dados[14]);
                                comando.Parameters.AddWithValue("@p15", dados[15]);
                                comando.Parameters.AddWithValue("@p16", dados[16]);
                                comando.Parameters.AddWithValue("@p17", dados[17]);
                                comando.Parameters.AddWithValue("@id", linhaSelecionada.Tag);

                                comando.ExecuteNonQuery();
                            }
                        }
                        CarregarDadosDoBanco();
                        MessageBox.Show("Item atualizado com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao atualizar no banco de dados: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma linha inteira para editar.");
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count > 0)
            {
                var confirmacao = MessageBox.Show("Tem certeza que deseja excluir o item selecionado?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        object idParaExcluir = dgvDados.SelectedRows[0].Tag;
                        using (var conexao = Database.GetConnection())
                        {
                            conexao.Open();
                            string sql = "DELETE FROM Instrumentos WHERE ID = @id";
                            using (var comando = new SQLiteCommand(sql, conexao))
                            {
                                comando.Parameters.AddWithValue("@id", idParaExcluir);
                                comando.ExecuteNonQuery();
                            }
                        }
                        dgvDados.Rows.Remove(dgvDados.SelectedRows[0]);
                        MessageBox.Show("Item excluído com sucesso.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir do banco de dados: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma linha inteira para excluir.");
            }
        }

        private void btnProcurar_Click(object sender, EventArgs e)
        {
            bool novoEstadoVisivel = !txtBusca.Visible;
            txtBusca.Visible = novoEstadoVisivel;
            lblBusca.Visible = novoEstadoVisivel;

            if (novoEstadoVisivel)
            {
                txtBusca.Focus();
            }
            else
            {
                txtBusca.Text = "";
            }
        }

        private void btnMecanicos_Click(object sender, EventArgs e)
        {
            FormMecanicos formGestaoMecanicos = new FormMecanicos();
            formGestaoMecanicos.ShowDialog();
        }

        // --- FUNÇÃO DO BOTÃO MOVIMENTAÇÃO ATUALIZADA ---
        private void btnMovimentacao_Click(object sender, EventArgs e)
        {
            // Abre o novo formulário de Check-in/Check-out
            FormMovimentacao formMovi = new FormMovimentacao();
            formMovi.ShowDialog();

            // Depois que o formulário de movimentação fechar,
            // atualiza a grelha principal para mostrar quem pegou a ferramenta.
            CarregarDadosDoBanco();
        }
        // ------------------------------------------

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
                    {
                        linha.Visible = true;
                    }
                    else
                    {
                        linha.Visible = false;
                    }
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