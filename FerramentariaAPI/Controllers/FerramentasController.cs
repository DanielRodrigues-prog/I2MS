using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FerramentariaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FerramentasController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public FerramentasController(IConfiguration configuration) { _configuration = configuration; }
        private SqlConnection GetConnection() => new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        // --- MODELS INTERNOS ---
        public class Instrumento
        {
            public int ID { get; set; }
            public string? InstrumentoNome { get; set; }
            public string? Modelo { get; set; }
            public string? PN { get; set; }
            public string? SN { get; set; }
            public string? IdentifSOD { get; set; }
            public string? IdentifOficina { get; set; }
            public string? Certificado { get; set; }
            public string? DataCalibracao { get; set; }
            public string? DataVencimento { get; set; }
            public string? Executante { get; set; }
            public string? Instalada { get; set; }
            public string? Local { get; set; }
            public string? SubLocalizacao { get; set; }
            public string? Observacoes { get; set; }
            public string? Mecanico { get; set; }
            public byte[]? Foto { get; set; }
            public byte[]? CertificadoPDF { get; set; }
        }
        public class SemCalibracao
        {
            public int ID { get; set; }
            public string? Descricao { get; set; }
            public string? Codigo { get; set; }
            public string? PN { get; set; }
            public string? Fabricante { get; set; }
            public string? Local { get; set; }
            public string? CadastroLocal { get; set; }
            public string? CodLocal { get; set; }
            public string? Status { get; set; }
            public string? Mecanico { get; set; }
            public byte[]? Foto { get; set; }
            public byte[]? CertificadoPDF { get; set; }
        }

        // --- COM CALIBRAÇÃO (CRUD COMPLETO) ---

        [HttpGet("ComCalibracao")]
        public IActionResult GetCom()
        {
            var l = new List<Instrumento>();
            using (var c = GetConnection())
            {
                c.Open();
                using (var r = new SqlCommand("SELECT * FROM Instrumentos", c).ExecuteReader())
                {
                    while (r.Read()) l.Add(new Instrumento
                    {
                        ID = (int)r["ID"],
                        InstrumentoNome = r["Instrumento"].ToString(),
                        Modelo = r["Modelo"].ToString(),
                        PN = r["PN"].ToString(),
                        SN = r["SN"].ToString(),
                        IdentifSOD = r["IdentifSOD"].ToString(),
                        IdentifOficina = r["IdentifOficina"].ToString(),
                        Certificado = r["Certificado"].ToString(),
                        DataCalibracao = r["DataCalibracao"].ToString(),
                        DataVencimento = r["DataVencimento"].ToString(),
                        Executante = r["Executante"].ToString(),
                        Instalada = r["Instalada"].ToString(),
                        Local = r["Local"].ToString(),
                        SubLocalizacao = r["SubLocalizacao"].ToString(),
                        Observacoes = r["Observacoes"].ToString(),
                        Mecanico = r["Mecanico"].ToString(),
                        Foto = r["Foto"] as byte[],
                        CertificadoPDF = r["CertificadoPDF"] as byte[]
                    });
                }
            }
            return Ok(l);
        }

        [HttpPost("ComCalibracao")]
        public IActionResult PostCom([FromBody] Instrumento i)
        {
            using (var c = GetConnection())
            {
                c.Open();
                var sql = "INSERT INTO Instrumentos (Instrumento, Modelo, PN, SN, IdentifSOD, IdentifOficina, Certificado, DataCalibracao, DataVencimento, Executante, Instalada, Local, SubLocalizacao, Observacoes, Mecanico, Foto, CertificadoPDF) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12,@p13,@p14,@p15,@p16,@p17)";
                using (var cmd = new SqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@p1", i.InstrumentoNome ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p2", i.Modelo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p3", i.PN ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p4", i.SN ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p5", i.IdentifSOD ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p6", i.IdentifOficina ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p7", i.Certificado ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p8", i.DataCalibracao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p9", i.DataVencimento ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p10", i.Executante ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p11", i.Instalada ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p12", i.Local ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p13", i.SubLocalizacao ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p14", i.Observacoes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p15", i.Mecanico ?? (object)DBNull.Value);
                    cmd.Parameters.Add("@p16", SqlDbType.VarBinary).Value = i.Foto ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p17", SqlDbType.VarBinary).Value = i.CertificadoPDF ?? (object)DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpPut("ComCalibracao/{id}")]
        public IActionResult PutCom(int id, [FromBody] Instrumento i)
        {
            using (var c = GetConnection())
            {
                c.Open();
                var sql = "UPDATE Instrumentos SET Instrumento=@p1, Modelo=@p2, PN=@p3, SN=@p4, IdentifSOD=@p5, IdentifOficina=@p6, Certificado=@p7, DataCalibracao=@p8, DataVencimento=@p9, Executante=@p10, Instalada=@p11, Local=@p12, SubLocalizacao=@p13, Observacoes=@p14, Mecanico=@p15 WHERE ID=@id";
                if (i.Foto != null) sql = sql.Replace("WHERE", ", Foto=@p16 WHERE");
                if (i.CertificadoPDF != null) sql = sql.Replace("WHERE", ", CertificadoPDF=@p17 WHERE");

                using (var cmd = new SqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@p1", i.InstrumentoNome ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p2", i.Modelo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p3", i.PN ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p4", i.SN ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p5", i.IdentifSOD ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p6", i.IdentifOficina ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p7", i.Certificado ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p8", i.DataCalibracao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p9", i.DataVencimento ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p10", i.Executante ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p11", i.Instalada ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p12", i.Local ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p13", i.SubLocalizacao ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p14", i.Observacoes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p15", i.Mecanico ?? (object)DBNull.Value);
                    if (i.Foto != null) cmd.Parameters.Add("@p16", SqlDbType.VarBinary).Value = i.Foto;
                    if (i.CertificadoPDF != null) cmd.Parameters.Add("@p17", SqlDbType.VarBinary).Value = i.CertificadoPDF;
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpDelete("ComCalibracao/{id}")]
        public IActionResult DelCom(int id)
        {
            try
            {
                // ✅ CORRIGIDO: Usar parâmetro @id
                using (var c = GetConnection()) { c.Open(); var cmd = new SqlCommand("DELETE FROM Instrumentos WHERE ID=@id", c); cmd.Parameters.AddWithValue("@id", id); int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                        return Ok(new { sucesso = true, mensagem = "Instrumento deletado com sucesso." });
                    else
                        return NotFound(new { erro = "Instrumento não encontrado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao deletar", detalhes = ex.Message });
            }
        }

        // --- SEM CALIBRAÇÃO (CRUD COMPLETO - ADICIONADO PUT/DELETE) ---

        [HttpGet("SemCalibracao")]
        public IActionResult GetSem()
        {
            var l = new List<SemCalibracao>();
            using (var c = GetConnection())
            {
                c.Open();
                using (var r = new SqlCommand("SELECT * FROM FerramentasSemCalibracao", c).ExecuteReader())
                {
                    while (r.Read()) l.Add(new SemCalibracao
                    {
                        ID = (int)r["ID"],
                        Descricao = r["Descricao"].ToString(),
                        Codigo = r["Codigo"].ToString(),
                        PN = r["PN"].ToString(),
                        Fabricante = r["Fabricante"].ToString(),
                        Local = r["Local"].ToString(),
                        CadastroLocal = r["CadastroLocal"].ToString(),
                        CodLocal = r["CodLocal"].ToString(),
                        Status = r["Status"].ToString(),
                        Mecanico = r["Mecanico"].ToString(),
                        Foto = r["Foto"] as byte[],
                        CertificadoPDF = r["CertificadoPDF"] as byte[]
                    });
                }
            }
            return Ok(l);
        }

        [HttpPost("SemCalibracao")]
        public IActionResult PostSem([FromBody] SemCalibracao i)
        {
            using (var c = GetConnection())
            {
                c.Open();
                var sql = "INSERT INTO FerramentasSemCalibracao (Descricao, Codigo, PN, Fabricante, Local, CadastroLocal, CodLocal, Status, Mecanico, Foto, CertificadoPDF) VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11)";
                using (var cmd = new SqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@p1", i.Descricao ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p2", i.Codigo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p3", i.PN ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p4", i.Fabricante ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p5", i.Local ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p6", i.CadastroLocal ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p7", i.CodLocal ?? (object)DBNull.Value); cmd.Parameters.AddWithValue("@p8", i.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p9", i.Mecanico ?? (object)DBNull.Value);
                    cmd.Parameters.Add("@p10", SqlDbType.VarBinary).Value = i.Foto ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p11", SqlDbType.VarBinary).Value = i.CertificadoPDF ?? (object)DBNull.Value;
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        // *** NOVOS MÉTODOS PARA SEM CALIBRAÇÃO ***
        [HttpPut("SemCalibracao/{id}")]
        public IActionResult PutSem(int id, [FromBody] SemCalibracao i)
        {
            using (var c = GetConnection())
            {
                c.Open();
                var sql = "UPDATE FerramentasSemCalibracao SET Descricao=@p1, Codigo=@p2, PN=@p3, Fabricante=@p4, Local=@p5, CadastroLocal=@p6, CodLocal=@p7, Status=@p8, Mecanico=@p9 WHERE ID=@id";
                if (i.Foto != null) sql = sql.Replace("WHERE", ", Foto=@p10 WHERE");
                if (i.CertificadoPDF != null) sql = sql.Replace("WHERE", ", CertificadoPDF=@p11 WHERE");

                using (var cmd = new SqlCommand(sql, c))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@p1", i.Descricao ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p2", i.Codigo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p3", i.PN ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p4", i.Fabricante ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p5", i.Local ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p6", i.CadastroLocal ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p7", i.CodLocal ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p8", i.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@p9", i.Mecanico ?? (object)DBNull.Value);
                    if (i.Foto != null) cmd.Parameters.Add("@p10", SqlDbType.VarBinary).Value = i.Foto;
                    if (i.CertificadoPDF != null) cmd.Parameters.Add("@p11", SqlDbType.VarBinary).Value = i.CertificadoPDF;
                    cmd.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpDelete("SemCalibracao/{id}")]
        public IActionResult DelSem(int id)
        {
            try
            {
                // ✅ CORRIGIDO: Usar parâmetro @id
                using (var c = GetConnection()) { c.Open(); var cmd = new SqlCommand("DELETE FROM FerramentasSemCalibracao WHERE ID=@id", c); cmd.Parameters.AddWithValue("@id", id); int linhasAfetadas = cmd.ExecuteNonQuery();

                    if (linhasAfetadas > 0)
                        return Ok(new { sucesso = true, mensagem = "Ferramenta deletada com sucesso." });
                    else
                        return NotFound(new { erro = "Ferramenta não encontrada." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = "Erro ao deletar", detalhes = ex.Message });
            }
        }
    }
}