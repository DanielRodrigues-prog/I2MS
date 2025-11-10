using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormItemNovo : Form
    {
        public object[] NovoItemDados { get; private set; }
        private string pastaCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");
        private string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

        public FormItemNovo()
        {
            InitializeComponent();
            // Garante que a pasta de certificados existe
            if (!Directory.Exists(pastaCertificados))
                Directory.CreateDirectory(pastaCertificados);
        }

        private void btnSelecionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivos de Imagem|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string caminhoOriginal = openFileDialog.FileName;
                        string nomeUnico = Guid.NewGuid().ToString() + Path.GetExtension(caminhoOriginal);
                        string caminhoDestino = Path.Combine(pastaImagens, nomeUnico);
                        File.Copy(caminhoOriginal, caminhoDestino);

                        txtCaminhoFoto.Text = nomeUnico;
                        pictureBoxFoto.Image = Image.FromFile(caminhoDestino);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao copiar imagem: " + ex.Message);
                    }
                }
            }
        }

        private void btnAnexarPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Arquivo PDF|*.pdf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string caminhoOriginal = openFileDialog.FileName;
                        string nomeArquivo = Path.GetFileName(caminhoOriginal);
                        string caminhoDestino = Path.Combine(pastaCertificados, nomeArquivo);

                        File.Copy(caminhoOriginal, caminhoDestino, true); // Permite sobrescrever
                        txtCaminhoPDF.Text = nomeArquivo;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao copiar PDF: " + ex.Message);
                    }
                }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            NovoItemDados = new object[]
            {
                txtInstrumento.Text,
                txtModelo.Text,
                txtPN.Text,
                txtSN.Text,
                txtIdentifSOD.Text,
                txtIdentifOficina.Text,
                txtCertificado.Text,
                dtpDataCalibracao.Value,
                dtpDataVencimento.Value,
                null, // Espaço para SITUAÇÃO
                txtExecutante.Text,
                txtInstalada.Text,
                txtLocal.Text,
                txtSubLocalizacao.Text,
                txtCaminhoFoto.Text,
                txtObservacoes.Text, // NOVO
                txtMecanico.Text, // NOVO
                txtCaminhoPDF.Text // NOVO
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