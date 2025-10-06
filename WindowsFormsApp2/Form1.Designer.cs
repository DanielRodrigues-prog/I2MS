namespace WindowsFormsApp2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelEsquerdo = new System.Windows.Forms.Panel();
            this.btnBuscaAvancada = new System.Windows.Forms.Button();
            this.btnRelatorios = new System.Windows.Forms.Button();
            this.btnFiltrar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNova = new System.Windows.Forms.Button();
            this.btnOrdensServico = new System.Windows.Forms.Button();
            this.btnProcurar = new System.Windows.Forms.Button();
            this.btnConfiguracoes = new System.Windows.Forms.Button();
            this.panelSuperior = new System.Windows.Forms.Panel();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.dgvOrdensServico = new System.Windows.Forms.DataGridView();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Abertura = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Fechamento = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prazo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Artigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TipoArtigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cliente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radTodas = new System.Windows.Forms.RadioButton();
            this.radAbertas = new System.Windows.Forms.RadioButton();
            this.radEncerradas = new System.Windows.Forms.RadioButton();
            this.panelPrincipal = new System.Windows.Forms.Panel();
            this.panelEsquerdo.SuspendLayout();
            this.panelSuperior.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdensServico)).BeginInit();
            this.panelPrincipal.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEsquerdo
            // 
            this.panelEsquerdo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panelEsquerdo.Controls.Add(this.btnBuscaAvancada);
            this.panelEsquerdo.Controls.Add(this.btnRelatorios);
            this.panelEsquerdo.Controls.Add(this.btnFiltrar);
            this.panelEsquerdo.Controls.Add(this.btnEditar);
            this.panelEsquerdo.Controls.Add(this.btnNova);
            this.panelEsquerdo.Controls.Add(this.btnOrdensServico);
            this.panelEsquerdo.Controls.Add(this.btnProcurar);
            this.panelEsquerdo.Controls.Add(this.btnConfiguracoes);
            this.panelEsquerdo.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEsquerdo.Location = new System.Drawing.Point(0, 40);
            this.panelEsquerdo.Name = "panelEsquerdo";
            this.panelEsquerdo.Size = new System.Drawing.Size(200, 521);
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
            this.btnBuscaAvancada.Location = new System.Drawing.Point(0, 280);
            this.btnBuscaAvancada.Name = "btnBuscaAvancada";
            this.btnBuscaAvancada.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnBuscaAvancada.Size = new System.Drawing.Size(200, 40);
            this.btnBuscaAvancada.TabIndex = 7;
            this.btnBuscaAvancada.Text = "Busca Avançada";
            this.btnBuscaAvancada.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBuscaAvancada.UseVisualStyleBackColor = false;
            // 
            // btnRelatorios
            // 
            this.btnRelatorios.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnRelatorios.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnRelatorios.FlatAppearance.BorderSize = 0;
            this.btnRelatorios.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRelatorios.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnRelatorios.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnRelatorios.Location = new System.Drawing.Point(0, 240);
            this.btnRelatorios.Name = "btnRelatorios";
            this.btnRelatorios.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnRelatorios.Size = new System.Drawing.Size(200, 40);
            this.btnRelatorios.TabIndex = 6;
            this.btnRelatorios.Text = "Relatórios";
            this.btnRelatorios.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRelatorios.UseVisualStyleBackColor = false;
            // 
            // btnFiltrar
            // 
            this.btnFiltrar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnFiltrar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnFiltrar.FlatAppearance.BorderSize = 0;
            this.btnFiltrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFiltrar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnFiltrar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnFiltrar.Location = new System.Drawing.Point(0, 200);
            this.btnFiltrar.Name = "btnFiltrar";
            this.btnFiltrar.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnFiltrar.Size = new System.Drawing.Size(200, 40);
            this.btnFiltrar.TabIndex = 5;
            this.btnFiltrar.Text = "Filtrar";
            this.btnFiltrar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFiltrar.UseVisualStyleBackColor = false;
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnEditar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnEditar.FlatAppearance.BorderSize = 0;
            this.btnEditar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnEditar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnEditar.Location = new System.Drawing.Point(0, 160);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnEditar.Size = new System.Drawing.Size(200, 40);
            this.btnEditar.TabIndex = 4;
            this.btnEditar.Text = "Editar";
            this.btnEditar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnEditar.UseVisualStyleBackColor = false;
            // 
            // btnNova
            // 
            this.btnNova.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnNova.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnNova.FlatAppearance.BorderSize = 0;
            this.btnNova.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNova.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnNova.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnNova.Location = new System.Drawing.Point(0, 120);
            this.btnNova.Name = "btnNova";
            this.btnNova.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnNova.Size = new System.Drawing.Size(200, 40);
            this.btnNova.TabIndex = 3;
            this.btnNova.Text = "Nova";
            this.btnNova.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNova.UseVisualStyleBackColor = false;
            // 
            // btnOrdensServico
            // 
            this.btnOrdensServico.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnOrdensServico.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnOrdensServico.FlatAppearance.BorderSize = 0;
            this.btnOrdensServico.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOrdensServico.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnOrdensServico.ForeColor = System.Drawing.Color.White;
            this.btnOrdensServico.Location = new System.Drawing.Point(0, 80);
            this.btnOrdensServico.Name = "btnOrdensServico";
            this.btnOrdensServico.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnOrdensServico.Size = new System.Drawing.Size(200, 40);
            this.btnOrdensServico.TabIndex = 2;
            this.btnOrdensServico.Text = "Ordens de Serviço";
            this.btnOrdensServico.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOrdensServico.UseVisualStyleBackColor = false;
            // 
            // btnProcurar
            // 
            this.btnProcurar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnProcurar.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnProcurar.FlatAppearance.BorderSize = 0;
            this.btnProcurar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcurar.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnProcurar.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnProcurar.Location = new System.Drawing.Point(0, 40);
            this.btnProcurar.Name = "btnProcurar";
            this.btnProcurar.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnProcurar.Size = new System.Drawing.Size(200, 40);
            this.btnProcurar.TabIndex = 1;
            this.btnProcurar.Text = "Procurar";
            this.btnProcurar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProcurar.UseVisualStyleBackColor = false;
            // 
            // btnConfiguracoes
            // 
            this.btnConfiguracoes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.btnConfiguracoes.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnConfiguracoes.FlatAppearance.BorderSize = 0;
            this.btnConfiguracoes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfiguracoes.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnConfiguracoes.ForeColor = System.Drawing.Color.Gainsboro;
            this.btnConfiguracoes.Location = new System.Drawing.Point(0, 0);
            this.btnConfiguracoes.Name = "btnConfiguracoes";
            this.btnConfiguracoes.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnConfiguracoes.Size = new System.Drawing.Size(200, 40);
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
            this.panelSuperior.Name = "panelSuperior";
            this.panelSuperior.Size = new System.Drawing.Size(1184, 40);
            this.panelSuperior.TabIndex = 1;
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.ForeColor = System.Drawing.Color.White;
            this.lblTitulo.Location = new System.Drawing.Point(12, 13);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(227, 13);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "I2MS - INDEPENDENT MAINTENANCE SYSTEM";
            // 
            // dgvOrdensServico
            // 
            this.dgvOrdensServico.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvOrdensServico.BackgroundColor = System.Drawing.Color.White;
            this.dgvOrdensServico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrdensServico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Status,
            this.OS,
            this.Abertura,
            this.Fechamento,
            this.Prazo,
            this.OM,
            this.Artigo,
            this.TipoArtigo,
            this.Cliente,
            this.Descricao});
            this.dgvOrdensServico.Location = new System.Drawing.Point(10, 50);
            this.dgvOrdensServico.Name = "dgvOrdensServico";
            this.dgvOrdensServico.RowHeadersVisible = false;
            this.dgvOrdensServico.Size = new System.Drawing.Size(952, 459);
            this.dgvOrdensServico.TabIndex = 2;
            // 
            // Status
            // 
            this.Status.HeaderText = "STATUS";
            this.Status.Name = "Status";
            // 
            // OS
            // 
            this.OS.HeaderText = "OS";
            this.OS.Name = "OS";
            // 
            // Abertura
            // 
            this.Abertura.HeaderText = "ABERTURA";
            this.Abertura.Name = "Abertura";
            // 
            // Fechamento
            // 
            this.Fechamento.HeaderText = "FECHAMENTO";
            this.Fechamento.Name = "Fechamento";
            // 
            // Prazo
            // 
            this.Prazo.HeaderText = "PRAZO";
            this.Prazo.Name = "Prazo";
            // 
            // OM
            // 
            this.OM.HeaderText = "OM";
            this.OM.Name = "OM";
            // 
            // Artigo
            // 
            this.Artigo.HeaderText = "ARTIGO";
            this.Artigo.Name = "Artigo";
            // 
            // TipoArtigo
            // 
            this.TipoArtigo.HeaderText = "TIPO ARTIGO";
            this.TipoArtigo.Name = "TipoArtigo";
            // 
            // Cliente
            // 
            this.Cliente.HeaderText = "CLIENTE";
            this.Cliente.Name = "Cliente";
            // 
            // Descricao
            // 
            this.Descricao.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Descricao.HeaderText = "DESCRICAO";
            this.Descricao.Name = "Descricao";
            // 
            // radTodas
            // 
            this.radTodas.AutoSize = true;
            this.radTodas.Checked = true;
            this.radTodas.Location = new System.Drawing.Point(20, 15);
            this.radTodas.Name = "radTodas";
            this.radTodas.Size = new System.Drawing.Size(63, 17);
            this.radTodas.TabIndex = 3;
            this.radTodas.TabStop = true;
            this.radTodas.Text = "TODAS";
            this.radTodas.UseVisualStyleBackColor = true;
            // 
            // radAbertas
            // 
            this.radAbertas.AutoSize = true;
            this.radAbertas.Location = new System.Drawing.Point(100, 15);
            this.radAbertas.Name = "radAbertas";
            this.radAbertas.Size = new System.Drawing.Size(72, 17);
            this.radAbertas.TabIndex = 4;
            this.radAbertas.Text = "ABERTAS";
            this.radAbertas.UseVisualStyleBackColor = true;
            // 
            // radEncerradas
            // 
            this.radEncerradas.AutoSize = true;
            this.radEncerradas.Location = new System.Drawing.Point(190, 15);
            this.radEncerradas.Name = "radEncerradas";
            this.radEncerradas.Size = new System.Drawing.Size(99, 17);
            this.radEncerradas.TabIndex = 5;
            this.radEncerradas.Text = "ENCERRADAS";
            this.radEncerradas.UseVisualStyleBackColor = true;
            // 
            // panelPrincipal
            // 
            this.panelPrincipal.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelPrincipal.Controls.Add(this.radEncerradas);
            this.panelPrincipal.Controls.Add(this.radAbertas);
            this.panelPrincipal.Controls.Add(this.radTodas);
            this.panelPrincipal.Controls.Add(this.dgvOrdensServico);
            this.panelPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrincipal.Location = new System.Drawing.Point(200, 40);
            this.panelPrincipal.Name = "panelPrincipal";
            this.panelPrincipal.Size = new System.Drawing.Size(984, 521);
            this.panelPrincipal.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.panelPrincipal);
            this.Controls.Add(this.panelEsquerdo);
            this.Controls.Add(this.panelSuperior);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "I2MS - INDEPENDENT MAINTENANCE SYSTEM";
            this.panelEsquerdo.ResumeLayout(false);
            this.panelSuperior.ResumeLayout(false);
            this.panelSuperior.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrdensServico)).EndInit();
            this.panelPrincipal.ResumeLayout(false);
            this.panelPrincipal.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelEsquerdo;
        private System.Windows.Forms.Panel panelSuperior;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.DataGridView dgvOrdensServico;
        private System.Windows.Forms.RadioButton radTodas;
        private System.Windows.Forms.RadioButton radAbertas;
        private System.Windows.Forms.RadioButton radEncerradas;
        private System.Windows.Forms.Button btnConfiguracoes;
        private System.Windows.Forms.Button btnBuscaAvancada;
        private System.Windows.Forms.Button btnRelatorios;
        private System.Windows.Forms.Button btnFiltrar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnNova;
        private System.Windows.Forms.Button btnOrdensServico;
        private System.Windows.Forms.Button btnProcurar;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn OS;
        private System.Windows.Forms.DataGridViewTextBoxColumn Abertura;
        private System.Windows.Forms.DataGridViewTextBoxColumn Fechamento;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prazo;
        private System.Windows.Forms.DataGridViewTextBoxColumn OM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Artigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn TipoArtigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cliente;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descricao;
        private System.Windows.Forms.Panel panelPrincipal;
    }
}