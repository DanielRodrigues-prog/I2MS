using System;
using System.Globalization;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormItemEditar : Form
    {
        public object[] ItemEditadoDados { get; private set; }

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
                null, // Espaço para SITUAÇÃO (será recalculado)
                txtExecutante.Text,
                txtInstalada.Text,
                txtLocal.Text,
                txtSubLocalizacao.Text
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