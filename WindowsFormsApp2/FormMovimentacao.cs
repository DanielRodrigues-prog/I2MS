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

        public FormMovimentacao()
        {
            InitializeComponent();
            // Pega o usuário do Windows automaticamente
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
                    string sql = "SELECT Nome FROM Mecanicos WHERE MecanicoID = @id";
                    using (var comando = new SQLiteCommand(sql, conexao))
                    {
                        comando.Parameters.AddWithValue("@id", this.usuarioWindows);
                        using (var reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Mecânico encontrado
                                this.nomeMecanico = reader["Nome"].ToString();
                                lblNomeMecanico.Text = "Nome: " + this.nomeMecanico;
                                lblStatusMecanico.Text = "Status: VÁLIDO";
                                lblStatusMecanico.ForeColor = Color.Green;
                                this.mecanicoValido = true;
                            }
                            else
                            {
                                // Mecânico não encontrado
                                lblNomeMecanico.Text = "Nome: ???";
                                lblStatusMecanico.Text = "Status: NÃO CADASTRADO";
                                lblStatusMecanico.ForeColor = Color.Red;
                                this.mecanicoValido = false;
                                MessageBox.Show($"O usuário '{this.usuarioWindows}' não está cadastrado como mecânico. Peça ao administrador.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            btnCheckOut.Enabled = this.mecanicoValido;
            btnCheckIn.Enabled = this.mecanicoValido;
        }

        private void txtInstrumentoID_TextChanged(object sender, EventArgs e)
        {
            // Verifica status em tempo real enquanto digita
            VerificarStatusFerramenta();
        }

        private void VerificarStatusFerramenta()
        {
            string idFerramenta = txtInstrumentoID.Text.Trim();
            if (string.IsNullOrEmpty(idFerramenta))
            {
                lblStatusFerramenta.Text = "Status: (Digite o ID)";
                lblStatusFerramenta.ForeColor = Color.Black;
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();
                    string mecanicoAtual = "";
                    bool encontrada = false;

                    // 1. Procura na tabela COM Calibração
                    string sql1 = "SELECT Mecanico FROM Instrumentos WHERE PN = @id OR SN = @id OR IdentifSOD = @id OR IdentifOficina = @id";
                    using (var cmd = new SQLiteCommand(sql1, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idFerramenta);
                        object res = cmd.ExecuteScalar();
                        if (res != null) { mecanicoAtual = res.ToString(); encontrada = true; }
                    }

                    // 2. Se não achou, procura na SEM Calibração
                    if (!encontrada)
                    {
                        string sql2 = "SELECT Mecanico FROM FerramentasSemCalibracao WHERE PN = @id OR Codigo = @id";
                        using (var cmd = new SQLiteCommand(sql2, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", idFerramenta);
                            object res = cmd.ExecuteScalar();
                            if (res != null) { mecanicoAtual = res.ToString(); encontrada = true; }
                        }
                    }

                    if (encontrada)
                    {
                        if (string.IsNullOrEmpty(mecanicoAtual))
                        {
                            lblStatusFerramenta.Text = "Status: DISPONÍVEL";
                            lblStatusFerramenta.ForeColor = Color.Green;
                        }
                        else
                        {
                            lblStatusFerramenta.Text = $"Status: COM {mecanicoAtual.ToUpper()}";
                            lblStatusFerramenta.ForeColor = Color.Red;
                        }
                    }
                    else
                    {
                        lblStatusFerramenta.Text = "Status: NÃO ENCONTRADA";
                        lblStatusFerramenta.ForeColor = Color.Gray;
                    }
                }
            }
            catch { lblStatusFerramenta.Text = "Erro"; }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            RealizarMovimentacao("PEGAR");
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            RealizarMovimentacao("DEVOLVER");
        }

        private void RealizarMovimentacao(string tipo)
        {
            if (!this.mecanicoValido) return;

            string idFerramenta = txtInstrumentoID.Text.Trim();
            if (string.IsNullOrEmpty(idFerramenta))
            {
                MessageBox.Show("Digite o ID da ferramenta.");
                return;
            }

            string tabela = "";
            long idDb = -1;
            string mecanicoAtual = "";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    conn.Open();

                    // 1. Localizar a ferramenta
                    string sql1 = "SELECT ID, Mecanico FROM Instrumentos WHERE PN=@id OR SN=@id OR IdentifSOD=@id OR IdentifOficina=@id";
                    using (var cmd = new SQLiteCommand(sql1, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", idFerramenta);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (r.Read()) { idDb = Convert.ToInt64(r["ID"]); mecanicoAtual = r["Mecanico"].ToString(); tabela = "Instrumentos"; }
                        }
                    }

                    if (tabela == "")
                    {
                        string sql2 = "SELECT ID, Mecanico FROM FerramentasSemCalibracao WHERE PN=@id OR Codigo=@id";
                        using (var cmd = new SQLiteCommand(sql2, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", idFerramenta);
                            using (var r = cmd.ExecuteReader())
                            {
                                if (r.Read()) { idDb = Convert.ToInt64(r["ID"]); mecanicoAtual = r["Mecanico"].ToString(); tabela = "FerramentasSemCalibracao"; }
                            }
                        }
                    }

                    if (tabela == "")
                    {
                        MessageBox.Show("Ferramenta não encontrada.");
                        return;
                    }

                    // 2. Validações de Lógica
                    if (tipo == "PEGAR")
                    {
                        if (!string.IsNullOrEmpty(mecanicoAtual))
                        {
                            MessageBox.Show($"Ferramenta já está com {mecanicoAtual}.");
                            return;
                        }
                    }
                    else // DEVOLVER
                    {
                        if (string.IsNullOrEmpty(mecanicoAtual))
                        {
                            MessageBox.Show("Ferramenta já está livre.");
                            return;
                        }
                        // Opcional: Verificar se quem está devolvendo é o mesmo que pegou. 
                        // Se quiser permitir que qualquer um devolva, remova este 'if'.
                        if (mecanicoAtual != this.nomeMecanico)
                        {
                            var resp = MessageBox.Show($"Esta ferramenta está registada com {mecanicoAtual}. Deseja devolver mesmo assim?", "Confirmação", MessageBoxButtons.YesNo);
                            if (resp == DialogResult.No) return;
                        }
                    }

                    // 3. Executar a Ação
                    using (var trans = conn.BeginTransaction())
                    {
                        string novoMecanico = (tipo == "PEGAR") ? this.nomeMecanico : null;
                        string origem = (tabela == "Instrumentos") ? "COM" : "SEM";
                        string dataAgora = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

                        // Atualiza a tabela da ferramenta
                        string sqlUp = $"UPDATE {tabela} SET Mecanico = @mec WHERE ID = @id";
                        using (var cmd = new SQLiteCommand(sqlUp, conn))
                        {
                            cmd.Parameters.AddWithValue("@mec", novoMecanico);
                            cmd.Parameters.AddWithValue("@id", idDb);
                            cmd.ExecuteNonQuery();
                        }

                        // Regista no Histórico
                        if (tipo == "PEGAR")
                        {
                            string sqlIns = "INSERT INTO Emprestimos (InstrumentoID, TabelaOrigem, MecanicoID, DataSaida) VALUES (@idInst, @origem, @idMec, @data)";
                            using (var cmd = new SQLiteCommand(sqlIns, conn))
                            {
                                cmd.Parameters.AddWithValue("@idInst", idDb);
                                cmd.Parameters.AddWithValue("@origem", origem);
                                cmd.Parameters.AddWithValue("@idMec", this.usuarioWindows);
                                cmd.Parameters.AddWithValue("@data", dataAgora);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else // DEVOLVER
                        {
                            string sqlUpHist = "UPDATE Emprestimos SET DataDevolucao = @data WHERE InstrumentoID = @idInst AND TabelaOrigem = @origem AND DataDevolucao IS NULL";
                            using (var cmd = new SQLiteCommand(sqlUpHist, conn))
                            {
                                cmd.Parameters.AddWithValue("@data", dataAgora);
                                cmd.Parameters.AddWithValue("@idInst", idDb);
                                cmd.Parameters.AddWithValue("@origem", origem);
                                cmd.ExecuteNonQuery();
                            }
                        }

                        trans.Commit();
                    }

                    MessageBox.Show($"Ação '{tipo}' realizada com sucesso!");
                    txtInstrumentoID.Clear();
                    txtInstrumentoID.Focus();
                    VerificarStatusFerramenta();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}