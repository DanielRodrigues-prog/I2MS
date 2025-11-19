using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormDetalhes : Form
    {
        private string caminhoDoCertificado = "";
        private string pastaCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");
        private string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

        public FormDetalhes(DataGridViewRow linha)
        {
            InitializeComponent();
            CarregarDados(linha);
        }

        private void CarregarDados(DataGridViewRow linha)
        {
            // Pega os dados da linha e preenche os campos
            lblInstrumentoTitulo.Text = linha.Cells["colInstrumento"].Value?.ToString();
            txtObservacoes.Text = linha.Cells["colObservacoes"].Value?.ToString();
            txtMecanico.Text = linha.Cells["colMecanico"].Value?.ToString();

            // Carrega a Foto
            string nomeFoto = linha.Cells["colFoto"].Value?.ToString();
            if (!string.IsNullOrEmpty(nomeFoto))
            {
                string caminhoFoto = Path.Combine(pastaImagens, nomeFoto);
                if (File.Exists(caminhoFoto))
                    pictureBoxFoto.Image = Image.FromFile(caminhoFoto);
            }

            // Prepara o link do Certificado
            string nomeCertificado = linha.Cells["colCertificadoPDF"].Value?.ToString();
            if (!string.IsNullOrEmpty(nomeCertificado))
            {
                this.caminhoDoCertificado = Path.Combine(pastaCertificados, nomeCertificado);
                if (File.Exists(this.caminhoDoCertificado))
                {
                    linkCertificado.Text = nomeCertificado;
                    btnAbrirCertificado.Enabled = true;
                }
                else
                {
                    linkCertificado.Text = "Arquivo não encontrado: " + nomeCertificado;
                }
            }
        }

        private void btnAbrirCertificado_Click(object sender, EventArgs e)
        {
            try
            {
                // Abre o arquivo PDF com o leitor padrão do Windows
                System.Diagnostics.Process.Start(this.caminhoDoCertificado);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível abrir o arquivo PDF.\n" + ex.Message);
            }
        }

        private void FormDetalhes_Load(object sender, EventArgs e)
        {

        }
    }
}