namespace WindowsFormsApp2
{
    partial class FormDetalhes
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pictureBoxFoto = new System.Windows.Forms.PictureBox();
            this.lblInstrumentoTitulo = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMecanico = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linkCertificado = new System.Windows.Forms.Label();
            this.btnAbrirCertificado = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxFoto
            // 
            this.pictureBoxFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFoto.Location = new System.Drawing.Point(422, 53);
            this.pictureBoxFoto.Name = "pictureBoxFoto";
            this.pictureBoxFoto.Size = new System.Drawing.Size(250, 250);
            this.pictureBoxFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFoto.TabIndex = 0;
            this.pictureBoxFoto.TabStop = false;
            // 
            // lblInstrumentoTitulo
            // 
            this.lblInstrumentoTitulo.AutoSize = true;
            this.lblInstrumentoTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInstrumentoTitulo.Location = new System.Drawing.Point(23, 19);
            this.lblInstrumentoTitulo.Name = "lblInstrumentoTitulo";
            this.lblInstrumentoTitulo.Size = new System.Drawing.Size(196, 25);
            this.lblInstrumentoTitulo.TabIndex = 1;
            this.lblInstrumentoTitulo.Text = "[Nome Instrumento]";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Observações:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.Location = new System.Drawing.Point(28, 84);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.ReadOnly = true;
            this.txtObservacoes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservacoes.Size = new System.Drawing.Size(365, 100);
            this.txtObservacoes.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Em posse do Mecânico:";
            // 
            // txtMecanico
            // 
            this.txtMecanico.Location = new System.Drawing.Point(28, 219);
            this.txtMecanico.Name = "txtMecanico";
            this.txtMecanico.ReadOnly = true;
            this.txtMecanico.Size = new System.Drawing.Size(183, 20);
            this.txtMecanico.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 263);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Certificado de Calibração:";
            // 
            // linkCertificado
            // 
            this.linkCertificado.AutoSize = true;
            this.linkCertificado.Location = new System.Drawing.Point(25, 281);
            this.linkCertificado.Name = "linkCertificado";
            this.linkCertificado.Size = new System.Drawing.Size(76, 13);
            this.linkCertificado.TabIndex = 7;
            this.linkCertificado.Text = "Nenhum (N/A)";
            // 
            // btnAbrirCertificado
            // 
            this.btnAbrirCertificado.Enabled = false;
            this.btnAbrirCertificado.Location = new System.Drawing.Point(28, 309);
            this.btnAbrirCertificado.Name = "btnAbrirCertificado";
            this.btnAbrirCertificado.Size = new System.Drawing.Size(95, 23);
            this.btnAbrirCertificado.TabIndex = 8;
            this.btnAbrirCertificado.Text = "Abrir PDF";
            this.btnAbrirCertificado.UseVisualStyleBackColor = true;
            this.btnAbrirCertificado.Click += new System.EventHandler(this.btnAbrirCertificado_Click);
            // 
            // FormDetalhes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(699, 355);
            this.Controls.Add(this.btnAbrirCertificado);
            this.Controls.Add(this.linkCertificado);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMecanico);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtObservacoes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblInstrumentoTitulo);
            this.Controls.Add(this.pictureBoxFoto);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDetalhes";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Detalhes do Instrumento";
            this.Load += new System.EventHandler(this.FormDetalhes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxFoto;
        private System.Windows.Forms.Label lblInstrumentoTitulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMecanico;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label linkCertificado;
        private System.Windows.Forms.Button btnAbrirCertificado;
    }
}