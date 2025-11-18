using System;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Collections.Generic;

namespace WindowsFormsApp2
{
    public class Database
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "calibracao.db");

        public static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source={dbPath};Version=3;");
        }

        public static void InitializeDatabase()
        {
            // Se o arquivo do banco não existe, vamos criá-lo e popular com os dados dos CSVs
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);

                using (var conexao = GetConnection())
                {
                    conexao.Open();

                    // 1. Tabela Instrumentos (Com Calibração) - 18 Colunas de dados
                    string sqlInstrumentos = @"
                        CREATE TABLE Instrumentos (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Instrumento TEXT, Modelo TEXT, PN TEXT, SN TEXT,
                            IdentifSOD TEXT, IdentifOficina TEXT, Certificado TEXT,
                            DataCalibracao TEXT, DataVencimento TEXT, Situacao TEXT,
                            Executante TEXT, Instalada TEXT, Local TEXT, SubLocalizacao TEXT,
                            Foto TEXT, Observacoes TEXT, Mecanico TEXT, CertificadoPDF TEXT
                        )";
                    using (var cmd = new SQLiteCommand(sqlInstrumentos, conexao)) { cmd.ExecuteNonQuery(); }

                    // 2. Tabela FerramentasSemCalibracao (Sem Calibração) - 11 Colunas de dados
                    string sqlSemCalibracao = @"
                        CREATE TABLE FerramentasSemCalibracao (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Descricao TEXT, Codigo TEXT, PN TEXT, Fabricante TEXT,
                            Local TEXT, CadastroLocal TEXT, CodLocal TEXT, Status TEXT,
                            Mecanico TEXT, Foto TEXT, CertificadoPDF TEXT
                        )";
                    using (var cmd = new SQLiteCommand(sqlSemCalibracao, conexao)) { cmd.ExecuteNonQuery(); }

                    // 3. Tabelas Auxiliares (Mecânicos e Empréstimos)
                    string sqlMecanicos = "CREATE TABLE Mecanicos (MecanicoID TEXT PRIMARY KEY, Nome TEXT, StatusBloqueio TEXT DEFAULT 'Livre')";
                    using (var cmd = new SQLiteCommand(sqlMecanicos, conexao)) { cmd.ExecuteNonQuery(); }

                    string sqlEmprestimos = "CREATE TABLE Emprestimos (EmprestimoID INTEGER PRIMARY KEY AUTOINCREMENT, InstrumentoID INTEGER, TabelaOrigem TEXT, MecanicoID TEXT, DataSaida TEXT, DataDevolucao TEXT)";
                    using (var cmd = new SQLiteCommand(sqlEmprestimos, conexao)) { cmd.ExecuteNonQuery(); }
                }

                // Executa a migração dos dados antigos (CSVs) para o novo banco
                MigrateDadosNormais();
                MigrateDadosSemCalibracao();
            }
        }

        // Migra do 'dados.csv' (15 colunas originais -> 18 colunas no banco)
        private static void MigrateDadosNormais()
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dados.csv");
            if (!File.Exists(csvPath)) return;

            try
            {
                string[] linhas = File.ReadAllLines(csvPath);
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        // Começa em 1 para pular o cabeçalho
                        for (int i = 1; i < linhas.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(linhas[i])) continue;

                            string[] colunas = linhas[i].Split(';');
                            List<string> dados = new List<string>(colunas);

                            // O banco espera 18 colunas, mas o CSV antigo tem 15.
                            // Adicionamos strings vazias para completar (Observacoes, Mecanico, PDF)
                            while (dados.Count < 18) dados.Add("");

                            string sql = @"INSERT INTO Instrumentos (
                                            Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, 
                                            Certificado, DataCalibracao, DataVencimento, Situacao, 
                                            Executante, Instalada, Local, SubLocalizacao, 
                                            Foto, Observacoes, Mecanico, CertificadoPDF
                                        ) VALUES (
                                            @p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, 
                                            @p10, @p11, @p12, @p13, @p14, @p15, @p16, @p17
                                        )";

                            using (var cmd = new SQLiteCommand(sql, conexao))
                            {
                                for (int j = 0; j < 18; j++)
                                    cmd.Parameters.AddWithValue($"@p{j}", dados[j]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transacao.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao importar dados.csv: " + ex.Message);
            }
        }

        // Migra do 'dados_sem_calibracao.csv' (8 colunas originais -> 11 colunas no banco)
        private static void MigrateDadosSemCalibracao()
        {
            string csvPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "dados_sem_calibracao.csv");
            if (!File.Exists(csvPath)) return;

            try
            {
                string[] linhas = File.ReadAllLines(csvPath);
                using (var conexao = GetConnection())
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        for (int i = 1; i < linhas.Length; i++)
                        {
                            if (string.IsNullOrWhiteSpace(linhas[i])) continue;

                            string[] colunas = linhas[i].Split(';');
                            List<string> dados = new List<string>(colunas);

                            // O banco espera 11 colunas, mas o CSV tem 8.
                            // Adicionamos vazios para Mecanico, Foto e CertificadoPDF
                            while (dados.Count < 11) dados.Add("");

                            string sql = @"INSERT INTO FerramentasSemCalibracao (
                                            Descricao, Codigo, PN, Fabricante, 
                                            Local, CadastroLocal, CodLocal, Status,
                                            Mecanico, Foto, CertificadoPDF
                                        ) VALUES (
                                            @p0, @p1, @p2, @p3, 
                                            @p4, @p5, @p6, @p7,
                                            @p8, @p9, @p10
                                        )";

                            using (var cmd = new SQLiteCommand(sql, conexao))
                            {
                                for (int j = 0; j < 11; j++)
                                    cmd.Parameters.AddWithValue($"@p{j}", dados[j]);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        transacao.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao importar dados_sem_calibracao.csv: " + ex.Message);
            }
        }
    }
}