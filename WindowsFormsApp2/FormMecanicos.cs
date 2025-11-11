using System;
using System.Data.SQLite;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormMecanicos : Form
    {
        public FormMecanicos()
        {
            InitializeComponent();
        }

        private void FormMecanicos_Load(object sender, EventArgs e)
        {
            CarregarMecanicos();
        }

        private void CarregarMecanicos()
        {
            dgvMecanicos.Rows.Clear();
            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sql = "SELECT MecanicoID, Nome, StatusBloqueio FROM Mecanicos";
                    using (var comando = new SQLiteCommand(sql, conexao))
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dgvMecanicos.Rows.Add(new object[] {
                                reader["MecanicoID"],
                                reader["Nome"],
                                reader["StatusBloqueio"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar mecânicos: " + ex.Message);
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            string id = txtMecanicoID.Text.Trim();
            string nome = txtNome.Text.Trim();

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(nome))
            {
                MessageBox.Show("Por favor, preencha o ID (usuário do Windows) e o Nome.");
                return;
            }

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sql = "INSERT INTO Mecanicos (MecanicoID, Nome) VALUES (@id, @nome)";
                    using (var comando = new SQLiteCommand(sql, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", id);
                        comando.Parameters.AddWithValue("@nome", nome);
                        comando.ExecuteNonQuery();
                    }
                }
                txtMecanicoID.Clear();
                txtNome.Clear();
                CarregarMecanicos(); // Atualiza a grelha
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao adicionar mecânico (verifique se o ID já existe):\n" + ex.Message);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvMecanicos.SelectedRows.Count > 0)
            {
                string idParaExcluir = dgvMecanicos.SelectedRows[0].Cells["colMecanicoID"].Value.ToString();
                var confirmacao = MessageBox.Show($"Tem certeza que deseja excluir o mecânico '{idParaExcluir}'?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (confirmacao == DialogResult.Yes)
                {
                    try
                    {
                        using (var conexao = Database.GetConnection())
                        {
                            conexao.Open();
                            string sql = "DELETE FROM Mecanicos WHERE MecanicoID = @id";
                            using (var comando = new SQLiteCommand(sql, conexao))
                            {
                                comando.Parameters.AddWithValue("@id", idParaExcluir);
                                comando.ExecuteNonQuery();
                            }
                        }
                        CarregarMecanicos(); // Atualiza a grelha
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro ao excluir mecânico: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, selecione um mecânico na grelha para excluir.");
            }
        }
    }
}