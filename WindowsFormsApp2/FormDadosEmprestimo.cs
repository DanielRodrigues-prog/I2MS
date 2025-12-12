using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormDadosEmprestimo : Form
    {
        // Propriedades
        public string Obs { get { return txtObs.Text; } }
        public string CedenteNome { get { return txtCedNome.Text; } }
        public string CedenteEmpresa { get { return cmbCedEmpresa.Text; } }
        public string CedenteEmail { get { return txtCedEmail.Text; } }
        public string CedenteTelefone { get { return txtCedTelefone.Text; } }
        public string RequerenteNome { get { return txtReqNome.Text; } }
        public string RequerenteEmpresa { get { return cmbReqEmpresa.Text; } }
        public string RequerenteEmail { get { return txtReqEmail.Text; } }
        public string RequerenteTelefone { get { return txtReqTelefone.Text; } }

        // Controles
        private TextBox txtObs;
        private TextBox txtCedNome, txtCedEmail, txtCedTelefone;
        private ComboBox cmbCedEmpresa;
        private TextBox txtReqNome, txtReqEmail, txtReqTelefone;
        private ComboBox cmbReqEmpresa;
        private ListBox lstFerramentas;

        public FormDadosEmprestimo(List<DataGridViewRow> ferramentasSelecionadas, string usuarioLogado)
        {
            InitializeComponent();
            MontarVisualClean(ferramentasSelecionadas, usuarioLogado);
        }

        private void MontarVisualClean(List<DataGridViewRow> ferramentas, string usuarioLogado)
        {
            // Configurações da Janela (Estilo Limpo)
            this.Text = "Empréstimo de Ferramentas";
            this.Size = new Size(820, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke; // Fundo cinza bem claro

            Font fontTitulo = new Font("Segoe UI", 14, FontStyle.Bold);
            Font fontLabel = new Font("Segoe UI", 9, FontStyle.Regular); // Fonte normal, não negrito para ficar leve
            Font fontNormal = new Font("Segoe UI", 10);

            // --- CABEÇALHO ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(50, 50, 50) }; // Cinza escuro profissional
            Label lblTitulo = new Label { Text = "Novo Empréstimo", ForeColor = Color.White, Font = fontTitulo, AutoSize = true, Location = new Point(20, 15) };

            Button btnGerar = new Button { Text = "Gerar Recibo", BackColor = Color.DodgerBlue, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 9, FontStyle.Bold), Size = new Size(140, 35), Location = new Point(640, 12), Cursor = Cursors.Hand };
            btnGerar.FlatAppearance.BorderSize = 0;
            btnGerar.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnGerar);
            this.Controls.Add(pnlHeader);

            // --- CONTEÚDO SUPERIOR ---
            int y = 80;

            // Lista de Ferramentas (Esquerda)
            Label lblFerramentas = new Label { Text = "Itens Selecionados:", Location = new Point(20, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.DimGray };
            lstFerramentas = new ListBox { Location = new Point(20, y + 25), Size = new Size(370, 120), Font = fontNormal, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            foreach (var row in ferramentas)
            {
                string desc = "Item";
                string pn = "N/A";

                // Lógica segura para pegar valores mesmo se a célula for nula
                if (row.DataGridView.Name == "dgvDados") // Aba Com Calibração
                {
                    if (row.Cells["colInstrumento"].Value != null) desc = row.Cells["colInstrumento"].Value.ToString();
                    if (row.Cells["colPN"].Value != null) pn = row.Cells["colPN"].Value.ToString();
                }
                else // Aba Sem Calibração
                {
                    if (row.Cells["colSemDescricao"].Value != null) desc = row.Cells["colSemDescricao"].Value.ToString();
                    if (row.Cells["colSemPN"].Value != null) pn = row.Cells["colSemPN"].Value.ToString();
                }
                lstFerramentas.Items.Add($"{desc} (PN: {pn})");
            }

            // Observações (Direita)
            Label lblObs = new Label { Text = "Observações / O.S.:", Location = new Point(410, y), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = Color.DimGray };
            txtObs = new TextBox { Location = new Point(410, y + 25), Size = new Size(370, 120), Multiline = true, Font = fontNormal, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            this.Controls.Add(lblFerramentas);
            this.Controls.Add(lstFerramentas);
            this.Controls.Add(lblObs);
            this.Controls.Add(txtObs);

            // --- GRUPOS CEDENTE / REQUERENTE ---
            y += 160;

            // Criar Grupos (Visual Clean)
            GroupBox grpCedente = CriarGrupoClean("Cedente (Quem Empresta)", 20, y, fontLabel, fontNormal);
            txtCedNome = (TextBox)grpCedente.Controls["txtNome"];
            cmbCedEmpresa = (ComboBox)grpCedente.Controls["cmbEmpresa"];
            txtCedEmail = (TextBox)grpCedente.Controls["txtEmail"];
            txtCedTelefone = (TextBox)grpCedente.Controls["txtTelefone"];

            // Preenchimento Automático
            txtCedNome.Text = usuarioLogado;
            cmbCedEmpresa.Text = "VOAR AVIATION";
            txtCedEmail.Text = "suprimentos@voar.aero";
            txtCedTelefone.Text = "+55 (11) 5070 6000";

            GroupBox grpRequerente = CriarGrupoClean("Requerente (Quem Recebe)", 410, y, fontLabel, fontNormal);
            txtReqNome = (TextBox)grpRequerente.Controls["txtNome"];
            cmbReqEmpresa = (ComboBox)grpRequerente.Controls["cmbEmpresa"];
            txtReqEmail = (TextBox)grpRequerente.Controls["txtEmail"];
            txtReqTelefone = (TextBox)grpRequerente.Controls["txtTelefone"];

            this.Controls.Add(grpCedente);
            this.Controls.Add(grpRequerente);
        }

        private GroupBox CriarGrupoClean(string titulo, int x, int y, Font fLabel, Font fNormal)
        {
            GroupBox grp = new GroupBox { Text = titulo, Location = new Point(x, y), Size = new Size(370, 260), Font = new Font("Segoe UI", 10, FontStyle.Bold), ForeColor = Color.FromArgb(64, 64, 64) };

            // Cores Clean
            Color backBox = Color.White;
            Color foreBox = Color.Black;

            int ly = 35; int dy = 25; // Espaçamento vertical

            // Campo Nome
            Label l1 = new Label { Text = "Nome:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray };
            TextBox t1 = new TextBox { Name = "txtNome", Location = new Point(20, ly + 20), Size = new Size(330, 25), BackColor = backBox, ForeColor = foreBox, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };

            ly += 60;
            // Campo Empresa
            Label l2 = new Label { Text = "Empresa:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray };
            ComboBox c2 = new ComboBox { Name = "cmbEmpresa", Location = new Point(20, ly + 20), Size = new Size(330, 25), BackColor = backBox, ForeColor = foreBox, FlatStyle = FlatStyle.System, Font = fNormal };
            c2.Items.AddRange(new string[] { "VOAR AVIATION", "MTX", "EMBRAER", "OUTRA" });

            ly += 60;
            // Campo Email
            Label l3 = new Label { Text = "E-mail:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray };
            TextBox t3 = new TextBox { Name = "txtEmail", Location = new Point(20, ly + 20), Size = new Size(330, 25), BackColor = backBox, ForeColor = foreBox, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };

            ly += 60;
            // Campo Telefone
            Label l4 = new Label { Text = "Telefone:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray };
            TextBox t4 = new TextBox { Name = "txtTelefone", Location = new Point(20, ly + 20), Size = new Size(330, 25), BackColor = backBox, ForeColor = foreBox, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };

            grp.Controls.AddRange(new Control[] { l1, t1, l2, c2, l3, t3, l4, t4 });
            return grp;
        }

        private void FormDadosEmprestimo_Load(object sender, EventArgs e) { }
    }
}