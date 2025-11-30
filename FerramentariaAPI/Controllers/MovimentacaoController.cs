using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FerramentariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovimentacaoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MovimentacaoController(IConfiguration configuration) { _configuration = configuration; }
        private SqlConnection GetConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        // --- CLASSES DE DADOS (DTOs) ---
        public class ReqMov
        {
            public string? Tipo { get; set; }
            public string? IdFerramenta { get; set; }
            public string? IdMecanico { get; set; }
            public string? NomeMecanico { get; set; }
            public string? Aeronave { get; set; }
            public string? UsuarioLogado { get; set; }
        }

        public class HistoricoDTO
        {
            public string? Data { get; set; }
            public string? Acao { get; set; }
            public string? Ferramenta { get; set; }
            public string? Mecanico { get; set; }
            public string? Aeronave { get; set; }
            public string? Admin { get; set; }
        }

        // --- MÉTODOS (ENDPOINTS) ---

        // 1. Verificar se Mecânico existe
        [HttpGet("VerificarMecanico/{id}")]
        public IActionResult VerMec(string id)
        {
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();
                    // ✅ CORRIGIDO: Usar parâmetro @id em vez de interpolação
                    var cmd = new SqlCommand("SELECT Nome FROM Mecanicos WHERE MecanicoID=@id", c);
                    cmd.Parameters.AddWithValue("@id", id);
                    var res = cmd.ExecuteScalar();
                    return Ok(new { existe = res != null, nome = res?.ToString() });
                }
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        // 2. Verificar Status da Ferramenta (Se está livre ou ocupada)
        [HttpGet("StatusFerramenta/{id}")]
        public IActionResult VerFerr(string id)
        {
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();
                    string mec = "";
                    bool achou = false;

                    // ✅ CORRIGIDO: Usar parâmetros em vez de interpolação
                    var cmd1 = new SqlCommand("SELECT Mecanico FROM Instrumentos WHERE PN=@id OR SN=@id OR IdentifSOD=@id OR IdentifOficina=@id", c);
                    cmd1.Parameters.AddWithValue("@id", id);

                    var res = cmd1.ExecuteScalar();
                    if (res != null) { mec = res.ToString(); achou = true; }

                    // Tenta tabela 2 (Sem Calibração)
                    if (!achou)
                    {
                        var cmd2 = new SqlCommand("SELECT Mecanico FROM FerramentasSemCalibracao WHERE PN=@id OR Codigo=@id", c);
                        cmd2.Parameters.AddWithValue("@id", id);
                        res = cmd2.ExecuteScalar();
                        if (res != null) { mec = res.ToString(); achou = true; }
                    }

                    return Ok(new { encontrada = achou, mecanicoAtual = mec });
                }
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        // 3. Registrar Movimentação (Pegar/Devolver)
        [HttpPost("Registrar")]
        public IActionResult Reg([FromBody] ReqMov req)
        {
            try
            {
                // ✅ VALIDAÇÃO: Verificar entrada obrigatória
                if (string.IsNullOrWhiteSpace(req.Tipo) || string.IsNullOrWhiteSpace(req.IdFerramenta))
                    return BadRequest("Tipo e IdFerramenta são obrigatórios.");

                using (var c = GetConnection())
                {
                    c.Open();
                    string tab = "", mecAtual = "";
                    long idDb = -1;

                    // ✅ CORRIGIDO: Usar parâmetros em vez de interpolação
                    var cmdInst = new SqlCommand("SELECT ID, Mecanico FROM Instrumentos WHERE PN=@id OR SN=@id OR IdentifSOD=@id OR IdentifOficina=@id", c);
                    cmdInst.Parameters.AddWithValue("@id", req.IdFerramenta);

                    using (var r = cmdInst.ExecuteReader())
                        if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"]?.ToString() ?? ""; tab = "Instrumentos"; }

                    if (tab == "")
                    {
                        var cmdFerr = new SqlCommand("SELECT ID, Mecanico FROM FerramentasSemCalibracao WHERE PN=@id OR Codigo=@id", c);
                        cmdFerr.Parameters.AddWithValue("@id", req.IdFerramenta);

                        using (var r = cmdFerr.ExecuteReader())
                            if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"]?.ToString() ?? ""; tab = "FerramentasSemCalibracao"; }
                    }

                    // Validações
                    if (tab == "") return BadRequest("Ferramenta não encontrada.");
                    if (req.Tipo == "PEGAR" && !string.IsNullOrEmpty(mecAtual)) return BadRequest($"Ferramenta já está com {mecAtual}.");
                    if (req.Tipo == "DEVOLVER" && string.IsNullOrEmpty(mecAtual)) return BadRequest("Ferramenta já está livre.");

                    // Executa Transação
                    using (var t = c.BeginTransaction())
                    {
                        try
                        {
                            string novoMec = (req.Tipo == "PEGAR") ? req.NomeMecanico ?? "" : "";
                            // ✅ CORRIGIDO: Usar parâmetros para UPDATE
                            var cmdUpdate = new SqlCommand($"UPDATE {tab} SET Mecanico=@mec WHERE ID=@id", c, t);
                            cmdUpdate.Parameters.AddWithValue("@mec", novoMec);
                            cmdUpdate.Parameters.AddWithValue("@id", idDb);
                            cmdUpdate.ExecuteNonQuery();

                            string dt = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            string ori = (tab == "Instrumentos") ? "COM" : "SEM";

                            if (req.Tipo == "PEGAR")
                            {
                                // ✅ CORRIGIDO: Usar parâmetros para INSERT
                                var sql = "INSERT INTO Emprestimos (InstrumentoID, TabelaOrigem, MecanicoID, DataSaida, Aeronave, UsuarioLiberou) VALUES (@id, @ori, @mec, @dt, @aero, @admin)";
                                var cmd = new SqlCommand(sql, c, t);
                                cmd.Parameters.AddWithValue("@id", idDb);
                                cmd.Parameters.AddWithValue("@ori", ori);
                                cmd.Parameters.AddWithValue("@mec", req.IdMecanico ?? "");
                                cmd.Parameters.AddWithValue("@dt", dt);
                                cmd.Parameters.AddWithValue("@aero", req.Aeronave ?? "");
                                cmd.Parameters.AddWithValue("@admin", req.UsuarioLogado ?? "");
                                cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                // ✅ CORRIGIDO: Usar parâmetros para UPDATE (devolução)
                                var cmdDev = new SqlCommand("UPDATE Emprestimos SET DataDevolucao=@dt WHERE InstrumentoID=@id AND TabelaOrigem=@ori AND DataDevolucao IS NULL", c, t);
                                cmdDev.Parameters.AddWithValue("@dt", dt);
                                cmdDev.Parameters.AddWithValue("@id", idDb);
                                cmdDev.Parameters.AddWithValue("@ori", ori);
                                cmdDev.ExecuteNonQuery();
                            }
                            t.Commit();
                            return Ok(new { sucesso = true, mensagem = "Movimentação registrada com sucesso." });
                        }
                        catch (Exception ex)
                        {
                            t.Rollback();
                            return StatusCode(500, new { erro = "Erro ao registrar movimentação", detalhes = ex.Message });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro geral", detalhes = ex.Message });
            }
        }

        // 4. Obter Histórico Completo
        [HttpGet("Historico")]
        public IActionResult GetHistorico()
        {
            var lista = new List<HistoricoDTO>();
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();
                    string sql = @"SELECT E.DataSaida, E.DataDevolucao, 
                                   CASE WHEN E.TabelaOrigem = 'COM' THEN I.Instrumento ELSE F.Descricao END as Ferramenta, 
                                   M.Nome as Mecanico, E.Aeronave, E.UsuarioLiberou 
                                   FROM Emprestimos E 
                                   LEFT JOIN Mecanicos M ON E.MecanicoID = M.MecanicoID 
                                   LEFT JOIN Instrumentos I ON E.InstrumentoID = I.ID AND E.TabelaOrigem = 'COM' 
                                   LEFT JOIN FerramentasSemCalibracao F ON E.InstrumentoID = F.ID AND E.TabelaOrigem = 'SEM' 
                                   ORDER BY E.ID DESC";

                    using (var r = new SqlCommand(sql, c).ExecuteReader())
                    {
                        while (r.Read())
                        {
                            // Linha de SAÍDA
                            lista.Add(new HistoricoDTO
                            {
                                Data = r["DataSaida"].ToString(),
                                Acao = "SAÍDA",
                                Ferramenta = r["Ferramenta"].ToString(),
                                Mecanico = r["Mecanico"].ToString(),
                                Aeronave = r["Aeronave"].ToString(),
                                Admin = r["UsuarioLiberou"].ToString()
                            });

                            // Linha de DEVOLUÇÃO (se existir)
                            if (!string.IsNullOrEmpty(r["DataDevolucao"]?.ToString()))
                                lista.Add(new HistoricoDTO
                                {
                                    Data = r["DataDevolucao"].ToString(),
                                    Acao = "DEVOLUÇÃO",
                                    Ferramenta = r["Ferramenta"].ToString(),
                                    Mecanico = r["Mecanico"].ToString(),
                                    Aeronave = r["Aeronave"].ToString(),
                                    Admin = r["UsuarioLiberou"].ToString()
                                });
                        }
                    }
                }
                return Ok(lista.OrderByDescending(x => x.Data)); // Ordena por data
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }
    }
}