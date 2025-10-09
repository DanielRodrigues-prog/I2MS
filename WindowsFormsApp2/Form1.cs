using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private string usuarioAtual;

        public Form1(string usuarioLogado)
        {
            InitializeComponent();
            this.usuarioAtual = usuarioLogado;
            this.Text = $"CONTROLE DE CALIBRAÇÃO - Usuário: {this.usuarioAtual}";

            if (this.usuarioAtual.ToLower() == "Matheus.machado")
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

        // --- FUNÇÃO ATUALIZADA ABAIXO ---
        private void Form1_Load(object sender, EventArgs e)
        {
            string caminhoArquivo = "dados.csv";
            if (File.Exists(caminhoArquivo))
            {
                string[] linhas = File.ReadAllLines(caminhoArquivo);
                for (int i = 1; i < linhas.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(linhas[i])) continue;

                    string[] colunasLidasDoCsv = linhas[i].Split(';');

                    // Cria um "molde" de array com o tamanho exato da grade (15 posições)
                    object[] linhaParaGrid = new object[dgvDados.Columns.Count];

                    // Copia os dados que foram lidos do CSV para dentro do nosso "molde".
                    // Se o CSV tiver menos de 15 colunas, o resto do molde ficará vazio (null).
                    int colunasACopiar = Math.Min(colunasLidasDoCsv.Length, linhaParaGrid.Length);
                    Array.Copy(colunasLidasDoCsv, linhaParaGrid, colunasACopiar);

                    // Adiciona a linha perfeitamente formatada na grade
                    dgvDados.Rows.Add(linhaParaGrid);
                }
            }
            else
            {
                MessageBox.Show("Arquivo 'dados.csv' não encontrado.");
            }
        }

        private void btnNova_Click(object sender, EventArgs e)
        {
            FormItemNovo formularioDeCadastro = new FormItemNovo();
            if (formularioDeCadastro.ShowDialog() == DialogResult.OK)
            {
                dgvDados.Rows.Add(formularioDeCadastro.NovoItemDados);
                SalvarDadosNoCsv();
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
                    for (int i = 0; i < formularioDeEdicao.ItemEditadoDados.Length; i++)
                    {
                        if (i != 9) // 9 é o índice da coluna "Situação"
                        {
                            linhaSelecionada.Cells[i].Value = formularioDeEdicao.ItemEditadoDados[i];
                        }
                    }
                    SalvarDadosNoCsv();
                    MessageBox.Show("Item atualizado com sucesso.");
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
                    dgvDados.Rows.Remove(dgvDados.SelectedRows[0]);
                    SalvarDadosNoCsv();
                    MessageBox.Show("Item excluído com sucesso.");
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione uma linha inteira para excluir.");
            }
        }

        private void SalvarDadosNoCsv()
        {
            try
            {
                List<string> linhasParaSalvar = new List<string>();

                var cabecalho = dgvDados.Columns.Cast<DataGridViewColumn>().Select(col => col.HeaderText);
                linhasParaSalvar.Add(string.Join(";", cabecalho));

                foreach (DataGridViewRow row in dgvDados.Rows)
                {
                    if (row.IsNewRow) continue;

                    var celulas = row.Cells.Cast<DataGridViewCell>().Select(cell => {
                        if (cell.Value is DateTime)
                            return ((DateTime)cell.Value).ToString("dd/MM/yyyy");
                        return cell.Value?.ToString();
                    });
                    linhasParaSalvar.Add(string.Join(";", celulas));
                }

                File.WriteAllLines("dados.csv", linhasParaSalvar);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao salvar as alterações no arquivo CSV: " + ex.Message);
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

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            string termoBusca = txtBusca.Text.ToLower();

            foreach (DataGridViewRow linha in dgvDados.Rows)
            {
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
            }
        }

        private void dgvDados_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (this.dgvDados.Columns[e.ColumnIndex].Name == "colSituacao")
            {
                var celulaData = this.dgvDados.Rows[e.RowIndex].Cells["colDataVencimento"];

                string[] formatosDeData = { "dd/MM/yyyy HH:mm", "dd/MM/yyyy" };
                if (celulaData.Value != null && DateTime.TryParseExact(celulaData.Value.ToString().Trim(), formatosDeData, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataVencimento))
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
            }
        }

        private void dgvDados_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string nomeArquivoFoto = dgvDados.Rows[e.RowIndex].Cells["colFoto"].Value?.ToString();

            if (!string.IsNullOrEmpty(nomeArquivoFoto))
            {
                string caminhoCompleto = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens", nomeArquivoFoto);

                if (File.Exists(caminhoCompleto))
                {
                    FormVerFoto formFoto = new FormVerFoto(caminhoCompleto);
                    formFoto.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Arquivo de imagem não encontrado: " + nomeArquivoFoto);
                }
            }
        }
    }
}