using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FerramentariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MecanicosController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public MecanicosController(IConfiguration configuration) { _configuration = configuration; }
        private SqlConnection GetConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        public class Mecanico { public string MecanicoID { get; set; } public string Nome { get; set; } public string StatusBloqueio { get; set; } }

        [HttpGet]
        public IActionResult GetAll()
        {
            var l = new List<Mecanico>();
            using (var c = GetConnection()) { c.Open(); using (var r = new SqlCommand("SELECT * FROM Mecanicos", c).ExecuteReader()) while (r.Read()) l.Add(new Mecanico { MecanicoID = r["MecanicoID"].ToString(), Nome = r["Nome"].ToString(), StatusBloqueio = r["StatusBloqueio"].ToString() }); }
            return Ok(l);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Mecanico m)
        {
            try
            {
                // ✅ VALIDAÇÃO: Verificar entrada obrigatória
                if (string.IsNullOrWhiteSpace(m.MecanicoID) || string.IsNullOrWhiteSpace(m.Nome))
                    return BadRequest(new { erro = "MecanicoID e Nome são obrigatórios." });

                using (var c = GetConnection())
                {
                    c.Open();
                    // ✅ CORRIGIDO: Usar parâmetros em vez de interpolação
                    var cmd = new SqlCommand("INSERT INTO Mecanicos (MecanicoID, Nome, StatusBloqueio) VALUES (@id, @nome, @status)", c);
                    cmd.Parameters.AddWithValue("@id", m.MecanicoID);
                    cmd.Parameters.AddWithValue("@nome", m.Nome);
                    cmd.Parameters.AddWithValue("@status", "Ativo");
                    cmd.ExecuteNonQuery();
                }
                return Ok(new { sucesso = true, mensagem = "Mecânico adicionado com sucesso." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao adicionar mecânico", detalhes = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Del(string id)
        {
            try
            {
                // ✅ VALIDAÇÃO: Verificar entrada obrigatória
                if (string.IsNullOrWhiteSpace(id))
                    return BadRequest(new { erro = "ID é obrigatório." });

                // ✅ CORRIGIDO: Usar parâmetro @id
                using (var c = GetConnection())
                {
                    c.Open();
                    var cmd = new SqlCommand("DELETE FROM Mecanicos WHERE MecanicoID=@id", c);
                    cmd.Parameters.AddWithValue("@id", id);
                    int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                        return Ok(new { sucesso = true, mensagem = "Mecânico deletado com sucesso." });
                    else
                        return NotFound(new { erro = "Mecânico não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao deletar mecânico", detalhes = ex.Message });
            }
        }
    }
}