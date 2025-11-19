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
            using (var c = GetConnection()) { c.Open(); new SqlCommand($"INSERT INTO Mecanicos (MecanicoID, Nome, StatusBloqueio) VALUES ('{m.MecanicoID}', '{m.Nome}', 'Ativo')", c).ExecuteNonQuery(); }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Del(string id)
        {
            using (var c = GetConnection()) { c.Open(); new SqlCommand($"DELETE FROM Mecanicos WHERE MecanicoID = '{id}'", c).ExecuteNonQuery(); }
            return Ok();
        }
    }
}