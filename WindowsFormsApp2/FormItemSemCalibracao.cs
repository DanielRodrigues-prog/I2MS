using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormItemSemCalibracao : Form
    {
        public object[] ItemDados { get; private set; }
        private long itemId = -1;
        private string pastaDasImagens;
        private string pastaDosCertificados;

        public FormItemSemCalibracao()
        {
            InitializeComponent();
            ConfigurarPastas();
            cmbStatus.SelectedIndex = 0;
            txtMecanico.Text = "";
        }

        public FormItemSemCalibracao(DataGridViewRow linhaSelecionada)
        {
            InitializeComponent();
            ConfigurarPastas();
            CarregarDadosParaEdicao(linhaSelecionada);
        }

        private void ConfigurarPastas()
        {
            string pastaDoPrograma = AppDomain.CurrentDomain.BaseDirectory;
            this.pastaDasImagens = Path.Combine(pastaDoPrograma, "Imagens");
            this.pastaDosCertificados = Path.Combine(pastaDoPrograma, "Certificados");
            if (!Directory.Exists(pastaDasImagens)) Directory.CreateDirectory(pastaDasImagens);
            if (!Directory.Exists(pastaDosCertificados)) Directory.CreateDirectory(pastaDosCertificados);
        }

        private void CarregarDadosParaEdicao(DataGridViewRow linha)
        {
            if (linha.Tag != null) itemId = Convert.ToInt64(linha.Tag);

            // CORREÇÃO AQUI: Usei os nomes "colSem..." que estão no Form1
            // Se alguma coluna não existir (ex: vinda de outro grid), usamos ?. para não quebrar

            txtDescricao.Text = ObterValorCelula(linha, "colSemDescricao");
            txtCodigo.Text = ObterValorCelula(linha, "colSemCodigo");
            txtPN.Text = ObterValorCelula(linha, "colSemPN");
            txtFabricante.Text = ObterValorCelula(linha, "colSemFabricante");
            txtLocal.Text = ObterValorCelula(linha, "colSemLocal");
            txtCadastroLocal.Text = ObterValorCelula(linha, "colSemCadastroLocal");
            txtCodLocal.Text = ObterValorCelula(linha, "colSemCodLocal");
            txtMecanico.Text = ObterValorCelula(linha, "colSemMecanico");

            string status = ObterValorCelula(linha, "colSemStatus");
            if (!string.IsNullOrEmpty(status) && cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;

            // Tenta carregar foto/pdf se o grid tiver essas colunas escondidas (opcional)
            // Mas no seu caso, o Form1 não salva o caminho no grid, ele salva no objeto da API.
            // Então, na edição, a foto vai vir vazia a menos que você a selecione de novo,
            // ou implementemos uma lógica para baixar a foto novamente (avançado).
        }

        // Função auxiliar para evitar erro se o nome da coluna estiver errado
        private string ObterValorCelula(DataGridViewRow linha, string nomeColuna)
        {
            if (linha.DataGridView.Columns.Contains(nomeColuna))
            {
                return linha.Cells[nomeColuna].Value?.ToString();
            }
            return ""; // Retorna vazio se a coluna não existir
        }

        private void btnSelecionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imagens|*.jpg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string nomeUnico = Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName);
                        string destino = Path.Combine(pastaDasImagens, nomeUnico);
                        File.Copy(ofd.FileName, destino, true);

                        picFoto.Image = Image.FromFile(destino);
                        txtCaminhoFoto.Text = nomeUnico;
                    }
                    catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
        }

        private void btnAnexarCertificado_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "PDF|*.pdf";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string nomeUnico = Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName);
                        string destino = Path.Combine(pastaDosCertificados, nomeUnico);
                        File.Copy(ofd.FileName, destino, true);

                        txtCaminhoCertificado.Text = nomeUnico;
                    }
                    catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
                }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDescricao.Text) || string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MessageBox.Show("Descrição e Código são obrigatórios.");
                return;
            }

            ItemDados = new object[]
            {
                txtDescricao.Text.Trim(),       // 0
                txtCodigo.Text.Trim(),          // 1
                txtPN.Text.Trim(),              // 2
                txtFabricante.Text.Trim(),      // 3
                txtLocal.Text.Trim(),           // 4
                txtCadastroLocal.Text.Trim(),   // 5
                txtCodLocal.Text.Trim(),        // 6
                cmbStatus.SelectedItem?.ToString() ?? "", // 7
                txtMecanico.Text.Trim(),        // 8
                txtCaminhoFoto.Text.Trim(),     // 9
                txtCaminhoCertificado.Text.Trim(), // 10
                itemId                          // 11
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FormItemSemCalibracao_Load(object sender, EventArgs e) { }
    }
}