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

        // --- DTOs (Classes de Dados) ---
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

        [HttpGet("VerificarMecanico/{id}")]
        public IActionResult VerMec(string id)
        {
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();
                    var res = new SqlCommand($"SELECT Nome FROM Mecanicos WHERE MecanicoID='{id}'", c).ExecuteScalar();
                    return Ok(new { existe = res != null, nome = res?.ToString() });
                }
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

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

                    var res = new SqlCommand($"SELECT Mecanico FROM Instrumentos WHERE PN='{id}' OR SN='{id}' OR IdentifSOD='{id}' OR IdentifOficina='{id}'", c).ExecuteScalar();
                    if (res != null) { mec = res.ToString(); achou = true; }

                    if (!achou)
                    {
                        res = new SqlCommand($"SELECT Mecanico FROM FerramentasSemCalibracao WHERE PN='{id}' OR Codigo='{id}'", c).ExecuteScalar();
                        if (res != null) { mec = res.ToString(); achou = true; }
                    }

                    return Ok(new { encontrada = achou, mecanicoAtual = mec });
                }
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpPost("Registrar")]
        public IActionResult Reg([FromBody] ReqMov req)
        {
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();
                    string tab = "", mecAtual = "";
                    long idDb = -1;

                    using (var r = new SqlCommand($"SELECT ID, Mecanico FROM Instrumentos WHERE PN='{req.IdFerramenta}' OR SN='{req.IdFerramenta}' OR IdentifSOD='{req.IdFerramenta}' OR IdentifOficina='{req.IdFerramenta}'", c).ExecuteReader())
                        if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"].ToString(); tab = "Instrumentos"; }

                    if (tab == "")
                        using (var r = new SqlCommand($"SELECT ID, Mecanico FROM FerramentasSemCalibracao WHERE PN='{req.IdFerramenta}' OR Codigo='{req.IdFerramenta}'", c).ExecuteReader())
                            if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"].ToString(); tab = "FerramentasSemCalibracao"; }

                    if (tab == "") return BadRequest("Ferramenta não encontrada.");
                    if (req.Tipo == "PEGAR" && !string.IsNullOrEmpty(mecAtual)) return BadRequest($"Já está com {mecAtual}.");
                    if (req.Tipo == "DEVOLVER" && string.IsNullOrEmpty(mecAtual)) return BadRequest("Já está livre.");

                    using (var t = c.BeginTransaction())
                    {
                        try
                        {
                            string novoMec = (req.Tipo == "PEGAR") ? req.NomeMecanico : "";
                            new SqlCommand($"UPDATE {tab} SET Mecanico='{novoMec}' WHERE ID={idDb}", c, t).ExecuteNonQuery();

                            // --- CORREÇÃO DA HORA (BRASIL) ---
                            DateTime horaUtc = DateTime.UtcNow;
                            TimeZoneInfo fusoBrasil;
                            try { fusoBrasil = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"); }
                            catch { fusoBrasil = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo"); }
                            DateTime horaBrasilia = TimeZoneInfo.ConvertTimeFromUtc(horaUtc, fusoBrasil);
                            string dt = horaBrasilia.ToString("dd/MM/yyyy HH:mm");
                            // ---------------------------------

                            string ori = (tab == "Instrumentos") ? "COM" : "SEM";

                            if (req.Tipo == "PEGAR")
                            {
                                // ✅ MUDANÇA: Adicionar coluna NomeMecanico
                                var cmdInsert = new SqlCommand(
                                    "INSERT INTO Emprestimos (InstrumentoID, TabelaOrigem, MecanicoID, NomeMecanico, DataSaida, Aeronave, UsuarioLiberou) " +
                                    "VALUES (@id, @ori, @mec, @nome, @dt, @aero, @admin)", c, t);
                                cmdInsert.Parameters.AddWithValue("@id", idDb);
                                cmdInsert.Parameters.AddWithValue("@ori", ori);
                                cmdInsert.Parameters.AddWithValue("@mec", req.IdMecanico ?? "0");
                                cmdInsert.Parameters.AddWithValue("@nome", req.NomeMecanico ?? ""); // ✅ NOVO
                                cmdInsert.Parameters.AddWithValue("@dt", dt);
                                cmdInsert.Parameters.AddWithValue("@aero", req.Aeronave ?? "");
                                cmdInsert.Parameters.AddWithValue("@admin", req.UsuarioLogado ?? "");
                                cmdInsert.ExecuteNonQuery();
                            }
                            else
                            {
                                var cmd = new SqlCommand($"UPDATE Emprestimos SET DataDevolucao=@dt WHERE InstrumentoID={idDb} AND TabelaOrigem='{ori}' AND DataDevolucao IS NULL", c, t);
                                cmd.Parameters.AddWithValue("@dt", dt);
                                cmd.ExecuteNonQuery();
                            }
                            t.Commit(); return Ok("OK");
                        }
                        catch { t.Rollback(); return StatusCode(500, "Erro no banco"); }
                    }
                }
            }
            catch (Exception ex) { return StatusCode(500, ex.Message); }
        }

        [HttpGet("Historico")]
        public IActionResult GetHistorico()
        {
            var lista = new List<HistoricoDTO>();
            try
            {
                using (var c = GetConnection())
                {
                    c.Open();

                    // ✅ MUDANÇA: Usar COALESCE para pegar o nome da tabela Mecanicos OU da coluna NomeMecanico
                    string sql = @"
                SELECT 
                    E.DataSaida, 
                    E.DataDevolucao, 
                    CASE WHEN E.TabelaOrigem = 'COM' THEN I.Instrumento ELSE F.Descricao END as Ferramenta, 
                    COALESCE(M.Nome, E.NomeMecanico) as Mecanico,
                    E.Aeronave, 
                    E.UsuarioLiberou 
                FROM Emprestimos E 
                LEFT JOIN Mecanicos M ON E.MecanicoID = M.MecanicoID 
                LEFT JOIN Instrumentos I ON E.InstrumentoID = I.ID AND E.TabelaOrigem = 'COM' 
                LEFT JOIN FerramentasSemCalibracao F ON E.InstrumentoID = F.ID AND E.TabelaOrigem = 'SEM' 
                ORDER BY E.ID DESC";

                    using (var r = new SqlCommand(sql, c).ExecuteReader())
                    {
                        while (r.Read())
                        {
                            lista.Add(new HistoricoDTO
                            {
                                Data = r["DataSaida"].ToString(),
                                Acao = "SAÍDA",
                                Ferramenta = r["Ferramenta"].ToString(),
                                Mecanico = r["Mecanico"].ToString(), // ✅ Agora pega o nome correto
                                Aeronave = r["Aeronave"].ToString(),
                                Admin = r["UsuarioLiberou"].ToString()
                            });

                            if (!string.IsNullOrEmpty(r["DataDevolucao"]?.ToString()))
                            {
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
                }
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}