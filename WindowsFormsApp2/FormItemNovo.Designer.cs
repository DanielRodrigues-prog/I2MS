namespace WindowsFormsApp2
{
    partial class FormItemNovo
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
            this.lblInstrumento = new System.Windows.Forms.Label();
            this.txtInstrumento = new System.Windows.Forms.TextBox();
            this.lblModelo = new System.Windows.Forms.Label();
            this.txtModelo = new System.Windows.Forms.TextBox();
            this.lblPN = new System.Windows.Forms.Label();
            this.txtPN = new System.Windows.Forms.TextBox();
            this.lblSN = new System.Windows.Forms.Label();
            this.txtSN = new System.Windows.Forms.TextBox();
            this.lblIdentifSOD = new System.Windows.Forms.Label();
            this.txtIdentifSOD = new System.Windows.Forms.TextBox();
            this.lblIdentifOficina = new System.Windows.Forms.Label();
            this.txtIdentifOficina = new System.Windows.Forms.TextBox();
            this.lblCertificado = new System.Windows.Forms.Label();
            this.txtCertificado = new System.Windows.Forms.TextBox();
            this.lblDataCalibracao = new System.Windows.Forms.Label();
            this.dtpDataCalibracao = new System.Windows.Forms.DateTimePicker();
            this.lblDataVencimento = new System.Windows.Forms.Label();
            this.dtpDataVencimento = new System.Windows.Forms.DateTimePicker();
            this.lblExecutante = new System.Windows.Forms.Label();
            this.txtExecutante = new System.Windows.Forms.TextBox();
            this.lblInstalada = new System.Windows.Forms.Label();
            this.txtInstalada = new System.Windows.Forms.TextBox();
            this.lblLocal = new System.Windows.Forms.Label();
            this.txtLocal = new System.Windows.Forms.TextBox();
            this.lblSubLocalizacao = new System.Windows.Forms.Label();
            this.txtSubLocalizacao = new System.Windows.Forms.TextBox();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnSelecionarFoto = new System.Windows.Forms.Button();
            this.pictureBoxFoto = new System.Windows.Forms.PictureBox();
            this.txtCaminhoFoto = new System.Windows.Forms.TextBox();
            this.lblObservacoes = new System.Windows.Forms.Label();
            this.txtObservacoes = new System.Windows.Forms.TextBox();
            this.lblMecanico = new System.Windows.Forms.Label();
            this.txtMecanico = new System.Windows.Forms.TextBox();
            this.lblCertificadoPDF = new System.Windows.Forms.Label();
            this.txtCaminhoPDF = new System.Windows.Forms.TextBox();
            this.btnAnexarPDF = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInstrumento
            // 
            this.lblInstrumento.AutoSize = true;
            this.lblInstrumento.Location = new System.Drawing.Point(25, 23);
            this.lblInstrumento.Name = "lblInstrumento";
            this.lblInstrumento.Size = new System.Drawing.Size(172, 13);
            this.lblInstrumento.TabIndex = 0;
            this.lblInstrumento.Text = "INSTRUMENTO / FERRAMENTA";
            // 
            // txtInstrumento
            // 
            this.txtInstrumento.Location = new System.Drawing.Point(28, 39);
            this.txtInstrumento.Name = "txtInstrumento";
            this.txtInstrumento.Size = new System.Drawing.Size(326, 20);
            this.txtInstrumento.TabIndex = 1;
            // 
            // lblModelo
            // 
            this.lblModelo.AutoSize = true;
            this.lblModelo.Location = new System.Drawing.Point(25, 72);
            this.lblModelo.Name = "lblModelo";
            this.lblModelo.Size = new System.Drawing.Size(53, 13);
            this.lblModelo.TabIndex = 2;
            this.lblModelo.Text = "MODELO";
            // 
            // txtModelo
            // 
            this.txtModelo.Location = new System.Drawing.Point(28, 88);
            this.txtModelo.Name = "txtModelo";
            this.txtModelo.Size = new System.Drawing.Size(326, 20);
            this.txtModelo.TabIndex = 3;
            // 
            // lblPN
            // 
            this.lblPN.AutoSize = true;
            this.lblPN.Location = new System.Drawing.Point(25, 121);
            this.lblPN.Name = "lblPN";
            this.lblPN.Size = new System.Drawing.Size(27, 13);
            this.lblPN.TabIndex = 4;
            this.lblPN.Text = "P/N";
            // 
            // txtPN
            // 
            this.txtPN.Location = new System.Drawing.Point(28, 137);
            this.txtPN.Name = "txtPN";
            this.txtPN.Size = new System.Drawing.Size(326, 20);
            this.txtPN.TabIndex = 5;
            // 
            // lblSN
            // 
            this.lblSN.AutoSize = true;
            this.lblSN.Location = new System.Drawing.Point(25, 170);
            this.lblSN.Name = "lblSN";
            this.lblSN.Size = new System.Drawing.Size(27, 13);
            this.lblSN.TabIndex = 6;
            this.lblSN.Text = "S/N";
            // 
            // txtSN
            // 
            this.txtSN.Location = new System.Drawing.Point(28, 186);
            this.txtSN.Name = "txtSN";
            this.txtSN.Size = new System.Drawing.Size(326, 20);
            this.txtSN.TabIndex = 7;
            // 
            // lblIdentifSOD
            // 
            this.lblIdentifSOD.AutoSize = true;
            this.lblIdentifSOD.Location = new System.Drawing.Point(25, 219);
            this.lblIdentifSOD.Name = "lblIdentifSOD";
            this.lblIdentifSOD.Size = new System.Drawing.Size(81, 13);
            this.lblIdentifSOD.TabIndex = 8;
            this.lblIdentifSOD.Text = "IDENTIF. SOD.";
            // 
            // txtIdentifSOD
            // 
            this.txtIdentifSOD.Location = new System.Drawing.Point(28, 235);
            this.txtIdentifSOD.Name = "txtIdentifSOD";
            this.txtIdentifSOD.Size = new System.Drawing.Size(326, 20);
            this.txtIdentifSOD.TabIndex = 9;
            // 
            // lblIdentifOficina
            // 
            this.lblIdentifOficina.AutoSize = true;
            this.lblIdentifOficina.Location = new System.Drawing.Point(25, 268);
            this.lblIdentifOficina.Name = "lblIdentifOficina";
            this.lblIdentifOficina.Size = new System.Drawing.Size(97, 13);
            this.lblIdentifOficina.TabIndex = 10;
            this.lblIdentifOficina.Text = "IDENTIF. OFICINA";
            // 
            // txtIdentifOficina
            // 
            this.txtIdentifOficina.Location = new System.Drawing.Point(28, 284);
            this.txtIdentifOficina.Name = "txtIdentifOficina";
            this.txtIdentifOficina.Size = new System.Drawing.Size(326, 20);
            this.txtIdentifOficina.TabIndex = 11;
            // 
            // lblCertificado
            // 
            this.lblCertificado.AutoSize = true;
            this.lblCertificado.Location = new System.Drawing.Point(25, 317);
            this.lblCertificado.Name = "lblCertificado";
            this.lblCertificado.Size = new System.Drawing.Size(78, 13);
            this.lblCertificado.TabIndex = 12;
            this.lblCertificado.Text = "CERTIFICADO";
            // 
            // txtCertificado
            // 
            this.txtCertificado.Location = new System.Drawing.Point(28, 333);
            this.txtCertificado.Name = "txtCertificado";
            this.txtCertificado.Size = new System.Drawing.Size(326, 20);
            this.txtCertificado.TabIndex = 13;
            // 
            // lblDataCalibracao
            // 
            this.lblDataCalibracao.AutoSize = true;
            this.lblDataCalibracao.Location = new System.Drawing.Point(25, 366);
            this.lblDataCalibracao.Name = "lblDataCalibracao";
            this.lblDataCalibracao.Size = new System.Drawing.Size(124, 13);
            this.lblDataCalibracao.TabIndex = 14;
            this.lblDataCalibracao.Text = "DATA DE CALIBRAÇÃO";
            // 
            // dtpDataCalibracao
            // 
            this.dtpDataCalibracao.Location = new System.Drawing.Point(28, 382);
            this.dtpDataCalibracao.Name = "dtpDataCalibracao";
            this.dtpDataCalibracao.Size = new System.Drawing.Size(220, 20);
            this.dtpDataCalibracao.TabIndex = 15;
            // 
            // lblDataVencimento
            // 
            this.lblDataVencimento.AutoSize = true;
            this.lblDataVencimento.Location = new System.Drawing.Point(384, 366);
            this.lblDataVencimento.Name = "lblDataVencimento";
            this.lblDataVencimento.Size = new System.Drawing.Size(128, 13);
            this.lblDataVencimento.TabIndex = 16;
            this.lblDataVencimento.Text = "DATA DE VENCIMENTO";
            // 
            // dtpDataVencimento
            // 
            this.dtpDataVencimento.Location = new System.Drawing.Point(387, 382);
            this.dtpDataVencimento.Name = "dtpDataVencimento";
            this.dtpDataVencimento.Size = new System.Drawing.Size(220, 20);
            this.dtpDataVencimento.TabIndex = 17;
            // 
            // lblExecutante
            // 
            this.lblExecutante.AutoSize = true;
            this.lblExecutante.Location = new System.Drawing.Point(384, 253);
            this.lblExecutante.Name = "lblExecutante";
            this.lblExecutante.Size = new System.Drawing.Size(79, 13);
            this.lblExecutante.TabIndex = 18;
            this.lblExecutante.Text = "EXECUTANTE";
            // 
            // txtExecutante
            // 
            this.txtExecutante.Location = new System.Drawing.Point(387, 269);
            this.txtExecutante.Name = "txtExecutante";
            this.txtExecutante.Size = new System.Drawing.Size(266, 20);
            this.txtExecutante.TabIndex = 19;
            // 
            // lblInstalada
            // 
            this.lblInstalada.AutoSize = true;
            this.lblInstalada.Location = new System.Drawing.Point(384, 308);
            this.lblInstalada.Name = "lblInstalada";
            this.lblInstalada.Size = new System.Drawing.Size(174, 13);
            this.lblInstalada.TabIndex = 20;
            this.lblInstalada.Text = "INSTALADA NO EQUIPAMENTO?";
            // 
            // txtInstalada
            // 
            this.txtInstalada.Location = new System.Drawing.Point(387, 324);
            this.txtInstalada.Name = "txtInstalada";
            this.txtInstalada.Size = new System.Drawing.Size(266, 20);
            this.txtInstalada.TabIndex = 21;
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.Location = new System.Drawing.Point(384, 193);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(41, 13);
            this.lblLocal.TabIndex = 22;
            this.lblLocal.Text = "LOCAL";
            // 
            // txtLocal
            // 
            this.txtLocal.Location = new System.Drawing.Point(387, 209);
            this.txtLocal.Name = "txtLocal";
            this.txtLocal.Size = new System.Drawing.Size(266, 20);
            this.txtLocal.TabIndex = 23;
            // 
            // lblSubLocalizacao
            // 
            this.lblSubLocalizacao.AutoSize = true;
            this.lblSubLocalizacao.Location = new System.Drawing.Point(384, 23);
            this.lblSubLocalizacao.Name = "lblSubLocalizacao";
            this.lblSubLocalizacao.Size = new System.Drawing.Size(105, 13);
            this.lblSubLocalizacao.TabIndex = 24;
            this.lblSubLocalizacao.Text = "SUB.LOCALIZAÇÃO";
            // 
            // txtSubLocalizacao
            // 
            this.txtSubLocalizacao.Location = new System.Drawing.Point(387, 39);
            this.txtSubLocalizacao.Name = "txtSubLocalizacao";
            this.txtSubLocalizacao.Size = new System.Drawing.Size(266, 20);
            this.txtSubLocalizacao.TabIndex = 25;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(497, 540);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(75, 23);
            this.btnSalvar.TabIndex = 26;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // btnCancelar
            // 
            this.btnCancelar.Location = new System.Drawing.Point(578, 540);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 23);
            this.btnCancelar.TabIndex = 27;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnSelecionarFoto
            // 
            this.btnSelecionarFoto.Location = new System.Drawing.Point(387, 417);
            this.btnSelecionarFoto.Name = "btnSelecionarFoto";
            this.btnSelecionarFoto.Size = new System.Drawing.Size(121, 23);
            this.btnSelecionarFoto.TabIndex = 28;
            this.btnSelecionarFoto.Text = "Selecionar Foto...";
            this.btnSelecionarFoto.UseVisualStyleBackColor = true;
            this.btnSelecionarFoto.Click += new System.EventHandler(this.btnSelecionarFoto_Click);
            // 
            // pictureBoxFoto
            // 
            this.pictureBoxFoto.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxFoto.Location = new System.Drawing.Point(514, 417);
            this.pictureBoxFoto.Name = "pictureBoxFoto";
            this.pictureBoxFoto.Size = new System.Drawing.Size(139, 100);
            this.pictureBoxFoto.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxFoto.TabIndex = 29;
            this.pictureBoxFoto.TabStop = false;
            // 
            // txtCaminhoFoto
            // 
            this.txtCaminhoFoto.Location = new System.Drawing.Point(387, 446);
            this.txtCaminhoFoto.Name = "txtCaminhoFoto";
            this.txtCaminhoFoto.ReadOnly = true;
            this.txtCaminhoFoto.Size = new System.Drawing.Size(121, 20);
            this.txtCaminhoFoto.TabIndex = 30;
            this.txtCaminhoFoto.Visible = false;
            // 
            // lblObservacoes
            // 
            this.lblObservacoes.AutoSize = true;
            this.lblObservacoes.Location = new System.Drawing.Point(384, 72);
            this.lblObservacoes.Name = "lblObservacoes";
            this.lblObservacoes.Size = new System.Drawing.Size(73, 13);
            this.lblObservacoes.TabIndex = 31;
            this.lblObservacoes.Text = "Observações:";
            // 
            // txtObservacoes
            // 
            this.txtObservacoes.Location = new System.Drawing.Point(387, 88);
            this.txtObservacoes.Multiline = true;
            this.txtObservacoes.Name = "txtObservacoes";
            this.txtObservacoes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtObservacoes.Size = new System.Drawing.Size(266, 88);
            this.txtObservacoes.TabIndex = 32;
            // 
            // lblMecanico
            // 
            this.lblMecanico.AutoSize = true;
            this.lblMecanico.Location = new System.Drawing.Point(25, 484);
            this.lblMecanico.Name = "lblMecanico";
            this.lblMecanico.Size = new System.Drawing.Size(123, 13);
            this.lblMecanico.TabIndex = 33;
            this.lblMecanico.Text = "Mecânico (ID ou Nome):";
            // 
            // txtMecanico
            // 
            this.txtMecanico.Location = new System.Drawing.Point(28, 500);
            this.txtMecanico.Name = "txtMecanico";
            this.txtMecanico.Size = new System.Drawing.Size(326, 20);
            this.txtMecanico.TabIndex = 34;
            // 
            // lblCertificadoPDF
            // 
            this.lblCertificadoPDF.AutoSize = true;
            this.lblCertificadoPDF.Location = new System.Drawing.Point(384, 484);
            this.lblCertificadoPDF.Name = "lblCertificadoPDF";
            this.lblCertificadoPDF.Size = new System.Drawing.Size(90, 13);
            this.lblCertificadoPDF.TabIndex = 35;
            this.lblCertificadoPDF.Text = "Certificado (PDF):";
            // 
            // txtCaminhoPDF
            // 
            this.txtCaminhoPDF.Location = new System.Drawing.Point(387, 500);
            this.txtCaminhoPDF.Name = "txtCaminhoPDF";
            this.txtCaminhoPDF.ReadOnly = true;
            this.txtCaminhoPDF.Size = new System.Drawing.Size(180, 20);
            this.txtCaminhoPDF.TabIndex = 36;
            // 
            // btnAnexarPDF
            // 
            this.btnAnexarPDF.Location = new System.Drawing.Point(573, 498);
            this.btnAnexarPDF.Name = "btnAnexarPDF";
            this.btnAnexarPDF.Size = new System.Drawing.Size(80, 23);
            this.btnAnexarPDF.TabIndex = 37;
            this.btnAnexarPDF.Text = "Anexar PDF...";
            this.btnAnexarPDF.UseVisualStyleBackColor = true;
            this.btnAnexarPDF.Click += new System.EventHandler(this.btnAnexarPDF_Click);
            // 
            // FormItemNovo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 575);
            this.Controls.Add(this.btnAnexarPDF);
            this.Controls.Add(this.txtCaminhoPDF);
            this.Controls.Add(this.lblCertificadoPDF);
            this.Controls.Add(this.txtMecanico);
            this.Controls.Add(this.lblMecanico);
            this.Controls.Add(this.txtObservacoes);
            this.Controls.Add(this.lblObservacoes);
            this.Controls.Add(this.txtCaminhoFoto);
            this.Controls.Add(this.pictureBoxFoto);
            this.Controls.Add(this.btnSelecionarFoto);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.txtSubLocalizacao);
            this.Controls.Add(this.lblSubLocalizacao);
            this.Controls.Add(this.txtLocal);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.txtInstalada);
            this.Controls.Add(this.lblInstalada);
            this.Controls.Add(this.txtExecutante);
            this.Controls.Add(this.lblExecutante);
            this.Controls.Add(this.dtpDataVencimento);
            this.Controls.Add(this.lblDataVencimento);
            this.Controls.Add(this.dtpDataCalibracao);
            this.Controls.Add(this.lblDataCalibracao);
            this.Controls.Add(this.txtCertificado);
            this.Controls.Add(this.lblCertificado);
            this.Controls.Add(this.txtIdentifOficina);
            this.Controls.Add(this.lblIdentifOficina);
            this.Controls.Add(this.txtIdentifSOD);
            this.Controls.Add(this.lblIdentifSOD);
            this.Controls.Add(this.txtSN);
            this.Controls.Add(this.lblSN);
            this.Controls.Add(this.txtPN);
            this.Controls.Add(this.lblPN);
            this.Controls.Add(this.txtModelo);
            this.Controls.Add(this.lblModelo);
            this.Controls.Add(this.txtInstrumento);
            this.Controls.Add(this.lblInstrumento);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormItemNovo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Adicionar Novo Instrumento";
            this.Load += new System.EventHandler(this.FormItemNovo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFoto)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInstrumento;
        private System.Windows.Forms.TextBox txtInstrumento;
        private System.Windows.Forms.Label lblModelo;
        private System.Windows.Forms.TextBox txtModelo;
        private System.Windows.Forms.Label lblPN;
        private System.Windows.Forms.TextBox txtPN;
        private System.Windows.Forms.Label lblSN;
        private System.Windows.Forms.TextBox txtSN;
        private System.Windows.Forms.Label lblIdentifSOD;
        private System.Windows.Forms.TextBox txtIdentifSOD;
        private System.Windows.Forms.Label lblIdentifOficina;
        private System.Windows.Forms.TextBox txtIdentifOficina;
        private System.Windows.Forms.Label lblCertificado;
        private System.Windows.Forms.TextBox txtCertificado;
        private System.Windows.Forms.Label lblDataCalibracao;
        private System.Windows.Forms.DateTimePicker dtpDataCalibracao;
        private System.Windows.Forms.Label lblDataVencimento;
        private System.Windows.Forms.DateTimePicker dtpDataVencimento;
        private System.Windows.Forms.Label lblExecutante;
        private System.Windows.Forms.TextBox txtExecutante;
        private System.Windows.Forms.Label lblInstalada;
        private System.Windows.Forms.TextBox txtInstalada;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.TextBox txtLocal;
        private System.Windows.Forms.Label lblSubLocalizacao;
        private System.Windows.Forms.TextBox txtSubLocalizacao;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnSelecionarFoto;
        private System.Windows.Forms.PictureBox pictureBoxFoto;
        private System.Windows.Forms.TextBox txtCaminhoFoto;
        private System.Windows.Forms.Label lblObservacoes;
        private System.Windows.Forms.TextBox txtObservacoes;
        private System.Windows.Forms.Label lblMecanico;
        private System.Windows.Forms.TextBox txtMecanico;
        private System.Windows.Forms.Label lblCertificadoPDF;
        private System.Windows.Forms.TextBox txtCaminhoPDF;
        private System.Windows.Forms.Button btnAnexarPDF;
    }
}