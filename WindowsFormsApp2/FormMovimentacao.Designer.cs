namespace WindowsFormsApp2
{
    partial class FormMovimentacao
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtAeronave = new System.Windows.Forms.TextBox();
            this.cmbMecanicos = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblStatusFerramenta = new System.Windows.Forms.Label();
            this.txtInstrumentoID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCheckOut = new System.Windows.Forms.Button();
            this.btnCheckIn = new System.Windows.Forms.Button();
            this.btnFechar = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtAeronave);
            this.groupBox1.Controls.Add(this.cmbMecanicos);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Dados da Saída";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(244, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Aeronave/Prefixo:";
            // 
            // txtAeronave
            // 
            this.txtAeronave.Location = new System.Drawing.Point(247, 53);
            this.txtAeronave.Name = "txtAeronave";
            this.txtAeronave.Size = new System.Drawing.Size(194, 23);
            this.txtAeronave.TabIndex = 2;
            // 
            // cmbMecanicos
            // 
            this.cmbMecanicos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMecanicos.FormattingEnabled = true;
            this.cmbMecanicos.Location = new System.Drawing.Point(19, 53);
            this.cmbMecanicos.Name = "cmbMecanicos";
            this.cmbMecanicos.Size = new System.Drawing.Size(208, 23);
            this.cmbMecanicos.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Selecione Mecânico:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatusFerramenta);
            this.groupBox2.Controls.Add(this.txtInstrumentoID);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.groupBox2.Location = new System.Drawing.Point(12, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(460, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "2. Ferramenta";
            // 
            // lblStatusFerramenta
            // 
            this.lblStatusFerramenta.AutoSize = true;
            this.lblStatusFerramenta.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblStatusFerramenta.Location = new System.Drawing.Point(230, 56);
            this.lblStatusFerramenta.Name = "lblStatusFerramenta";
            this.lblStatusFerramenta.Size = new System.Drawing.Size(155, 19);
            this.lblStatusFerramenta.TabIndex = 4;
            this.lblStatusFerramenta.Text = "Status: AGUARDANDO";
            // 
            // txtInstrumentoID
            // 
            this.txtInstrumentoID.Location = new System.Drawing.Point(19, 53);
            this.txtInstrumentoID.Name = "txtInstrumentoID";
            this.txtInstrumentoID.Size = new System.Drawing.Size(188, 23);
            this.txtInstrumentoID.TabIndex = 1;
            this.txtInstrumentoID.TextChanged += new System.EventHandler(this.txtInstrumentoID_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Código Ferramenta";
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.Color.SeaGreen;
            this.btnCheckOut.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheckOut.ForeColor = System.Drawing.Color.White;
            this.btnCheckOut.Location = new System.Drawing.Point(12, 235);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(225, 60);
            this.btnCheckOut.TabIndex = 2;
            this.btnCheckOut.Text = "LIBERAR (Saída)";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // btnCheckIn
            // 
            this.btnCheckIn.BackColor = System.Drawing.Color.DarkOrange;
            this.btnCheckIn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnCheckIn.Location = new System.Drawing.Point(247, 235);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(225, 60);
            this.btnCheckIn.TabIndex = 3;
            this.btnCheckIn.Text = "RECEBER (Devolução)";
            this.btnCheckIn.UseVisualStyleBackColor = false;
            this.btnCheckIn.Click += new System.EventHandler(this.btnCheckIn_Click);
            // 
            // btnFechar
            // 
            this.btnFechar.Location = new System.Drawing.Point(397, 310);
            this.btnFechar.Name = "btnFechar";
            this.btnFechar.Size = new System.Drawing.Size(75, 23);
            this.btnFechar.TabIndex = 4;
            this.btnFechar.Text = "Fechar";
            this.btnFechar.UseVisualStyleBackColor = true;
            this.btnFechar.Click += new System.EventHandler(this.btnFechar_Click);
            // 
            // FormMovimentacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 345);
            this.Controls.Add(this.btnFechar);
            this.Controls.Add(this.btnCheckIn);
            this.Controls.Add(this.btnCheckOut);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormMovimentacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Movimentação de Ferramentas";
            this.Load += new System.EventHandler(this.FormMovimentacao_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbMecanicos;
        private System.Windows.Forms.TextBox txtAeronave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblStatusFerramenta;
        private System.Windows.Forms.TextBox txtInstrumentoID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Button btnCheckIn;
        private System.Windows.Forms.Button btnFechar;
    }
}