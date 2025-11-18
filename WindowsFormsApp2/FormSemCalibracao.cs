using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormSemCalibracao : Form
    {
        public FormSemCalibracao()
        {
            InitializeComponent();
        }

        private void FormSemCalibracao_Load(object sender, EventArgs e)
        {
            dgvDados.Rows.Clear();
            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sql = "SELECT * FROM FerramentasSemCalibracao";
                    using (var cmd = new SQLiteCommand(sql, conexao))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvDados.Rows.Add(new object[] {
                                reader["Descricao"], reader["Codigo"], reader["PN"],
                                reader["Fabricante"], reader["Local"], reader["CadastroLocal"],
                                reader["CodLocal"], reader["Status"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }
    }
}