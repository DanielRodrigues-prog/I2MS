using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace WindowsFormsApp2
{
    public partial class FormMovimentacao : Form
    {
        private string usuarioLogado; // O Admin que está no PC

        // Lista para guardar os mecânicos carregados (ID, Nome)
        private List<ApiService.Mecanico> listaMecanicos;

        public FormMovimentacao()
        {
            InitializeComponent();
            // Pega o usuário logado no Windows (Admin)
            this.usuarioLogado = Environment.UserName.ToLower();
        }

        private async void FormMovimentacao_Load(object sender, EventArgs e)
        {
            // Carrega a lista de mecânicos para o combobox
            await CarregarListaMecanicos();
            txtInstrumentoID.Focus();
        }

        private async Task CarregarListaMecanicos()
        {
            try
            {
                cmbMecanicos.Items.Clear();
                listaMecanicos = await ApiService.GetMecs();

                foreach (var m in listaMecanicos)
                {
                    // Exibe "Nome (ID)" no combobox
                    cmbMecanicos.Items.Add($"{m.Nome} ({m.MecanicoID})");
                }

                if (cmbMecanicos.Items.Count > 0)
                    cmbMecanicos.SelectedIndex = 0; // Seleciona o primeiro
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar mecânicos: " + ex.Message); }
        }

        private async void txtInstrumentoID_TextChanged(object sender, EventArgs e)
        {
            if (txtInstrumentoID.Text.Length > 2)
            {
                var s = await ApiService.VerFerr(txtInstrumentoID.Text);
                if (s.Encontrada)
                {
                    if (string.IsNullOrEmpty(s.MecanicoAtual))
                    {
                        lblStatusFerramenta.Text = "DISPONÍVEL";
                        lblStatusFerramenta.ForeColor = Color.Green;
                        // Habilita campos de saída
                        cmbMecanicos.Enabled = true;
                        txtAeronave.Enabled = true;
                    }
                    else
                    {
                        lblStatusFerramenta.Text = "COM " + s.MecanicoAtual;
                        lblStatusFerramenta.ForeColor = Color.Red;
                        // Bloqueia campos pois é devolução
                        cmbMecanicos.Enabled = false;
                        txtAeronave.Enabled = false;
                    }
                }
                else
                {
                    lblStatusFerramenta.Text = "NÃO ENCONTRADA";
                    lblStatusFerramenta.ForeColor = Color.Gray;
                }
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e) { Mover("PEGAR"); }
        private void btnCheckIn_Click(object sender, EventArgs e) { Mover("DEVOLVER"); }

        private async void Mover(string tipo)
        {
            string idFerramenta = txtInstrumentoID.Text;
            string aeronave = txtAeronave.Text;
            string idMecanico = "";
            string nomeMecanico = "";

            if (tipo == "PEGAR")
            {
                if (cmbMecanicos.SelectedIndex == -1) { MessageBox.Show("Selecione um mecânico."); return; }
                if (string.IsNullOrWhiteSpace(aeronave)) { MessageBox.Show("Digite o prefixo da Aeronave."); return; }

                // Pega os dados do mecânico selecionado na lista
                var mecSelecionado = listaMecanicos[cmbMecanicos.SelectedIndex];
                idMecanico = mecSelecionado.MecanicoID;
                nomeMecanico = mecSelecionado.Nome;
            }

            // Envia tudo para a API
            string res = await ApiService.RegMov(tipo, idFerramenta, idMecanico, nomeMecanico, aeronave, usuarioLogado);

            if (res == "OK")
            {
                MessageBox.Show("Sucesso!");
                txtInstrumentoID.Clear();
                txtAeronave.Clear();
                lblStatusFerramenta.Text = "...";
                txtInstrumentoID.Focus();
            }
            else
            {
                MessageBox.Show(res);
            }
        }
        private void btnFechar_Click(object sender, EventArgs e) { Close(); }
    }
}