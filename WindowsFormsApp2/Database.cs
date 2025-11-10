using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite; // O "tradutor" que acabámos de instalar
using System.Globalization;

namespace WindowsFormsApp2
{
    public class Database
    {
        // Define o caminho para o nosso novo ficheiro de banco de dados
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calibracao.db");

        // Função para criar uma nova ligação
        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        // Esta é a função principal que cria o banco de dados e as tabelas
        public static void InitializeDatabase()
        {
            // Só faz algo se o ficheiro do banco de dados AINDA NÃO EXISTIR
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath); // Cria o ficheiro vazio

                using (var conexao = GetConnection())
                {
                    conexao.Open();

                    // 1. Criar a Tabela de Instrumentos (baseada no seu CSV)
                    string sqlInstrumentos = @"
                        CREATE TABLE Instrumentos (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Instrumento TEXT, Modelo TEXT, PN TEXT, SN TEXT,
                            IdentifSOD TEXT, IdentifOficina TEXT, Certificado TEXT,
                            DataCalibracao TEXT, DataVencimento TEXT, Situacao TEXT,
                            Executante TEXT, Instalada TEXT, Local TEXT, SubLocalizacao TEXT,
                            Foto TEXT, Observacoes TEXT, Mecanico TEXT, CertificadoPDF TEXT
                        )";
                    using (var comando = new SQLiteCommand(sqlInstrumentos, conexao))
                    {
                        comando.ExecuteNonQuery();
                    }

                    // 2. Criar a Tabela de Mecânicos (para o seu sistema de check-in)
                    string sqlMecanicos = @"
                        CREATE TABLE Mecanicos (
                            MecanicoID TEXT PRIMARY KEY,
                            Nome TEXT,
                            StatusBloqueio TEXT DEFAULT 'Livre'
                        )";
                    using (var comando = new SQLiteCommand(sqlMecanicos, conexao))
                    {
                        comando.ExecuteNonQuery();
                    }

                    // 3. Criar a Tabela de Empréstimos (check-in/check-out)
                    string sqlEmprestimos = @"
                        CREATE TABLE Emprestimos (
                            EmprestimoID INTEGER PRIMARY KEY AUTOINCREMENT,
                            InstrumentoID INTEGER,
                            MecanicoID TEXT,
                            DataSaida TEXT,
                            DataDevolucao TEXT,
                            FOREIGN KEY(InstrumentoID) REFERENCES Instrumentos(ID),
                            FOREIGN KEY(MecanicoID) REFERENCES Mecanicos(MecanicoID)
                        )";
                    using (var comando = new SQLiteCommand(sqlEmprestimos, conexao))
                    {
                        comando.ExecuteNonQuery();
                    }
                }

                // 4. Agora, vamos migrar os dados do CSV antigo
                MigrateDataFromCsv();
            }
        }

        // Função que copia os dados do CSV para o novo banco de dados
        private static void MigrateDataFromCsv()
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dados.csv");
            if (!File.Exists(csvPath))
            {
                // Se não houver CSV, não há nada a migrar.
                return;
            }

            try
            {
                string[] linhas = File.ReadAllLines(csvPath);
                using (var conexao = GetConnection())
                {
                    conexao.Open();

                    // Usamos uma "Transação" para tornar a cópia milhares de vezes mais rápida
                    using (var transacao = conexao.BeginTransaction())
                    {
                        // Começa em 1 para pular o cabeçalho
                        for (int i = 1; i < linhas.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(linhas[i])) continue;

                            string[] colunas = linhas[i].Split(';');

                            // Precisamos ter certeza de que temos 18 colunas, como no seu CSV
                            if (colunas.Length < 18) continue;

                            string sql = @"INSERT INTO Instrumentos (
                                            Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, 
                                            Certificado, DataCalibracao, DataVencimento, Situacao, 
                                            Executante, Instalada, Local, SubLocalizacao, 
                                            Foto, Observacoes, Mecanico, CertificadoPDF
                                        ) VALUES (
                                            @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, 
                                            @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17
                                        )";

                            using (var comando = new SQLiteCommand(sql, conexao))
                            {
                                // Adiciona todos os 18 valores do CSV
                                for (int j = 0; j < 18; j++)
                                {
                                    comando.Parameters.AddWithValue($"@p{j}", colunas[j]);
                                }
                                comando.ExecuteNonQuery();
                            }
                        }
                        transacao.Commit(); // Salva todas as alterações no banco de dados
                    }
                }

                // Renomeia o CSV para evitar que seja importado de novo
                File.Move(csvPath, csvPath + ".MIGRADO_OK");
                MessageBox.Show("SUCESSO! Os seus dados do arquivo 'dados.csv' foram migrados com sucesso para o novo banco de dados.", "Migração Concluída", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro CRÍTICO durante a migração do CSV.\n\n" + ex.Message, "Erro de Migração", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}