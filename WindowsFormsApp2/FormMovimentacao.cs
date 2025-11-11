using System;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class FormMovimentacao : Form
    {
        private string usuarioWindows;
        private string nomeMecanico;
        private bool mecanicoValido = false;
        private bool mecanicoBloqueado = false;

        public FormMovimentacao()
        {
            InitializeComponent();
            // Pega o usuário do Windows assim que o formulário é criado
            this.usuarioWindows = Environment.UserName.ToLower();
            txtMecanicoID.Text = this.usuarioWindows;
        }

        private void FormMovimentacao_Load(object sender, EventArgs e)
        {
            VerificarMecanico();
        }

        private void VerificarMecanico()
        {
            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sql = "SELECT Nome, StatusBloqueio FROM Mecanicos WHERE MecanicoID = @id";
                    using (var comando = new SQLiteCommand(sql, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", this.usuarioWindows);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Mecânico encontrado!
                                this.nomeMecanico = reader["Nome"].ToString();
                                string status = reader["StatusBloqueio"].ToString();

                                lblNomeMecanico.Text = "Nome: " + this.nomeMecanico;

                                if (status == "Bloqueado")
                                {
                                    lblStatusMecanico.Text = "Status: BLOQUEADO";
                                    lblStatusMecanico.ForeColor = Color.Red;
                                    this.mecanicoBloqueado = true;
                                }
                                else
                                {
                                    lblStatusMecanico.Text = "Status: LIVRE";
                                    lblStatusMecanico.ForeColor = Color.Green;
                                    this.mecanicoBloqueado = false;
                                }
                                this.mecanicoValido = true;
                            }
                            else
                            {
                                // Mecânico não encontrado
                                lblNomeMecanico.Text = "Nome: ???";
                                lblStatusMecanico.Text = "Status: NÃO CADASTRADO";
                                lblStatusMecanico.ForeColor = Color.Red;
                                this.mecanicoValido = false;
                                MessageBox.Show($"O seu usuário do Windows ({this.usuarioWindows}) não está registado como mecânico. Por favor, contacte o administrador (rodridae).", "Erro de Acesso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar mecânico: " + ex.Message);
                this.mecanicoValido = false;
            }

            // Habilita ou desabilita os botões com base na validade do mecânico
            btnCheckOut.Enabled = this.mecanicoValido;
            btnCheckIn.Enabled = this.mecanicoValido;
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (!this.mecanicoValido)
            {
                MessageBox.Show("Usuário do Windows não cadastrado. Operação cancelada.");
                return;
            }

            if (this.mecanicoBloqueado)
            {
                MessageBox.Show("Você está bloqueado. Por favor, devolva a ferramenta anterior antes de pegar uma nova.", "Mecânico Bloqueado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string idFerramenta = txtInstrumentoID.Text.Trim();
            if (string.IsNullOrEmpty(idFerramenta))
            {
                MessageBox.Show("Por favor, digite ou leia o ID da ferramenta.");
                return;
            }

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    // 1. Encontrar a ferramenta e verificar se está disponível
                    string sqlVerifica = "SELECT ID, Mecanico FROM Instrumentos WHERE PN = @id OR SN = @id OR IdentifSOD = @id OR IdentifOficina = @id";
                    long dbInstrumentoID = -1;
                    string mecanicoAtual = "";

                    using (var comando = new SQLiteCommand(sqlVerifica, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", idFerramenta);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dbInstrumentoID = Convert.ToInt64(reader["ID"]);
                                mecanicoAtual = reader["Mecanico"]?.ToString() ?? "";
                            }
                        }
                    }

                    if (dbInstrumentoID == -1)
                    {
                        MessageBox.Show("Ferramenta não encontrada no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!string.IsNullOrEmpty(mecanicoAtual))
                    {
                        MessageBox.Show($"Esta ferramenta já está em posse de: {mecanicoAtual}", "Ferramenta Indisponível", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 2. Se tudo estiver OK, fazer o check-out
                    using (var transacao = conexao.BeginTransaction())
                    {
                        string dataAgora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                        // 2a. Atualiza a tabela Instrumentos
                        string sqlUpdate = "UPDATE Instrumentos SET Mecanico = @nomeMecanico WHERE ID = @id";
                        using (var cmdUpdate = new SQLiteCommand(sqlUpdate, conexao))
                        {
                            cmdUpdate.Parameters.AddWithValue("@nomeMecanico", this.nomeMecanico);
                            cmdUpdate.Parameters.AddWithValue("@id", dbInstrumentoID);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 2b. Regista o empréstimo
                        string sqlInsert = "INSERT INTO Emprestimos (InstrumentoID, MecanicoID, DataSaida) VALUES (@instID, @mecID, @data)";
                        using (var cmdInsert = new SQLiteCommand(sqlInsert, conexao))
                        {
                            cmdInsert.Parameters.AddWithValue("@instID", dbInstrumentoID);
                            cmdInsert.Parameters.AddWithValue("@mecID", this.usuarioWindows);
                            cmdInsert.Parameters.AddWithValue("@data", dataAgora);
                            cmdInsert.ExecuteNonQuery();
                        }

                        // 2c. Bloqueia o mecânico (pois ele agora tem uma ferramenta)
                        string sqlBlock = "UPDATE Mecanicos SET StatusBloqueio = 'Bloqueado' WHERE MecanicoID = @id";
                        using (var cmdBlock = new SQLiteCommand(sqlBlock, conexao))
                        {
                            cmdBlock.Parameters.AddWithValue("@id", this.usuarioWindows);
                            cmdBlock.ExecuteNonQuery();
                        }

                        transacao.Commit();
                    }

                    MessageBox.Show("Check-out realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mecanicoBloqueado = true; // Atualiza o status local
                    lblStatusMecanico.Text = "Status: BLOQUEADO";
                    lblStatusMecanico.ForeColor = Color.Red;
                    txtInstrumentoID.Clear();
                    txtInstrumentoID.Focus();
                    VerificarStatusFerramenta(); // Atualiza o status da ferramenta
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar check-out: " + ex.Message);
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            if (!this.mecanicoValido)
            {
                MessageBox.Show("Usuário do Windows não cadastrado. Operação cancelada.");
                return;
            }

            string idFerramenta = txtInstrumentoID.Text.Trim();
            if (string.IsNullOrEmpty(idFerramenta))
            {
                MessageBox.Show("Por favor, digite ou leia o ID da ferramenta.");
                return;
            }

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    // 1. Encontrar a ferramenta e verificar se está com este mecânico
                    string sqlVerifica = "SELECT ID, Mecanico FROM Instrumentos WHERE PN = @id OR SN = @id OR IdentifSOD = @id OR IdentifOficina = @id";
                    long dbInstrumentoID = -1;
                    string mecanicoAtual = "";

                    using (var comando = new SQLiteCommand(sqlVerifica, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", idFerramenta);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                dbInstrumentoID = Convert.ToInt64(reader["ID"]);
                                mecanicoAtual = reader["Mecanico"]?.ToString() ?? "";
                            }
                        }
                    }

                    if (dbInstrumentoID == -1)
                    {
                        MessageBox.Show("Ferramenta não encontrada no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Verifica se a ferramenta está com o mecânico que está a tentar devolver
                    if (string.IsNullOrEmpty(mecanicoAtual) || mecanicoAtual != this.nomeMecanico)
                    {
                        MessageBox.Show($"Esta ferramenta não está em sua posse. Ela consta como: {(string.IsNullOrEmpty(mecanicoAtual) ? "Disponível" : $"Em posse de {mecanicoAtual}")}", "Erro de Check-in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 2. Se tudo estiver OK, fazer o check-in
                    using (var transacao = conexao.BeginTransaction())
                    {
                        string dataAgora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                        // 2a. Atualiza a tabela Instrumentos (limpa o nome do mecânico)
                        string sqlUpdate = "UPDATE Instrumentos SET Mecanico = NULL WHERE ID = @id";
                        using (var cmdUpdate = new SQLiteCommand(sqlUpdate, conexao))
                        {
                            cmdUpdate.Parameters.AddWithValue("@id", dbInstrumentoID);
                            cmdUpdate.ExecuteNonQuery();
                        }

                        // 2b. Fecha o registo de empréstimo
                        string sqlUpdateEmp = "UPDATE Emprestimos SET DataDevolucao = @data WHERE InstrumentoID = @instID AND MecanicoID = @mecID AND DataDevolucao IS NULL";
                        using (var cmdInsert = new SQLiteCommand(sqlUpdateEmp, conexao))
                        {
                            cmdInsert.Parameters.AddWithValue("@instID", dbInstrumentoID);
                            cmdInsert.Parameters.AddWithValue("@mecID", this.usuarioWindows);
                            cmdInsert.Parameters.AddWithValue("@data", dataAgora);
                            cmdInsert.ExecuteNonQuery();
                        }

                        // 2c. Desbloqueia o mecânico
                        string sqlBlock = "UPDATE Mecanicos SET StatusBloqueio = 'Livre' WHERE MecanicoID = @id";
                        using (var cmdBlock = new SQLiteCommand(sqlBlock, conexao))
                        {
                            cmdBlock.Parameters.AddWithValue("@id", this.usuarioWindows);
                            cmdBlock.ExecuteNonQuery();
                        }

                        transacao.Commit();
                    }

                    MessageBox.Show("Check-in (Devolução) realizado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.mecanicoBloqueado = false; // Atualiza o status local
                    lblStatusMecanico.Text = "Status: LIVRE";
                    lblStatusMecanico.ForeColor = Color.Green;
                    txtInstrumentoID.Clear();
                    txtInstrumentoID.Focus();
                    VerificarStatusFerramenta(); // Atualiza o status da ferramenta
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao realizar check-in: " + ex.Message);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtInstrumentoID_TextChanged(object sender, EventArgs e)
        {
            // Quando o texto muda, verifica o status da ferramenta
            VerificarStatusFerramenta();
        }

        private void VerificarStatusFerramenta()
        {
            string idFerramenta = txtInstrumentoID.Text.Trim();
            if (string.IsNullOrEmpty(idFerramenta))
            {
                lblStatusFerramenta.Text = "Status: (Digite o ID)";
                return;
            }

            try
            {
                using (var conexao = Database.GetConnection())
                {
                    conexao.Open();
                    string sqlVerifica = "SELECT Mecanico FROM Instrumentos WHERE PN = @id OR SN = @id OR IdentifSOD = @id OR IdentifOficina = @id";
                    using (var comando = new SQLiteCommand(sqlVerifica, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", idFerramenta);
                        object resultado = comando.ExecuteScalar(); // Pega só o primeiro valor

                        if (resultado != null && resultado != DBNull.Value && !string.IsNullOrEmpty(resultado.ToString()))
                        {
                            lblStatusFerramenta.Text = $"Status: EM POSSE DE {resultado.ToString().ToUpper()}";
                            lblStatusFerramenta.ForeColor = Color.Red;
                        }
                        else if (resultado != null)
                        {
                            lblStatusFerramenta.Text = "Status: DISPONÍVEL";
                            lblStatusFerramenta.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatusFerramenta.Text = "Status: NÃO ENCONTRADA";
                            lblStatusFerramenta.ForeColor = Color.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatusFerramenta.Text = "Erro ao consultar";
            }
        }
    }
}