using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormItemEditar : Form
    {
        // Propriedade para retornar os dados editados para o formulário principal
        public object[] ItemEditadoDados { get; private set; }

        // Variável temporária para guardar o caminho da NOVA foto selecionada (antes de salvar)
        private string caminhoOrigemFoto = null;
        // Variável temporária para guardar o caminho do NOVO PDF selecionado
        private string caminhoOrigemPDF = null;

        // Definição das pastas
        private string pastaCertificados = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Certificados");
        private string pastaImagens = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Imagens");

        public FormItemEditar(DataGridViewRow linhaParaEditar)
        {
            InitializeComponent();

            // Garante que as pastas existem
            if (!Directory.Exists(pastaCertificados)) Directory.CreateDirectory(pastaCertificados);
            if (!Directory.Exists(pastaImagens)) Directory.CreateDirectory(pastaImagens);

            PreencherCampos(linhaParaEditar);
        }

        private void FormItemEditar_Load(object sender, EventArgs e)
        {
        }

        private void PreencherCampos(DataGridViewRow linha)
        {
            // Preenche os textos
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

            // Carrega campos extras se existirem
            if (linha.Cells["colFoto"] != null)
                txtCaminhoFoto.Text = linha.Cells["colFoto"].Value?.ToString();

            if (linha.Cells["colObservacoes"] != null)
                txtObservacoes.Text = linha.Cells["colObservacoes"].Value?.ToString();

            if (linha.Cells["colMecanico"] != null)
                txtMecanico.Text = linha.Cells["colMecanico"].Value?.ToString();

            if (linha.Cells["colCertificadoPDF"] != null)
                txtCaminhoPDF.Text = linha.Cells["colCertificadoPDF"].Value?.ToString();

            // Carrega a imagem atual no PictureBox (se existir)
            if (!string.IsNullOrEmpty(txtCaminhoFoto.Text))
            {
                string caminhoCompleto = Path.Combine(pastaImagens, txtCaminhoFoto.Text);
                if (File.Exists(caminhoCompleto))
                {
                    try
                    {
                        // FileStream evita travar o arquivo no disco
                        using (FileStream fs = new FileStream(caminhoCompleto, FileMode.Open, FileAccess.Read))
                        {
                            pictureBoxFoto.Image = Image.FromStream(fs);
                        }
                    }
                    catch
                    {
                        pictureBoxFoto.Image = null;
                    }
                }
            }
        }

        // ==========================================
        // BOTÃO SELECIONAR FOTO (CORRIGIDO)
        // ==========================================
        private void btnSelecionarFoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecionar Foto";
                ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // 1. Guarda o caminho de onde o usuário pegou a foto (NÃO COPIA AINDA)
                        caminhoOrigemFoto = ofd.FileName;

                        // 2. Mostra um preview na tela usando MemoryStream para não travar o arquivo
                        using (var stream = new MemoryStream(File.ReadAllBytes(caminhoOrigemFoto)))
                        {
                            pictureBoxFoto.Image?.Dispose(); // Limpa anterior
                            pictureBoxFoto.Image = Image.FromStream(stream);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao visualizar imagem: " + ex.Message);
                    }
                }
            }
        }

        // ==========================================
        // BOTÃO ANEXAR PDF (CORRIGIDO)
        // ==========================================
        private void btnAnexarPDF_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = "Selecionar PDF";
                ofd.Filter = "Arquivos PDF|*.pdf";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Apenas guarda o caminho, deixa para copiar no Salvar
                    caminhoOrigemPDF = ofd.FileName;
                    txtCaminhoPDF.Text = Path.GetFileName(ofd.FileName) + " (Aguardando Salvar...)";
                }
            }
        }

        // ==========================================
        // BOTÃO SALVAR (AQUI FAZ A CÓPIA REAL)
        // ==========================================
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // 1. Processar FOTO
            string nomeFinalFoto = txtCaminhoFoto.Text; // Mantém o antigo por padrão

            // Se o usuário escolheu uma foto nova (variavel não é null)
            if (!string.IsNullOrEmpty(caminhoOrigemFoto))
            {
                try
                {
                    string novoNome = "Foto_" + Guid.NewGuid().ToString() + Path.GetExtension(caminhoOrigemFoto);
                    string destino = Path.Combine(pastaImagens, novoNome);

                    File.Copy(caminhoOrigemFoto, destino, true); // Copia agora!
                    nomeFinalFoto = novoNome;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar a foto: " + ex.Message);
                    return; // Para tudo se der erro
                }
            }

            // 2. Processar PDF
            string nomeFinalPDF = txtCaminhoPDF.Text.Replace(" (Aguardando Salvar...)", ""); // Limpa texto visual

            if (!string.IsNullOrEmpty(caminhoOrigemPDF))
            {
                try
                {
                    string novoNomePDF = "Cert_" + Guid.NewGuid().ToString() + ".pdf";
                    string destinoPDF = Path.Combine(pastaCertificados, novoNomePDF);

                    File.Copy(caminhoOrigemPDF, destinoPDF, true); // Copia agora!
                    nomeFinalPDF = novoNomePDF;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao salvar o PDF: " + ex.Message);
                    return;
                }
            }

            // 3. Montar objeto de retorno
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
                null,
                txtExecutante.Text,
                txtInstalada.Text,
                txtLocal.Text,
                txtSubLocalizacao.Text,
                nomeFinalFoto,      // Usa o nome processado
                txtObservacoes.Text,
                txtMecanico.Text,
                nomeFinalPDF        // Usa o nome processado
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