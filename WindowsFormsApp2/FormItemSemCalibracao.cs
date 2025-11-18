using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;

namespace WindowsFormsApp2
{
    public partial class FormItemSemCalibracao : Form
    {
        public object[] ItemDados { get; private set; } // Retorna os dados para Form1
        private bool modoEdicao = false;
        private long itemId = -1; // Para guardar o ID do item sendo editado
        private string pastaDasImagens;
        private string pastaDosCertificados;

        // Construtor para NOVO item
        public FormItemSemCalibracao()
        {
            InitializeComponent();
            this.Text = "Novo Item Sem Calibração";
            ConfigurarPastas();
            cmbStatus.SelectedIndex = 0; // Define um status padrão
            txtMecanico.Text = ""; // Novo item começa sem mecânico
        }

        // Construtor para EDITAR item
        public FormItemSemCalibracao(DataGridViewRow linhaSelecionada)
        {
            InitializeComponent();
            this.Text = "Editar Item Sem Calibração";
            this.modoEdicao = true;
            ConfigurarPastas();
            CarregarDadosParaEdicao(linhaSelecionada);
        }

        private void ConfigurarPastas()
        {
            string pastaDoPrograma = AppDomain.CurrentDomain.BaseDirectory;
            this.pastaDasImagens = Path.Combine(pastaDoPrograma, "Imagens");
            this.pastaDosCertificados = Path.Combine(pastaDoPrograma, "Certificados");
        }

        private void CarregarDadosParaEdicao(DataGridViewRow linha)
        {
            itemId = Convert.ToInt64(linha.Tag); // O ID está na Tag da linha

            // Usamos os nomes das colunas definidos no Form1.Designer.cs para a aba sem calibração
            // Certifique-se que os nomes das colunas batem com o que está no Form1.Designer.cs
            // No Form1.Designer.cs atual, as colunas são: colSemDescricao, colSemCodigo, etc.

            txtDescricao.Text = linha.Cells["colSemDescricao"].Value?.ToString();
            txtCodigo.Text = linha.Cells["colSemCodigo"].Value?.ToString();
            txtPN.Text = linha.Cells["colSemPN"].Value?.ToString();
            txtFabricante.Text = linha.Cells["colSemFabricante"].Value?.ToString();
            txtLocal.Text = linha.Cells["colSemLocal"].Value?.ToString();
            txtCadastroLocal.Text = linha.Cells["colSemCadastroLocal"].Value?.ToString();
            txtCodLocal.Text = linha.Cells["colSemCodLocal"].Value?.ToString();
            cmbStatus.SelectedItem = linha.Cells["colSemStatus"].Value?.ToString();
            txtMecanico.Text = linha.Cells["colSemMecanico"].Value?.ToString();

            // Para Foto e Certificado, eles não estão na grade visível (geralmente).
            // Precisamos buscá-los do banco de dados usando o ID.
            BuscarExtrasDoBanco(itemId);
        }

        private void BuscarExtrasDoBanco(long id)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string sql = "SELECT Foto, CertificadoPDF FROM FerramentasSemCalibracao WHERE ID = @id";
                    using (var cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read())
                            {
                                string nomeFoto = r["Foto"].ToString();
                                string nomePdf = r["CertificadoPDF"].ToString();

                                if (!string.IsNullOrEmpty(nomeFoto))
                                {
                                    string caminhoCompleto = Path.Combine(pastaDasImagens, nomeFoto);
                                    if (File.Exists(caminhoCompleto))
                                    {
                                        picFoto.Image = Image.FromFile(caminhoCompleto);
                                        txtCaminhoFoto.Text = nomeFoto;
                                    }
                                }

                                if (!string.IsNullOrEmpty(nomePdf))
                                {
                                    txtCaminhoCertificado.Text = nomePdf;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar detalhes extras: " + ex.Message); }
        }

        private void FormItemSemCalibracao_Load(object sender, EventArgs e)
        {
            // Nada específico a carregar aqui no load além do construtor
        }

        private void btnSelecionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.bmp|Todos os Arquivos|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Copia a imagem para a pasta 'Imagens' do projeto
                        string nomeArquivo = Path.GetFileName(ofd.FileName);
                        string destino = Path.Combine(pastaDasImagens, nomeArquivo);
                        File.Copy(ofd.FileName, destino, true); // true para sobrescrever se existir

                        picFoto.Image = Image.FromFile(destino);
                        txtCaminhoFoto.Text = nomeArquivo; // Armazena apenas o nome do arquivo no TextBox
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao carregar ou copiar a imagem: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        picFoto.Image = null;
                        txtCaminhoFoto.Text = "";
                    }
                }
            }
        }

        private void btnAnexarCertificado_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Arquivos PDF|*.pdf|Todos os Arquivos|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Copia o PDF para a pasta 'Certificados' do projeto
                        string nomeArquivo = Path.GetFileName(ofd.FileName);
                        string destino = Path.Combine(pastaDosCertificados, nomeArquivo);
                        File.Copy(ofd.FileName, destino, true); // true para sobrescrever se existir

                        txtCaminhoCertificado.Text = nomeArquivo; // Armazena apenas o nome do arquivo
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao anexar ou copiar o certificado: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txtCaminhoCertificado.Text = "";
                    }
                }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("A descrição é obrigatória.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("O código é obrigatório.", "Erro de Validação", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Coleta todos os dados para retornar ao Form1
            ItemDados = new object[]
            {
                txtDescricao.Text.Trim(),
                txtCodigo.Text.Trim(),
                txtPN.Text.Trim(),
                txtFabricante.Text.Trim(),
                txtLocal.Text.Trim(),
                txtCadastroLocal.Text.Trim(),
                txtCodLocal.Text.Trim(),
                cmbStatus.SelectedItem?.ToString() ?? "",
                txtMecanico.Text.Trim(),
                txtCaminhoFoto.Text.Trim(),
                txtCaminhoCertificado.Text.Trim(),
                itemId // Usado para edição (pode ser -1 para novo)
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}