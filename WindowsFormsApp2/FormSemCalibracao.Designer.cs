namespace WindowsFormsApp2
{
    partial class FormSemCalibracao
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvDados = new System.Windows.Forms.DataGridView();
            this.colDescricao = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFabricante = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCadastroLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCodLocal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).BeginInit();
            this.SuspendLayout();

            // dgvDados
            this.dgvDados.AllowUserToAddRows = false;
            this.dgvDados.AllowUserToDeleteRows = false;
            this.dgvDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDados.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDescricao, this.colCodigo, this.colPN, this.colFabricante,
            this.colLocal, this.colCadastroLocal, this.colCodLocal, this.colStatus});
            this.dgvDados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDados.Location = new System.Drawing.Point(0, 0);
            this.dgvDados.Name = "dgvDados";
            this.dgvDados.ReadOnly = true;
            this.dgvDados.RowHeadersVisible = false;
            this.dgvDados.Size = new System.Drawing.Size(800, 450);
            this.dgvDados.TabIndex = 0;
            this.dgvDados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;

            // Colunas
            this.colDescricao.HeaderText = "DESCRIÇÃO"; this.colDescricao.Name = "colDescricao";
            this.colCodigo.HeaderText = "CÓDIGO"; this.colCodigo.Name = "colCodigo";
            this.colPN.HeaderText = "P/N"; this.colPN.Name = "colPN";
            this.colFabricante.HeaderText = "FABRICANTE"; this.colFabricante.Name = "colFabricante";
            this.colLocal.HeaderText = "LOCAL"; this.colLocal.Name = "colLocal";
            this.colCadastroLocal.HeaderText = "CADASTRO LOCAL"; this.colCadastroLocal.Name = "colCadastroLocal";
            this.colCodLocal.HeaderText = "COD LOCAL"; this.colCodLocal.Name = "colCodLocal";
            this.colStatus.HeaderText = "STATUS"; this.colStatus.Name = "colStatus";

            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvDados);
            this.Name = "FormSemCalibracao";
            this.Text = "Ferramentas Sem Calibração";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormSemCalibracao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDados)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.DataGridView dgvDados;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescricao;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPN;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFabricante;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCadastroLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodLocal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}