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
        private string usuarioLogado;
        private List<ApiService.Mecanico> listaMecanicos;

        public FormMovimentacao()
        {
            InitializeComponent();
            this.usuarioLogado = Environment.UserName.ToLower();
        }

        private async void FormMovimentacao_Load(object sender, EventArgs e)
        {
            await CarregarListaMecanicos();
            txtInstrumentoID.Focus();
        }

        private async Task CarregarListaMecanicos()
        {
            try
            {
                cmbMecanicos.Items.Clear();
                listaMecanicos = await ApiService.GetMecs();

                if (listaMecanicos != null)
                {
                    foreach (var m in listaMecanicos)
                    {
                        cmbMecanicos.Items.Add($"{m.Nome} ({m.MecanicoID})");
                    }
                    if (cmbMecanicos.Items.Count > 0) cmbMecanicos.SelectedIndex = 0;
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar mecânicos: " + ex.Message); }
        }

        // Verifica status ao digitar (Check-in/Check-out rápido)
        private async void txtInstrumentoID_TextChanged(object sender, EventArgs e)
        {
            if (txtInstrumentoID.Text.Length > 1) // Começa a verificar com 2 caracteres
            {
                try
                {
                    // Chama API para ver se a ferramenta existe e quem está com ela
                    var s = await ApiService.VerFerr(txtInstrumentoID.Text);

                    if (s.Encontrada)
                    {
                        if (string.IsNullOrEmpty(s.MecanicoAtual))
                        {
                            // Ferramenta está na prateleira -> Pode SAIR
                            lblStatusFerramenta.Text = "DISPONÍVEL";
                            lblStatusFerramenta.ForeColor = Color.Green;

                            cmbMecanicos.Enabled = true;
                            txtAeronave.Enabled = true;
                            btnCheckOut.Enabled = true;  // Habilita SAÍDA
                            btnCheckIn.Enabled = false; // Desabilita DEVOLUÇÃO
                        }
                        else
                        {
                            // Ferramenta está com alguém -> Pode DEVOLVER
                            lblStatusFerramenta.Text = "EM USO POR: " + s.MecanicoAtual;
                            lblStatusFerramenta.ForeColor = Color.Red;

                            cmbMecanicos.Enabled = false; // Não precisa selecionar mecânico na devolução
                            txtAeronave.Enabled = false;
                            btnCheckOut.Enabled = false; // Desabilita SAÍDA
                            btnCheckIn.Enabled = true;  // Habilita DEVOLUÇÃO
                        }
                    }
                    else
                    {
                        lblStatusFerramenta.Text = "NÃO ENCONTRADA";
                        lblStatusFerramenta.ForeColor = Color.Gray;
                        btnCheckOut.Enabled = false;
                        btnCheckIn.Enabled = false;
                    }
                }
                catch
                {
                    lblStatusFerramenta.Text = "..."; // Erro de conexão ou ID inválido
                }
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e) { Mover("SAÍDA"); }
        private void btnCheckIn_Click(object sender, EventArgs e) { Mover("DEVOLUÇÃO"); }

        private async void Mover(string tipo)
        {
            string idFerramenta = txtInstrumentoID.Text.Trim();
            string aeronave = txtAeronave.Text.Trim();
            string idMecanico = "0";
            string nomeMecanico = "SISTEMA"; // Valor padrão para devolução

            if (string.IsNullOrWhiteSpace(idFerramenta))
            {
                MessageBox.Show("Informe o ID ou Código da ferramenta.");
                return;
            }

            // Validações específicas para SAÍDA
            if (tipo == "SAÍDA")
            {
                if (cmbMecanicos.SelectedIndex == -1)
                {
                    MessageBox.Show("Selecione um mecânico.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(aeronave))
                {
                    MessageBox.Show("Digite o prefixo da Aeronave.");
                    return;
                }

                // Pega dados do mecânico
                if (listaMecanicos != null && cmbMecanicos.SelectedIndex < listaMecanicos.Count)
                {
                    var mec = listaMecanicos[cmbMecanicos.SelectedIndex];
                    idMecanico = mec.MecanicoID;
                    nomeMecanico = mec.Nome;
                }
            }
            else // DEVOLUÇÃO
            {
                // Na devolução, podemos mandar "DEVOLUÇÃO" como aeronave para ficar claro no histórico
                aeronave = "DEVOLUÇÃO";
            }

            try
            {
                // Chama a API unificada que faz tudo (Atualiza Status + Cria Histórico)
                string res = await ApiService.RegMov(tipo, idFerramenta, idMecanico, nomeMecanico, aeronave, usuarioLogado);

                if (res == "OK")
                {
                    // Feedback visual
                    string msg = (tipo == "SAÍDA") ?
                        $"✅ Empréstimo registrado para {nomeMecanico}!" :
                        "✅ Devolução registrada com sucesso!";

                    MessageBox.Show(msg, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Limpa para o próximo item (fluxo rápido)
                    txtInstrumentoID.Clear();
                    txtAeronave.Clear();
                    lblStatusFerramenta.Text = "AGUARDANDO LEITURA...";
                    lblStatusFerramenta.ForeColor = Color.Black;

                    // Reseta botões
                    btnCheckOut.Enabled = false;
                    btnCheckIn.Enabled = false;
                    cmbMecanicos.Enabled = true;
                    txtAeronave.Enabled = true;

                    txtInstrumentoID.Focus(); // Volta o foco para ler o próximo código de barras
                }
                else
                {
                    MessageBox.Show("Erro ao registrar: " + res, "Erro API", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message, "Erro Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e) { Close(); }
    }
}