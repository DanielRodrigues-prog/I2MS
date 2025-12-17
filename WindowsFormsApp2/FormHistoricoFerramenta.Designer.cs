namespace WindowsFormsApp2
{
    partial class FormHistoricoFerramenta
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FormHistoricoFerramenta
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Name = "FormHistoricoFerramenta";
            this.Text = "Histórico e Estatísticas";
            this.Load += new System.EventHandler(this.FormHistoricoFerramenta_Load);
            this.ResumeLayout(false);

        }
    }
}