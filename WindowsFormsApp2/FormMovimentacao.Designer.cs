namespace WindowsFormsApp2
{
    partial class FormMovimentacao
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatusMecanico = new System.Windows.Forms.Label();
            this.lblNomeMecanico = new System.Windows.Forms.Label();
            this.txtMecanicoID = new System.Windows.Forms.TextBox();
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
            this.groupBox1.Controls.Add(this.lblStatusMecanico);
            this.groupBox1.Controls.Add(this.lblNomeMecanico);
            this.groupBox1.Controls.Add(this.txtMecanicoID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(460, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "1. Mecânico (Automático)";
            // 
            // lblStatusMecanico
            // 
            this.lblStatusMecanico.AutoSize = true;
            this.lblStatusMecanico.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusMecanico.ForeColor = System.Drawing.Color.Green;
            this.lblStatusMecanico.Location = new System.Drawing.Point(230, 61);
            this.lblStatusMecanico.Name = "lblStatusMecanico";
            this.lblStatusMecanico.Size = new System.Drawing.Size(49, 17);
            this.lblStatusMecanico.TabIndex = 3;
            this.lblStatusMecanico.Text = "Status:";
            // 
            // lblNomeMecanico
            // 
            this.lblNomeMecanico.AutoSize = true;
            this.lblNomeMecanico.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNomeMecanico.Location = new System.Drawing.Point(230, 35);
            this.lblNomeMecanico.Name = "lblNomeMecanico";
            this.lblNomeMecanico.Size = new System.Drawing.Size(47, 17);
            this.lblNomeMecanico.TabIndex = 2;
            this.lblNomeMecanico.Text = "Nome:";
            // 
            // txtMecanicoID
            // 
            this.txtMecanicoID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMecanicoID.Location = new System.Drawing.Point(19, 53);
            this.txtMecanicoID.Name = "txtMecanicoID";
            this.txtMecanicoID.ReadOnly = true;
            this.txtMecanicoID.Size = new System.Drawing.Size(188, 25);
            this.txtMecanicoID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Usuário do Windows:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblStatusFerramenta);
            this.groupBox2.Controls.Add(this.txtInstrumentoID);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.lblStatusFerramenta.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusFerramenta.Location = new System.Drawing.Point(230, 56);
            this.lblStatusFerramenta.Name = "lblStatusFerramenta";
            this.lblStatusFerramenta.Size = new System.Drawing.Size(130, 17);
            this.lblStatusFerramenta.TabIndex = 4;
            this.lblStatusFerramenta.Text = "Status: DISPONÍVEL";
            // 
            // txtInstrumentoID
            // 
            this.txtInstrumentoID.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtInstrumentoID.Location = new System.Drawing.Point(19, 53);
            this.txtInstrumentoID.Name = "txtInstrumentoID";
            this.txtInstrumentoID.Size = new System.Drawing.Size(188, 25);
            this.txtInstrumentoID.TabIndex = 1;
            this.txtInstrumentoID.TextChanged += new System.EventHandler(this.txtInstrumentoID_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(191, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "ID do Instrumento (P/N, S/N ou ID):";
            // 
            // btnCheckOut
            // 
            this.btnCheckOut.BackColor = System.Drawing.Color.SeaGreen;
            this.btnCheckOut.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckOut.ForeColor = System.Drawing.Color.White;
            this.btnCheckOut.Location = new System.Drawing.Point(12, 235);
            this.btnCheckOut.Name = "btnCheckOut";
            this.btnCheckOut.Size = new System.Drawing.Size(225, 60);
            this.btnCheckOut.TabIndex = 2;
            this.btnCheckOut.Text = "PEGAR (Check-Out)";
            this.btnCheckOut.UseVisualStyleBackColor = false;
            this.btnCheckOut.Click += new System.EventHandler(this.btnCheckOut_Click);
            // 
            // btnCheckIn
            // 
            this.btnCheckIn.BackColor = System.Drawing.Color.DarkOrange;
            this.btnCheckIn.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCheckIn.ForeColor = System.Drawing.Color.White;
            this.btnCheckIn.Location = new System.Drawing.Point(247, 235);
            this.btnCheckIn.Name = "btnCheckIn";
            this.btnCheckIn.Size = new System.Drawing.Size(225, 60);
            this.btnCheckIn.TabIndex = 3;
            this.btnCheckIn.Text = "DEVOLVER (Check-In)";
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
            this.Text = "Movimentação de Ferramentas (Check-in/Check-out)";
            this.Load += new System.EventHandler(this.FormMovimentacao_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatusMecanico;
        private System.Windows.Forms.Label lblNomeMecanico;
        private System.Windows.Forms.TextBox txtMecanicoID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblStatusFerramenta;
        private System.Windows.Forms.TextBox txtInstrumentoID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCheckOut;
        private System.Windows.Forms.Button btnCheckIn;
        private System.Windows.Forms.Button btnFechar;
    }
}