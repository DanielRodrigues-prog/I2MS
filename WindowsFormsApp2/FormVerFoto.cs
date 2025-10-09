using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormVerFoto : Form
    {
        public FormVerFoto(string caminhoImagem)
        {
            InitializeComponent();
            try
            {
                pictureBox1.Image = Image.FromFile(caminhoImagem);
                this.Text = System.IO.Path.GetFileName(caminhoImagem);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Não foi possível carregar a imagem: " + ex.Message);
                pictureBox1.Image = pictureBox1.ErrorImage;
            }
        }
    }
}