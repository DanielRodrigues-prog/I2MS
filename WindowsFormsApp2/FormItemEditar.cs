using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormItemEditar : Form
    {
        public object[] ItemEditadoDados { get; private set; }
        private string pastaCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");
        private string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

        public FormItemEditar(DataGridViewRow linhaParaEditar)
        {
            InitializeComponent();
            PreencherCampos(linhaParaEditar);
        }

        private void PreencherCampos(DataGridViewRow linha)
        {
            txtInstrumento.Text = linha.Cells["colInstrumento"].Value?.ToString();
            txtModelo.Text = linha.Cells["colModelo"].Value?.ToString();
            txtPN.Text = linha.Cells["colPN"].Value?.ToString();
            txtSN.Text = linha.Cells["colSN"].Value?.ToString();
            txtIdentifSOD.Text = linha.Cells["colIdentifSOD"].Value?.ToString();
            txtIdentifOficina.Text = linha.Cells["colIdentifOficina"].Value?.ToString();
            txtCertificado.Text = linha.Cells["colCertificado"].Value?.ToString();

            if (DateTime.TryParse(linha.Cells["colDataCalibracao"].Value?.ToString(), out DateTime dataCal))
                dtpDataCalibracao.Value = dataCal;
            if (DateTime.TryParse(linha.Cells["colDataVencimento"].Value?.ToString(), out DateTime dataVenc))
                dtpDataVencimento.Value = dataVenc;

            txtExecutante.Text = linha.Cells["colExecutante"].Value?.ToString();
            txtInstalada.Text = linha.Cells["colInstalada"].Value?.ToString();
            txtLocal.Text = linha.Cells["colLocal"].Value?.ToString();
            txtSubLocalizacao.Text = linha.Cells["colSubLocalizacao"].Value?.ToString();

            // Carrega os novos campos
            txtCaminhoFoto.Text = linha.Cells["colFoto"].Value?.ToString();
            txtObservacoes.Text = linha.Cells["colObservacoes"].Value?.ToString();
            txtMecanico.Text = linha.Cells["colMecanico"].Value?.ToString();
            txtCaminhoPDF.Text = linha.Cells["colCertificadoPDF"].Value?.ToString();

            // Carrega a pré-visualização da foto
            if (!string.IsNullOrEmpty(txtCaminhoFoto.Text))
            {
                string caminhoFoto = Path.Combine(pastaImagens, txtCaminhoFoto.Text);
                if (File.Exists(caminhoFoto))
                    pictureBoxFoto.Image = Image.FromFile(caminhoFoto);
            }
        }

        private void btnSelecionarFoto_Click(object sender, EventArgs e)
        {
            // (Código idêntico ao do FormItemNovo.cs)
        }

        private void btnAnexarPDF_Click(object sender, EventArgs e)
        {
            // (Código idêntico ao do FormItemNovo.cs)
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            ItemEditadoDados = new object[]
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
                null, // SITUAÇÃO
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

        private void btnAnexarPDF_Click_1(object sender, EventArgs e)
        {

        }

        private void FormItemEditar_Load(object sender, EventArgs e)
        {

        }
    }
}