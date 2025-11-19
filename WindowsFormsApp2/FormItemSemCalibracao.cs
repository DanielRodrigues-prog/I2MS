using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
// Removemos o using Microsoft.Data.SqlClient porque não conecta mais direto!

namespace WindowsFormsApp2
{
    public partial class FormItemSemCalibracao : Form
    {
        // Essa variável guarda os dados para o Form1 ler depois
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

            // Carrega apenas o que está no Grid
            txtDescricao.Text = linha.Cells["colDescricao"].Value?.ToString();
            txtCodigo.Text = linha.Cells["colCodigo"].Value?.ToString();
            txtPN.Text = linha.Cells["colPN"].Value?.ToString();
            txtFabricante.Text = linha.Cells["colFabricante"].Value?.ToString();
            txtLocal.Text = linha.Cells["colLocal"].Value?.ToString();
            txtCadastroLocal.Text = linha.Cells["colCadastroLocal"].Value?.ToString();
            txtCodLocal.Text = linha.Cells["colCodLocal"].Value?.ToString();

            string status = linha.Cells["colStatus"].Value?.ToString();
            if (!string.IsNullOrEmpty(status) && cmbStatus.Items.Contains(status))
                cmbStatus.SelectedItem = status;

            // Nota: Mecânico não costuma vir nessa grid simples, mas se tiver, carrega
            // txtMecanico.Text = ... 

            // OBS: Não buscamos mais Foto/PDF no banco aqui (BuscarExtrasDoBanco foi removido).
            // Se quiser editar a foto, o usuário deve selecionar o arquivo novamente por enquanto.
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
                        txtCaminhoFoto.Text = nomeUnico; // Guarda só o nome do arquivo
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

                        txtCaminhoCertificado.Text = nomeUnico; // Guarda só o nome do arquivo
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

            // Prepara o pacote de dados para o Form1 ler e enviar pra API
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
                txtCaminhoFoto.Text.Trim(),     // 9  (Caminho local da imagem)
                txtCaminhoCertificado.Text.Trim(), // 10 (Caminho local do PDF)
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