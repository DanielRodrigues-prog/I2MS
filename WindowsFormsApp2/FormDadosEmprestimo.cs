using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace WindowsFormsApp2
{
    public partial class FormDadosEmprestimo : Form
    {
        // Public properties
        public string Obs => txtObs?.Text ?? "";
        public string CedenteNome => txtCedNome?.Text ?? "";
        public string CedenteEmpresa => cmbCedEmpresa?.Text ?? "";
        public string CedenteEmail => txtCedEmail?.Text ?? "";
        public string CedenteTelefone => txtCedTelefone?.Text ?? "";
        public string RequerenteNome => txtReqNome?.Text ?? "";
        public string RequerenteEmpresa => cmbReqEmpresa?.Text ?? "";
        public string RequerenteEmail => txtReqEmail?.Text ?? "";
        public string RequerenteTelefone => txtReqTelefone?.Text ?? "";

        // UI Controls
        private TextBox txtObs;
        private TextBox txtCedNome, txtCedEmail, txtCedTelefone;
        private ComboBox cmbCedEmpresa;
        private TextBox txtReqNome, txtReqEmail, txtReqTelefone;
        private ComboBox cmbReqEmpresa;
        private ListBox lstFerramentas;

        public FormDadosEmprestimo(List<DataGridViewRow> ferramentasSelecionadas, string usuarioLogado)
        {
            InitializeComponent();

            // Garante que a lista não seja nula
            var listaSegura = ferramentasSelecionadas ?? new List<DataGridViewRow>();

            MontarVisualClean(listaSegura, usuarioLogado);
        }

        private void MontarVisualClean(List<DataGridViewRow> ferramentas, string usuarioLogado)
        {
            // Configurações da Janela
            this.Text = "Empréstimo de Ferramentas";
            this.Size = new Size(840, 600); // Altura menor pois removemos a foto
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            // Fontes
            Font fontTitulo = new Font("Segoe UI", 14, FontStyle.Bold);
            Font fontLabel = new Font("Segoe UI", 9, FontStyle.Regular);
            Font fontBold = new Font("Segoe UI", 9, FontStyle.Bold);
            Font fontNormal = new Font("Segoe UI", 10);

            // --- HEADER ---
            Panel pnlHeader = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = Color.FromArgb(50, 50, 50) };
            Label lblTitulo = new Label { Text = "Novo Empréstimo", ForeColor = Color.White, Font = fontTitulo, AutoSize = true, Location = new Point(20, 15) };

            Button btnGerar = new Button
            {
                Text = "GERAR DOCUMENTO",
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(160, 35),
                Location = new Point(640, 12),
                Cursor = Cursors.Hand
            };
            btnGerar.FlatAppearance.BorderSize = 0;
            btnGerar.Click += (s, e) => { this.DialogResult = DialogResult.OK; this.Close(); };

            pnlHeader.Controls.Add(lblTitulo);
            pnlHeader.Controls.Add(btnGerar);
            this.Controls.Add(pnlHeader);

            // --- CONTEÚDO SUPERIOR ---
            int y = 80;

            // Lista de Ferramentas (Esquerda)
            Label lblFerramentas = new Label { Text = "Itens Selecionados:", Location = new Point(20, y), AutoSize = true, Font = fontBold, ForeColor = Color.DimGray };

            lstFerramentas = new ListBox
            {
                Location = new Point(20, y + 25),
                Size = new Size(380, 120),
                Font = fontNormal,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            foreach (var row in ferramentas)
            {
                string desc = "Item Desconhecido";
                string pn = "N/A";

                if (row.DataGridView != null)
                {
                    if (row.DataGridView.Name == "dgvDados") // Tab Com Calibração
                    {
                        desc = row.Cells["colInstrumento"]?.Value?.ToString() ?? desc;
                        pn = row.Cells["colPN"]?.Value?.ToString() ?? pn;
                    }
                    else // Tab Sem Calibração
                    {
                        desc = row.Cells["colSemDescricao"]?.Value?.ToString() ?? desc;
                        pn = row.Cells["colSemPN"]?.Value?.ToString() ?? pn;
                    }
                }
                lstFerramentas.Items.Add($"{desc} (PN: {pn})");
            }

            // Observações (Direita)
            Label lblObs = new Label { Text = "Observações / O.S.:", Location = new Point(420, y), AutoSize = true, Font = fontBold, ForeColor = Color.DimGray };
            txtObs = new TextBox { Location = new Point(420, y + 25), Size = new Size(380, 120), Multiline = true, Font = fontNormal, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Text = "NA" };

            this.Controls.AddRange(new Control[] { lblFerramentas, lstFerramentas, lblObs, txtObs });

            // --- GRUPOS CEDENTE / REQUERENTE ---
            y += 170;

            // Cedente
            GroupBox grpCedente = CriarGrupoClean("Cedente", 20, y, fontLabel, fontNormal);
            txtCedNome = (TextBox)grpCedente.Controls["txtNome"];
            cmbCedEmpresa = (ComboBox)grpCedente.Controls["cmbEmpresa"];
            txtCedEmail = (TextBox)grpCedente.Controls["txtEmail"];
            txtCedTelefone = (TextBox)grpCedente.Controls["txtTelefone"];

            // Preenchimento Automático
            txtCedNome.Text = usuarioLogado;
            cmbCedEmpresa.Text = "VOAR AVIATION";
            txtCedEmail.Text = "suprimentos@voar.aero";
            txtCedTelefone.Text = "+55 (11) 5070 6000";

            // Requerente
            GroupBox grpRequerente = CriarGrupoClean("Requerente", 420, y, fontLabel, fontNormal);
            txtReqNome = (TextBox)grpRequerente.Controls["txtNome"];
            cmbReqEmpresa = (ComboBox)grpRequerente.Controls["cmbEmpresa"];
            txtReqEmail = (TextBox)grpRequerente.Controls["txtEmail"];
            txtReqTelefone = (TextBox)grpRequerente.Controls["txtTelefone"];

            this.Controls.Add(grpCedente);
            this.Controls.Add(grpRequerente);
        }

        private GroupBox CriarGrupoClean(string titulo, int x, int y, Font fLabel, Font fNormal)
        {
            GroupBox grp = new GroupBox
            {
                Text = titulo,
                Location = new Point(x, y),
                Size = new Size(380, 280),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 64, 64)
            };

            int ly = 40; int spacing = 60;

            // Nome
            grp.Controls.Add(new Label { Text = "Nome:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray });
            TextBox t1 = new TextBox { Name = "txtNome", Location = new Point(20, ly + 20), Size = new Size(340, 25), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };
            grp.Controls.Add(t1);

            ly += spacing;
            // Empresa (Editável)
            grp.Controls.Add(new Label { Text = "Empresa:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray });
            ComboBox c2 = new ComboBox
            {
                Name = "cmbEmpresa",
                Location = new Point(20, ly + 20),
                Size = new Size(340, 25),
                BackColor = Color.White,
                ForeColor = Color.Black,
                FlatStyle = FlatStyle.System,
                Font = fNormal,
                DropDownStyle = ComboBoxStyle.DropDown // Permite digitar
            };
            c2.Items.AddRange(new string[] { "VOAR AVIATION", "OUTRA" });
            grp.Controls.Add(c2);

            ly += spacing;
            // Email
            grp.Controls.Add(new Label { Text = "E-mail:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray });
            TextBox t3 = new TextBox { Name = "txtEmail", Location = new Point(20, ly + 20), Size = new Size(340, 25), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };
            grp.Controls.Add(t3);

            ly += spacing;
            // Telefone
            grp.Controls.Add(new Label { Text = "Telefone:", Location = new Point(20, ly), AutoSize = true, Font = fLabel, ForeColor = Color.DimGray });
            TextBox t4 = new TextBox { Name = "txtTelefone", Location = new Point(20, ly + 20), Size = new Size(340, 25), BackColor = Color.White, ForeColor = Color.Black, BorderStyle = BorderStyle.FixedSingle, Font = fNormal };
            grp.Controls.Add(t4);

            return grp;
        }

        private void FormDadosEmprestimo_Load(object sender, EventArgs e)
        {
            // Empty load method required by designer
        }
    }
}