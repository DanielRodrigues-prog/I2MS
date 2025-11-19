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
        public class ReqMov { public string Tipo { get; set; } public string IdFerramenta { get; set; } public string IdMecanico { get; set; } public string NomeMecanico { get; set; } }

        [HttpGet("VerificarMecanico/{id}")]
        public IActionResult VerMec(string id)
        {
            using (var c = GetConnection())
            {
                c.Open(); var res = new SqlCommand($"SELECT Nome FROM Mecanicos WHERE MecanicoID='{id}'", c).ExecuteScalar();
                return Ok(new { existe = res != null, nome = res?.ToString() });
            }
        }

        [HttpGet("StatusFerramenta/{id}")]
        public IActionResult VerFerr(string id)
        {
            using (var c = GetConnection())
            {
                c.Open();
                string mec = ""; bool achou = false;
                // Tenta tabela 1
                var res = new SqlCommand($"SELECT Mecanico FROM Instrumentos WHERE PN='{id}' OR SN='{id}' OR IdentifSOD='{id}' OR IdentifOficina='{id}'", c).ExecuteScalar();
                if (res != null) { mec = res.ToString(); achou = true; }
                // Tenta tabela 2
                if (!achou)
                {
                    res = new SqlCommand($"SELECT Mecanico FROM FerramentasSemCalibracao WHERE PN='{id}' OR Codigo='{id}'", c).ExecuteScalar();
                    if (res != null) { mec = res.ToString(); achou = true; }
                }
                return Ok(new { encontrada = achou, mecanicoAtual = mec });
            }
        }

        [HttpPost("Registrar")]
        public IActionResult Reg([FromBody] ReqMov req)
        {
            using (var c = GetConnection())
            {
                c.Open();
                string tab = "", mecAtual = ""; long idDb = -1;
                // Achar Ferramenta
                using (var r = new SqlCommand($"SELECT ID, Mecanico FROM Instrumentos WHERE PN='{req.IdFerramenta}' OR SN='{req.IdFerramenta}' OR IdentifSOD='{req.IdFerramenta}' OR IdentifOficina='{req.IdFerramenta}'", c).ExecuteReader()) if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"].ToString(); tab = "Instrumentos"; }
                if (tab == "") using (var r = new SqlCommand($"SELECT ID, Mecanico FROM FerramentasSemCalibracao WHERE PN='{req.IdFerramenta}' OR Codigo='{req.IdFerramenta}'", c).ExecuteReader()) if (r.Read()) { idDb = (int)r["ID"]; mecAtual = r["Mecanico"].ToString(); tab = "FerramentasSemCalibracao"; }

                if (tab == "") return BadRequest("Ferramenta não encontrada.");
                if (req.Tipo == "PEGAR" && !string.IsNullOrEmpty(mecAtual)) return BadRequest($"Está com {mecAtual}.");
                if (req.Tipo == "DEVOLVER" && string.IsNullOrEmpty(mecAtual)) return BadRequest("Já está livre.");

                using (var t = c.BeginTransaction())
                {
                    try
                    {
                        string novoMec = (req.Tipo == "PEGAR") ? req.NomeMecanico : "";
                        new SqlCommand($"UPDATE {tab} SET Mecanico='{novoMec}' WHERE ID={idDb}", c, t).ExecuteNonQuery();

                        string dt = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                        string ori = (tab == "Instrumentos") ? "COM" : "SEM";
                        if (req.Tipo == "PEGAR") new SqlCommand($"INSERT INTO Emprestimos (InstrumentoID,TabelaOrigem,MecanicoID,DataSaida) VALUES ({idDb},'{ori}','{req.IdMecanico}','{dt}')", c, t).ExecuteNonQuery();
                        else new SqlCommand($"UPDATE Emprestimos SET DataDevolucao='{dt}' WHERE InstrumentoID={idDb} AND TabelaOrigem='{ori}' AND DataDevolucao IS NULL", c, t).ExecuteNonQuery();

                        t.Commit(); return Ok("OK");
                    }
                    catch { t.Rollback(); return StatusCode(500, "Erro no banco"); }
                }
            }
        }
    }
}