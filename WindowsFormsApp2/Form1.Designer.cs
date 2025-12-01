namespace WindowsFormsApp2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.panelEsquerdo = new System.Windows.Forms.Panel();
            this.btnBuscaAvancada = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnMovimentacao = new System.Windows.Forms.Button();
            this.btnMecanicos = new System.Windows.Forms.Button();
            this.btnExcluir = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNova = new System.Windows.Forms.Button();
            this.btnInstrumentos = new System.Windows.Forms.Button();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.panelSuperior = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.lblBusca = new System.Windows.Forms.Label();
            this.txtBusca = new System.Windows.Forms.TextBox();
            this.tabControlPrincipal = new System.Windows.Forms.TabControl();
            this.tabComCalibracao = new System.Windows.Forms.TabPage();
            this.dgvDados = new System.Windows.Forms.DataGridView();
            this.colInstrumento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModelo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdentifSOD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIdentifOficina = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCertificado = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataCalibracao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataVencimento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSituacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExecutante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstalada = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubLocalizacao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFoto = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colObservacoes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMecanico = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCertificadoPDF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabSemCalibracao = new System.Windows.Forms.TabPage();
            this.dgvSemCalibracao = new System.Windows.Forms.DataGridView();
            this.colSemDescricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemPN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemFabricante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemCadastroLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemCodLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSemMecanico = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelEsquerdo.SuspendLayout();
            this.panelSuperior.SuspendLayout();
            this.panelPrincipal.SuspendLayout();
            this.tabControlPrincipal.SuspendLayout();
            this.tabComCalibracao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).BeginInit();
            this.tabSemCalibracao.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSemCalibracao)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEsquerdo
            // 
            this.panelEsquerdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelEsquerdo.Controls.Add(this.btnBuscaAvancada);
            this.panelEsquerdo.Controls.Add(this.btnRelatorios);
            this.panelEsquerdo.Controls.Add(this.btnFiltrar);
            this.panelEsquerdo.Controls.Add(this.btnMovimentacao);
            this.panelEsquerdo.Controls.Add(this.btnMecanicos);
            this.panelEsquerdo.Controls.Add(this.btnExcluir);
            this.panelEsquerdo.Controls.Add(this.btnEditar);
            this.panelEsquerdo.Controls.Add(this.btnNova);
            this.panelEsquerdo.Controls.Add(this.btnInstrumentos);
            this.panelEsquerdo.Controls.Add(this.btnProcurar);
            this.panelEsquerdo.Controls.Add(this.btnConfiguracoes);
            this.panelEsquerdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEsquerdo.Location = new System.Drawing.Point(0, 49);
            this.panelEsquerdo.Margin = new System.Windows.Forms.Padding(4);
            this.panelEsquerdo.Name = "panelEsquerdo";
            this.panelEsquerdo.Size = new System.Drawing.Size(267, 641);
            this.panelEsquerdo.TabIndex = 0;
            // 
            // btnBuscaAvancada
            // 
            this.btnBuscaAvancada.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBuscaAvancada.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnBuscaAvancada.FlatAppearance.BorderSize = 0;
            this.btnBuscaAvancada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuscaAvancada.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnBuscaAvancada.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnBuscaAvancada.Location = new System.Drawing.Point(0, 441);
            this.btnBuscaAvancada.Margin = new System.Windows.Forms.Padding(4);
            this.btnBuscaAvancada.Name = "btnBuscaAvancada";
            this.btnBuscaAvancada.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnBuscaAvancada.Size = new System.Drawing.Size(267, 49);
            this.btnBuscaAvancada.TabIndex = 9;
            this.btnBuscaAvancada.Text = "Busca Avançada";
            this.btnBuscaAvancada.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscaAvancada.UseVisualStyleBackColor = false;
            this.btnBuscaAvancada.Click += new System.EventHandler(this.btnBuscaAvancada_Click);
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRelatorios.FlatAppearance.BorderSize = 0;
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRelatorios.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnRelatorios.Location = new System.Drawing.Point(0, 392);
            this.btnRelatorios.Margin = new System.Windows.Forms.Padding(4);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnRelatorios.Size = new System.Drawing.Size(267, 49);
            this.btnRelatorios.TabIndex = 8;
            this.btnRelatorios.Text = "Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.UseVisualStyleBackColor = false;
            this.btnRelatorios.Click += new System.EventHandler(this.btnRelatorios_Click);
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFiltrar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFiltrar.FlatAppearance.BorderSize = 0;
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnFiltrar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnFiltrar.Location = new System.Drawing.Point(0, 343);
            this.btnFiltrar.Margin = new System.Windows.Forms.Padding(4);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnFiltrar.Size = new System.Drawing.Size(267, 49);
            this.btnFiltrar.TabIndex = 7;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFiltrar.UseVisualStyleBackColor = false;
            this.btnFiltrar.Click += new System.EventHandler(this.btnFiltrar_Click);
            // 
            // btnMovimentacao
            // 
            this.btnMovimentacao.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMovimentacao.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMovimentacao.FlatAppearance.BorderSize = 0;
            this.btnMovimentacao.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMovimentacao.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMovimentacao.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnMovimentacao.Location = new System.Drawing.Point(0, 294);
            this.btnMovimentacao.Margin = new System.Windows.Forms.Padding(4);
            this.btnMovimentacao.Name = "btnMovimentacao";
            this.btnMovimentacao.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnMovimentacao.Size = new System.Drawing.Size(267, 49);
            this.btnMovimentacao.TabIndex = 6;
            this.btnMovimentacao.Text = "Movimentação";
            this.btnMovimentacao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMovimentacao.UseVisualStyleBackColor = false;
            this.btnMovimentacao.Click += new System.EventHandler(this.btnMovimentacao_Click);
            // 
            // btnMecanicos
            // 
            this.btnMecanicos.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnMecanicos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnMecanicos.Enabled = false;
            this.btnMecanicos.FlatAppearance.BorderSize = 0;
            this.btnMecanicos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMecanicos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnMecanicos.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnMecanicos.Location = new System.Drawing.Point(0, 245);
            this.btnMecanicos.Margin = new System.Windows.Forms.Padding(4);
            this.btnMecanicos.Name = "btnMecanicos";
            this.btnMecanicos.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnMecanicos.Size = new System.Drawing.Size(267, 49);
            this.btnMecanicos.TabIndex = 5;
            this.btnMecanicos.Text = "Mecânicos";
            this.btnMecanicos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMecanicos.UseVisualStyleBackColor = false;
            this.btnMecanicos.Click += new System.EventHandler(this.btnMecanicos_Click);
            // 
            // btnExcluir
            // 
            this.btnExcluir.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnExcluir.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnExcluir.Enabled = false;
            this.btnExcluir.FlatAppearance.BorderSize = 0;
            this.btnExcluir.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExcluir.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnExcluir.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnExcluir.Location = new System.Drawing.Point(0, 196);
            this.btnExcluir.Margin = new System.Windows.Forms.Padding(4);
            this.btnExcluir.Name = "btnExcluir";
            this.btnExcluir.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnExcluir.Size = new System.Drawing.Size(267, 49);
            this.btnExcluir.TabIndex = 4;
            this.btnExcluir.Text = "Excluir";
            this.btnExcluir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExcluir.UseVisualStyleBackColor = false;
            this.btnExcluir.Click += new System.EventHandler(this.btnExcluir_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEditar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEditar.Enabled = false;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEditar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnEditar.Location = new System.Drawing.Point(0, 147);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(4);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnEditar.Size = new System.Drawing.Size(267, 49);
            this.btnEditar.TabIndex = 4;
            this.btnEditar.Text = "Editar";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnNova
            // 
            this.btnNova.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnNova.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNova.FlatAppearance.BorderSize = 0;
            this.btnNova.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNova.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNova.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnNova.Location = new System.Drawing.Point(0, 98);
            this.btnNova.Margin = new System.Windows.Forms.Padding(4);
            this.btnNova.Name = "btnNova";
            this.btnNova.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnNova.Size = new System.Drawing.Size(267, 49);
            this.btnNova.TabIndex = 3;
            this.btnNova.Text = "Nova";
            this.btnNova.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNova.UseVisualStyleBackColor = false;
            this.btnNova.Click += new System.EventHandler(this.btnNova_Click);
            // 
            // btnInstrumentos
            // 
            this.btnInstrumentos.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnInstrumentos.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnInstrumentos.FlatAppearance.BorderSize = 0;
            this.btnInstrumentos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInstrumentos.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnInstrumentos.ForeColor = System.Drawing.Color.White;
            this.btnInstrumentos.Location = new System.Drawing.Point(0, 49);
            this.btnInstrumentos.Margin = new System.Windows.Forms.Padding(4);
            this.btnInstrumentos.Name = "btnInstrumentos";
            this.btnInstrumentos.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnInstrumentos.Size = new System.Drawing.Size(267, 49);
            this.btnInstrumentos.TabIndex = 1;
            this.btnInstrumentos.Text = "Instrumentos";
            this.btnInstrumentos.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnInstrumentos.UseVisualStyleBackColor = false;
            this.btnInstrumentos.Click += new System.EventHandler(this.btnInstrumentos_Click);
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnProcurar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnProcurar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnProcurar.Location = new System.Drawing.Point(0, 0);
            this.btnProcurar.Margin = new System.Windows.Forms.Padding(4);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnProcurar.Size = new System.Drawing.Size(267, 49);
            this.btnProcurar.TabIndex = 0;
            this.btnProcurar.Text = "Procurar";
            this.btnProcurar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcurar.UseVisualStyleBackColor = false;
            this.btnProcurar.Click += new System.EventHandler(this.btnProcurar_Click);
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnConfiguracoes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnConfiguracoes.FlatAppearance.BorderSize = 0;
            this.btnConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConfiguracoes.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnConfiguracoes.Location = new System.Drawing.Point(0, 592);
            this.btnConfiguracoes.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Padding = new System.Windows.Forms.Padding(13, 0, 0, 0);
            this.btnConfiguracoes.Size = new System.Drawing.Size(267, 49);
            this.btnConfiguracoes.TabIndex = 0;
            this.btnConfiguracoes.Text = "CONFIGURAÇÕES";
            this.btnConfiguracoes.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConfiguracoes.UseVisualStyleBackColor = false;
            // 
            // panelSuperior
            // 
            this.panelSuperior.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.panelSuperior.Controls.Add(this.lblTitulo);
            this.panelSuperior.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSuperior.Location = new System.Drawing.Point(0, 0);
            this.panelSuperior.Margin = new System.Windows.Forms.Padding(4);
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1685, 49);
            this.panelSuperior.TabIndex = 1;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(16, 14);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(388, 23);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "CONTROLE DE CALIBRAÇÃO DE INSTRUMENTOS";
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelPrincipal.Controls.Add(this.lblBusca);
            this.panelPrincipal.Controls.Add(this.txtBusca);
            this.panelPrincipal.Controls.Add(this.tabControlPrincipal);
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(267, 49);
            this.panelPrincipal.Margin = new System.Windows.Forms.Padding(4);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(1418, 641);
            this.panelPrincipal.TabIndex = 2;
            // 
            // lblBusca
            // 
            this.lblBusca.AutoSize = true;
            this.lblBusca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBusca.Location = new System.Drawing.Point(13, 18);
            this.lblBusca.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBusca.Name = "lblBusca";
            this.lblBusca.Size = new System.Drawing.Size(67, 20);
            this.lblBusca.TabIndex = 2;
            this.lblBusca.Text = "Procurar:";
            this.lblBusca.Visible = false;
            // 
            // txtBusca
            // 
            this.txtBusca.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtBusca.Location = new System.Drawing.Point(100, 15);
            this.txtBusca.Margin = new System.Windows.Forms.Padding(4);
            this.txtBusca.Name = "txtBusca";
            this.txtBusca.Size = new System.Drawing.Size(399, 27);
            this.txtBusca.TabIndex = 1;
            this.txtBusca.Visible = false;
            this.txtBusca.TextChanged += new System.EventHandler(this.txtBusca_TextChanged);
            // 
            // tabControlPrincipal
            // 
            this.tabControlPrincipal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPrincipal.Controls.Add(this.tabComCalibracao);
            this.tabControlPrincipal.Controls.Add(this.tabSemCalibracao);
            this.tabControlPrincipal.Location = new System.Drawing.Point(13, 55);
            this.tabControlPrincipal.Margin = new System.Windows.Forms.Padding(4);
            this.tabControlPrincipal.Name = "tabControlPrincipal";
            this.tabControlPrincipal.SelectedIndex = 0;
            this.tabControlPrincipal.Size = new System.Drawing.Size(1388, 571);
            this.tabControlPrincipal.TabIndex = 0;
            // 
            // tabComCalibracao
            // 
            this.tabComCalibracao.Controls.Add(this.dgvDados);
            this.tabComCalibracao.Location = new System.Drawing.Point(4, 25);
            this.tabComCalibracao.Margin = new System.Windows.Forms.Padding(4);
            this.tabComCalibracao.Name = "tabComCalibracao";
            this.tabComCalibracao.Padding = new System.Windows.Forms.Padding(4);
            this.tabComCalibracao.Size = new System.Drawing.Size(1380, 542);
            this.tabComCalibracao.TabIndex = 0;
            this.tabComCalibracao.Text = "Com Calibração";
            this.tabComCalibracao.UseVisualStyleBackColor = true;
            // 
            // dgvDados
            // 
            this.dgvDados.AllowUserToAddRows = false;
            this.dgvDados.AllowUserToDeleteRows = false;
            this.dgvDados.BackgroundColor = System.Drawing.Color.White;
            this.dgvDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colInstrumento,
            this.colModelo,
            this.colPN,
            this.colSN,
            this.colIdentifSOD,
            this.colIdentifOficina,
            this.colCertificado,
            this.colDataCalibracao,
            this.colDataVencimento,
            this.colSituacao,
            this.colExecutante,
            this.colInstalada,
            this.colLocal,
            this.colSubLocalizacao,
            this.colFoto,
            this.colObservacoes,
            this.colMecanico,
            this.colCertificadoPDF});
            this.dgvDados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDados.Location = new System.Drawing.Point(4, 4);
            this.dgvDados.Margin = new System.Windows.Forms.Padding(4);
            this.dgvDados.MultiSelect = false;
            this.dgvDados.Name = "dgvDados";
            this.dgvDados.ReadOnly = true;
            this.dgvDados.RowHeadersVisible = false;
            this.dgvDados.RowHeadersWidth = 51;
            this.dgvDados.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDados.Size = new System.Drawing.Size(1372, 534);
            this.dgvDados.TabIndex = 0;
            this.dgvDados.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDados_CellDoubleClick);
            this.dgvDados.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvDados_CellFormatting);
            // 
            // colInstrumento
            // 
            this.colInstrumento.HeaderText = "INSTRUMENTO / FERRAMENTA";
            this.colInstrumento.MinimumWidth = 6;
            this.colInstrumento.Name = "colInstrumento";
            this.colInstrumento.ReadOnly = true;
            this.colInstrumento.Width = 180;
            // 
            // colModelo
            // 
            this.colModelo.HeaderText = "MODELO";
            this.colModelo.MinimumWidth = 6;
            this.colModelo.Name = "colModelo";
            this.colModelo.ReadOnly = true;
            this.colModelo.Width = 80;
            // 
            // colPN
            // 
            this.colPN.HeaderText = "P/N";
            this.colPN.MinimumWidth = 6;
            this.colPN.Name = "colPN";
            this.colPN.ReadOnly = true;
            this.colPN.Width = 80;
            // 
            // colSN
            // 
            this.colSN.HeaderText = "S/N";
            this.colSN.MinimumWidth = 6;
            this.colSN.Name = "colSN";
            this.colSN.ReadOnly = true;
            this.colSN.Width = 80;
            // 
            // colIdentifSOD
            // 
            this.colIdentifSOD.HeaderText = "IDENTIF. SOD.";
            this.colIdentifSOD.MinimumWidth = 6;
            this.colIdentifSOD.Name = "colIdentifSOD";
            this.colIdentifSOD.ReadOnly = true;
            this.colIdentifSOD.Width = 90;
            // 
            // colIdentifOficina
            // 
            this.colIdentifOficina.HeaderText = "IDENTIF. OFICINA";
            this.colIdentifOficina.MinimumWidth = 6;
            this.colIdentifOficina.Name = "colIdentifOficina";
            this.colIdentifOficina.ReadOnly = true;
            this.colIdentifOficina.Width = 90;
            // 
            // colCertificado
            // 
            this.colCertificado.HeaderText = "CERTIFICADO";
            this.colCertificado.MinimumWidth = 6;
            this.colCertificado.Name = "colCertificado";
            this.colCertificado.ReadOnly = true;
            this.colCertificado.Width = 80;
            // 
            // colDataCalibracao
            // 
            this.colDataCalibracao.HeaderText = "DATA DE CALIBRAÇÃO";
            this.colDataCalibracao.MinimumWidth = 6;
            this.colDataCalibracao.Name = "colDataCalibracao";
            this.colDataCalibracao.ReadOnly = true;
            this.colDataCalibracao.Width = 90;
            // 
            // colDataVencimento
            // 
            this.colDataVencimento.HeaderText = "DATA DE VENCIMENTO";
            this.colDataVencimento.MinimumWidth = 6;
            this.colDataVencimento.Name = "colDataVencimento";
            this.colDataVencimento.ReadOnly = true;
            this.colDataVencimento.Width = 90;
            // 
            // colSituacao
            // 
            this.colSituacao.HeaderText = "SITUAÇÃO";
            this.colSituacao.MinimumWidth = 6;
            this.colSituacao.Name = "colSituacao";
            this.colSituacao.ReadOnly = true;
            this.colSituacao.Width = 90;
            // 
            // colExecutante
            // 
            this.colExecutante.HeaderText = "EXECUTANTE";
            this.colExecutante.MinimumWidth = 6;
            this.colExecutante.Name = "colExecutante";
            this.colExecutante.ReadOnly = true;
            this.colExecutante.Width = 80;
            // 
            // colInstalada
            // 
            this.colInstalada.HeaderText = "INSTALADA NO EQUIPAMENTO?";
            this.colInstalada.MinimumWidth = 6;
            this.colInstalada.Name = "colInstalada";
            this.colInstalada.ReadOnly = true;
            this.colInstalada.Width = 90;
            // 
            // colLocal
            // 
            this.colLocal.HeaderText = "LOCAL";
            this.colLocal.MinimumWidth = 6;
            this.colLocal.Name = "colLocal";
            this.colLocal.ReadOnly = true;
            this.colLocal.Width = 90;
            // 
            // colSubLocalizacao
            // 
            this.colSubLocalizacao.HeaderText = "SUB.LOCALIZAÇÃO";
            this.colSubLocalizacao.MinimumWidth = 6;
            this.colSubLocalizacao.Name = "colSubLocalizacao";
            this.colSubLocalizacao.ReadOnly = true;
            this.colSubLocalizacao.Width = 125;
            // 
            // colFoto
            // 
            this.colFoto.HeaderText = "FOTO";
            this.colFoto.MinimumWidth = 6;
            this.colFoto.Name = "colFoto";
            this.colFoto.ReadOnly = true;
            this.colFoto.Width = 125;
            // 
            // colObservacoes
            // 
            this.colObservacoes.HeaderText = "OBSERVACOES";
            this.colObservacoes.MinimumWidth = 6;
            this.colObservacoes.Name = "colObservacoes";
            this.colObservacoes.ReadOnly = true;
            this.colObservacoes.Width = 125;
            // 
            // colMecanico
            // 
            this.colMecanico.HeaderText = "MECANICO";
            this.colMecanico.MinimumWidth = 6;
            this.colMecanico.Name = "colMecanico";
            this.colMecanico.ReadOnly = true;
            this.colMecanico.Width = 125;
            // 
            // colCertificadoPDF
            // 
            this.colCertificadoPDF.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colCertificadoPDF.HeaderText = "CERTIFICADO_PDF";
            this.colCertificadoPDF.MinimumWidth = 6;
            this.colCertificadoPDF.Name = "colCertificadoPDF";
            this.colCertificadoPDF.ReadOnly = true;
            // 
            // tabSemCalibracao
            // 
            this.tabSemCalibracao.Controls.Add(this.dgvSemCalibracao);
            this.tabSemCalibracao.Location = new System.Drawing.Point(4, 25);
            this.tabSemCalibracao.Margin = new System.Windows.Forms.Padding(4);
            this.tabSemCalibracao.Name = "tabSemCalibracao";
            this.tabSemCalibracao.Padding = new System.Windows.Forms.Padding(4);
            this.tabSemCalibracao.Size = new System.Drawing.Size(1380, 542);
            this.tabSemCalibracao.TabIndex = 1;
            this.tabSemCalibracao.Text = "Sem Calibração";
            this.tabSemCalibracao.UseVisualStyleBackColor = true;
            // 
            // dgvSemCalibracao
            // 
            this.dgvSemCalibracao.AllowUserToAddRows = false;
            this.dgvSemCalibracao.AllowUserToDeleteRows = false;
            this.dgvSemCalibracao.BackgroundColor = System.Drawing.Color.White;
            this.dgvSemCalibracao.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSemCalibracao.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSemDescricao,
            this.colSemCodigo,
            this.colSemPN,
            this.colSemFabricante,
            this.colSemLocal,
            this.colSemCadastroLocal,
            this.colSemCodLocal,
            this.colSemStatus,
            this.colSemMecanico});
            this.dgvSemCalibracao.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSemCalibracao.Location = new System.Drawing.Point(4, 4);
            this.dgvSemCalibracao.Margin = new System.Windows.Forms.Padding(4);
            this.dgvSemCalibracao.MultiSelect = false;
            this.dgvSemCalibracao.Name = "dgvSemCalibracao";
            this.dgvSemCalibracao.ReadOnly = true;
            this.dgvSemCalibracao.RowHeadersVisible = false;
            this.dgvSemCalibracao.RowHeadersWidth = 51;
            this.dgvSemCalibracao.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSemCalibracao.Size = new System.Drawing.Size(1372, 534);
            this.dgvSemCalibracao.TabIndex = 0;
            this.dgvSemCalibracao.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSemCalibracao_CellDoubleClick);
            // 
            // colSemDescricao
            // 
            this.colSemDescricao.HeaderText = "DESCRIÇÃO";
            this.colSemDescricao.MinimumWidth = 6;
            this.colSemDescricao.Name = "colSemDescricao";
            this.colSemDescricao.ReadOnly = true;
            this.colSemDescricao.Width = 125;
            // 
            // colSemCodigo
            // 
            this.colSemCodigo.HeaderText = "CÓDIGO";
            this.colSemCodigo.MinimumWidth = 6;
            this.colSemCodigo.Name = "colSemCodigo";
            this.colSemCodigo.ReadOnly = true;
            this.colSemCodigo.Width = 125;
            // 
            // colSemPN
            // 
            this.colSemPN.HeaderText = "PN";
            this.colSemPN.MinimumWidth = 6;
            this.colSemPN.Name = "colSemPN";
            this.colSemPN.ReadOnly = true;
            this.colSemPN.Width = 125;
            // 
            // colSemFabricante
            // 
            this.colSemFabricante.HeaderText = "FABRICANTE";
            this.colSemFabricante.MinimumWidth = 6;
            this.colSemFabricante.Name = "colSemFabricante";
            this.colSemFabricante.ReadOnly = true;
            this.colSemFabricante.Width = 125;
            // 
            // colSemLocal
            // 
            this.colSemLocal.HeaderText = "LOCAL";
            this.colSemLocal.MinimumWidth = 6;
            this.colSemLocal.Name = "colSemLocal";
            this.colSemLocal.ReadOnly = true;
            this.colSemLocal.Width = 125;
            // 
            // colSemCadastroLocal
            // 
            this.colSemCadastroLocal.HeaderText = "CADASTRO LOCAL";
            this.colSemCadastroLocal.MinimumWidth = 6;
            this.colSemCadastroLocal.Name = "colSemCadastroLocal";
            this.colSemCadastroLocal.ReadOnly = true;
            this.colSemCadastroLocal.Width = 125;
            // 
            // colSemCodLocal
            // 
            this.colSemCodLocal.HeaderText = "COD LOCAL";
            this.colSemCodLocal.MinimumWidth = 6;
            this.colSemCodLocal.Name = "colSemCodLocal";
            this.colSemCodLocal.ReadOnly = true;
            this.colSemCodLocal.Width = 125;
            // 
            // colSemStatus
            // 
            this.colSemStatus.HeaderText = "STATUS";
            this.colSemStatus.MinimumWidth = 6;
            this.colSemStatus.Name = "colSemStatus";
            this.colSemStatus.ReadOnly = true;
            this.colSemStatus.Width = 125;
            // 
            // colSemMecanico
            // 
            this.colSemMecanico.HeaderText = "MECANICO";
            this.colSemMecanico.MinimumWidth = 6;
            this.colSemMecanico.Name = "colSemMecanico";
            this.colSemMecanico.ReadOnly = true;
            this.colSemMecanico.Width = 125;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1685, 690);
            this.Controls.Add(this.panelPrincipal);
            this.Controls.Add(this.panelEsquerdo);
            this.Controls.Add(this.panelSuperior);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CONTROLE DE CALIBRAÇÃO DE INSTRUMENTOS";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelEsquerdo.ResumeLayout(false);
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            this.panelPrincipal.ResumeLayout(false);
            this.panelPrincipal.PerformLayout();
            this.tabControlPrincipal.ResumeLayout(false);
            this.tabComCalibracao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).EndInit();
            this.tabSemCalibracao.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSemCalibracao)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEsquerdo;
        private System.Windows.Forms.Button btnBuscaAvancada;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnMovimentacao;
        private System.Windows.Forms.Button btnMecanicos;
        private System.Windows.Forms.Button btnExcluir;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnNova;
        private System.Windows.Forms.Button btnInstrumentos;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.Panel panelSuperior;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Panel panelPrincipal;
        private System.Windows.Forms.Label lblBusca;
        private System.Windows.Forms.TextBox txtBusca;

        private System.Windows.Forms.TabControl tabControlPrincipal;
        private System.Windows.Forms.TabPage tabComCalibracao;
        private System.Windows.Forms.DataGridView dgvDados;
        private System.Windows.Forms.TabPage tabSemCalibracao;
        private System.Windows.Forms.DataGridView dgvSemCalibracao;

        private System.Windows.Forms.DataGridViewTextBoxColumn colInstrumento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModelo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdentifSOD;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIdentifOficina;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCertificado;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataCalibracao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataVencimento;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSituacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExecutante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstalada;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubLocalizacao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFoto;
        private System.Windows.Forms.DataGridViewTextBoxColumn colObservacoes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMecanico;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCertificadoPDF;

        // Colunas da Nova Tabela
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemDescricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemFabricante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemCadastroLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemCodLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSemMecanico;
    }
}